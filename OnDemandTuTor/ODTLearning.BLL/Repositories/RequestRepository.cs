using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ODTLearning.BLL.Repositories
{
    public class RequestRepository
    {
        private readonly DbminiCapstoneContext _context;
        public RequestRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<bool>> CreateRequestLearning(string id, RequestLearningModel model)
        {
            // Tìm id của sinh viên trong bảng account
            var student = await _context.Accounts
                                  .Include(s => s.Requests)
                                  .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản !"
                };
            }

            // Tìm LearningModel theo tên
            var Class = await _context.Classes
                                              .FirstOrDefaultAsync(cl => cl.ClassName == model.Class);

            if (Class == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp nào với tên này. Vui lòng chọn lớp 10,11,12"
                };
            }
            var subjectModel = await _context.Subjects
                                              .FirstOrDefaultAsync(lm => lm.SubjectName == model.Subject);

            if (subjectModel == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy môn học nào với tên này. Vui lòng chọn lại!"
                };
            }

            // Validate và phân tích chuỗi thời gian để đảm bảo nó có định dạng đúng
            TimeOnly? parsedTimeStart = null;
            TimeOnly? parsedTimeEnd = null;
            if (!string.IsNullOrEmpty(model.TimeStart))
            {
                if (TimeOnly.TryParseExact(model.TimeStart, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                {
                    parsedTimeStart = time;
                }
                else
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Ngày bắt đầu sai định dạng hh:mm"
                    };
                }
            }
            if (!string.IsNullOrEmpty(model.TimeEnd))
            {
                if (TimeOnly.TryParseExact(model.TimeEnd, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                {
                    parsedTimeEnd = time;
                }
                else
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Ngày kết thúc sai định dạng hh:mm"
                    };
                }
            }



            // Tạo một đối tượng Schedule mới nếu có thông tin về lịch trình
            if (parsedTimeStart.HasValue && parsedTimeEnd.HasValue)
            {
                var requestOfStudent = new DAL.Entities.Request
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = model.Title,
                    CreateDate = DateTime.Now,
                    Price = model.Price,
                    TimeStart = parsedTimeStart,
                    TimeEnd = parsedTimeEnd,
                    TimeTable = model.TimeTable,
                    TotalSession = model.TotalSessions,
                    Description = model.Description,
                    Status = "Đang duyệt",
                    LearningMethod = model.LearningMethod,
                    IdAccount = id,
                    IdSubject = subjectModel.Id,
                    IdClass = Class.Id
                };

                // Thêm Request vào context
                await _context.Requests.AddAsync(requestOfStudent);

                var nofi = new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    Description = $"Bạn đã tạo yêu cầu tìm gia sư '{model.Title}' thành công",
                    CreateDate = DateTime.Now,
                    Status = "Chưa xem",
                    IdAccount = id,
                };

                await _context.Notifications.AddAsync(nofi);

                await _context.SaveChangesAsync();
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Tạo yêu cầu thành công. Yêu cầu của bạn đang chờ duyệt!",
            };
        }
        public async Task<ApiResponse<bool>> JoinRequest(string requestId, string id)
        {
            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(requestId) || string.IsNullOrEmpty(id))
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Yêu cầu không hợp lệ"
                };
            }

            // Tìm yêu cầu theo IdRequest
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
            if (request == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu nào"
                };
            }

            // Tìm account theo id và role là gia sư
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.Roles.ToLower() == "gia sư");
            if (account == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư nào với tài khoản này"
                };
            }

            // Tìm gia sư theo IdAccount
            var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.IdAccount == id);
            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư nào"
                };
            }

            var tutorId = tutor.Id;

            // Kiểm tra số dư tài khoản
            const float costPerService = 50000;
            int requestCount = await _context.RequestLearnings.Include(x => x.IdRequestNavigation).CountAsync(s => s.IdTutor == tutorId && s.IdRequestNavigation.Status.ToLower() == "đã duyệt");
            float remainingBalance = account.AccountBalance ?? 0;

            if (remainingBalance < costPerService)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Số dư tài khoản không đủ. Bạn cần ít nhất {costPerService} để tạo dịch vụ này.",
                };
            }

            int maxRequestThatCanBeJoined = (int)(remainingBalance / costPerService);
            int remainingRequestThatCanBeJoined = maxRequestThatCanBeJoined - requestCount;

            if (remainingRequestThatCanBeJoined <= 0)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Số dư tài khoản không đủ để tham gia thêm yêu cầu. Bạn đã tham gia {requestCount} yêu cầu.",
                };
            }

            // Kiểm tra xem gia sư đã tham gia yêu cầu này chưa
            var existingRequestLearning = await _context.RequestLearnings
                .FirstOrDefaultAsync(rl => rl.IdRequest == requestId && rl.IdTutor == tutorId);
            if (existingRequestLearning != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Gia sư đã tham gia vào yêu cầu này rồi"
                };
            }

            // Tạo bản ghi mới trong bảng RequestLearning
            var requestLearning = new RequestLearning
            {
                Id = Guid.NewGuid().ToString(),
                IdTutor = tutorId,
                IdRequest = requestId
            };

            // Cập nhật trạng thái yêu cầu

            _context.RequestLearnings.Add(requestLearning);
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã tham gia vào yêu cầu học tập '{request.Title}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = id,
            };

            await _context.Notifications.AddAsync(nofi);
            try
            {
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Bạn đã tham gia vào yêu cầu của học sinh"
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu có xảy ra
                Console.WriteLine($"Error while saving changes: {ex.Message}");
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lưu dữ liệu: " + ex.Message
                };
            }
        }

        public async Task<ApiResponse<List<RequestLearningResponse>>> GetPendingRequestsByAccountId(string accountId)
        {
            // Truy vấn danh sách các request có status là "Từ chối" và idAccount là accountId
            var requests = await _context.Requests
                .Include(r => r.IdSubjectNavigation)
                .Include(r => r.IdClassNavigation)
                .Where(r => r.IdAccount == accountId && r.Status == "Đang duyệt")
                .ToListAsync();

            if (requests == null || !requests.Any())
            {
                return new ApiResponse<List<RequestLearningResponse>>
                {
                    Success = true,
                    Message = "Không tìm thấy yêu cầu nào với trạng thái 'chờ duyệt' cho tài khoản này",
                };
            }

            // Chuyển đổi danh sách requests thành danh sách RequestLearningModel
            var requestLearningModels = requests.Select(r => new RequestLearningResponse
            {
                IdRequest = r.Id,
                Title = r.Title,
                Price = r.Price,
                Description = r.Description,
                Subject = r.IdSubjectNavigation?.SubjectName,
                LearningMethod = r.LearningMethod,
                Class = r.IdClassNavigation?.ClassName,
                TimeStart = r.TimeStart.HasValue ? r.TimeStart.Value.ToString("HH:mm") : null,
                TimeEnd = r.TimeEnd.HasValue ? r.TimeEnd.Value.ToString("HH:mm") : null,
                TimeTable = r.TimeTable,
                Status = r.Status,
                TotalSessions = r.TotalSession
            }).ToList();

            return new ApiResponse<List<RequestLearningResponse>>
            {
                Success = true,
                Message = "Danh sách yêu cầu dạy học được truy xuất thành công",
                Data = requestLearningModels
            };
        }
        public async Task<ApiResponse<List<RequestLearningResponse>>> GetApprovedRequestsByAccountId(string accountId)
        {
            // Truy vấn danh sách các request có status là "Từ chối" và idAccount là accountId
            var requests = await _context.Requests
                .Include(r => r.IdSubjectNavigation)
                .Include(r => r.IdClassNavigation)
                .Where(r => r.IdAccount == accountId && r.Status == "Đã duyệt")
                .ToListAsync();

            if (requests == null || !requests.Any())
            {
                return new ApiResponse<List<RequestLearningResponse>>
                {
                    Success = true,
                    Message = "Không tìm thấy yêu cầu nào với trạng thái 'đã duyệt' cho tài khoản này",
                    Data = null
                };
            }

            // Chuyển đổi danh sách requests thành danh sách RequestLearningModel
            var requestLearningModels = requests.Select(r => new RequestLearningResponse
            {
                IdRequest = r.Id,
                Title = r.Title,
                Price = r.Price,
                Description = r.Description,
                Subject = r.IdSubjectNavigation?.SubjectName,
                LearningMethod = r.LearningMethod,
                Class = r.IdClassNavigation?.ClassName,
                TimeStart = r.TimeStart.HasValue ? r.TimeStart.Value.ToString("HH:mm") : null,
                TimeEnd = r.TimeEnd.HasValue ? r.TimeEnd.Value.ToString("HH:mm") : null,
                TimeTable = r.TimeTable,
                Status = r.Status,
                TotalSessions = r.TotalSession
            }).ToList();

            return new ApiResponse<List<RequestLearningResponse>>
            {
                Success = true,
                Message = "Danh sách đã duyệt được truy xuất thành công",
                Data = requestLearningModels
            };
        }
        public async Task<ApiResponse<List<RequestLearningResponse>>> GetRejectRequestsByAccountId(string accountId)
        {
            // Truy vấn danh sách các request có status là "Từ chối" và idAccount là accountId
            var requests = await _context.Requests
                .Include(r => r.IdSubjectNavigation)
                .Include(r => r.IdClassNavigation)
                .Where(r => r.IdAccount == accountId && r.Status == "Từ chối")
                .ToListAsync();

            if (requests == null || !requests.Any())
            {
                return new ApiResponse<List<RequestLearningResponse>>
                {
                    Success = true,
                    Message = "Không tìm thấy yêu cầu nào với trạng thái 'Từ chối' cho tài khoản này",
                    Data = null
                };
            }

            // Chuyển đổi danh sách requests thành danh sách RequestLearningModel
            var requestLearningModels = requests.Select(r => new RequestLearningResponse
            {
                IdRequest = r.Id,
                Title = r.Title,
                Price = r.Price,
                Description = r.Description,
                Subject = r.IdSubjectNavigation?.SubjectName,
                LearningMethod = r.LearningMethod,
                Class = r.IdClassNavigation?.ClassName,
                TimeStart = r.TimeStart.HasValue ? r.TimeStart.Value.ToString("HH:mm") : null,
                TimeEnd = r.TimeEnd.HasValue ? r.TimeEnd.Value.ToString("HH:mm") : null,
                TimeTable = r.TimeTable,
                Status = r.Status,
                TotalSessions = r.TotalSession
            }).ToList();

            return new ApiResponse<List<RequestLearningResponse>>
            {
                Success = true,
                Message = "Danh sách yêu cầu từ chối xử lý đã được truy xuất thành công",
                Data = requestLearningModels
            };
        }
        public async Task<ApiResponse<bool>> UpdateRequestLearning(string requestId, RequestLearningModel model)
        {
            // Tìm request theo requestId
            var requestToUpdate = await _context.Requests
                                                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (requestToUpdate == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu nào với ID này!"
                };
            }

            // Kiểm tra trạng thái yêu cầu
            if (requestToUpdate.Status != "Đang duyệt")
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Chỉ có thể cập nhật yêu cầu ở trạng thái 'Đang duyệt'!"
                };
            }

            // Tìm Class theo tên nếu cần cập nhật
            var classEntity = await _context.Classes
                                            .FirstOrDefaultAsync(cl => cl.ClassName == model.Class);

            if (classEntity == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp nào với tên này. Vui lòng chọn lớp 10,11,12"
                };
            }

            // Tìm Subject theo tên nếu cần cập nhật
            var subjectEntity = await _context.Subjects
                                              .FirstOrDefaultAsync(s => s.SubjectName == model.Subject);

            if (subjectEntity == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy môn học nào với tên này. Vui lòng chọn lại!"
                };
            }

            // Validate và phân tích chuỗi thời gian để đảm bảo nó có định dạng đúng
            TimeOnly? parsedTimeStart = null;
            TimeOnly? parsedTimeEnd = null;
            if (!string.IsNullOrEmpty(model.TimeStart))
            {
                if (TimeOnly.TryParseExact(model.TimeStart, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                {
                    parsedTimeStart = time;
                }
                else
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Ngày bắt đầu sai định dạng hh:mm"
                    };
                }
            }
            if (!string.IsNullOrEmpty(model.TimeEnd))
            {
                if (TimeOnly.TryParseExact(model.TimeEnd, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time))
                {
                    parsedTimeEnd = time;
                }
                else
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Ngày kết thúc sai định dạng hh:mm"
                    };
                }
            }

            // Cập nhật các thuộc tính của request từ model
            requestToUpdate.Title = model.Title ?? requestToUpdate.Title;
            requestToUpdate.Price = model.Price ?? requestToUpdate.Price;
            requestToUpdate.TimeStart = parsedTimeStart ?? requestToUpdate.TimeStart;
            requestToUpdate.TimeEnd = parsedTimeEnd ?? requestToUpdate.TimeEnd;
            requestToUpdate.TimeTable = model.TimeTable ?? requestToUpdate.TimeTable;
            requestToUpdate.TotalSession = model.TotalSessions ?? requestToUpdate.TotalSession;
            requestToUpdate.Description = model.Description ?? requestToUpdate.Description;
            requestToUpdate.LearningMethod = model.LearningMethod ?? requestToUpdate.LearningMethod;
            requestToUpdate.IdSubject = subjectEntity.Id;
            requestToUpdate.IdClass = classEntity.Id;

            // Lưu các thay đổi vào context
            _context.Requests.Update(requestToUpdate);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã cập nhật yêu cầu tìm gia sư '{requestToUpdate.Title}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = requestToUpdate.IdAccount,
            };

            await _context.Notifications.AddAsync(nofi);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Cập nhật yêu cầu thành công",
            };
        }

        public async Task<ApiResponse<bool>> DeleteRequestByStudent(string accountId, string requestId)
        {
            // Tìm tài khoản theo accountId
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản nào với ID này!"
                };
            }

            // Tìm request theo requestId và accountId
            var requestToDelete = await _context.Requests
                                                 .FirstOrDefaultAsync(r => r.Id == requestId && r.IdAccount == accountId);

            if (requestToDelete == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu nào với ID này hoặc yêu cầu không thuộc về tài khoản này!"
                };
            }

            // Kiểm tra nếu request tồn tại trong bảng RequestLearning
            var requestInRequestLearning = await _context.RequestLearnings
                                                         .AnyAsync(rl => rl.IdRequest == requestId);

            if (requestInRequestLearning)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Đã có gia sư tham gia vào yêu cầu. Không thể xóa request."
                };
            }

            _context.Requests.Remove(requestToDelete);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã xóa yêu cầu tìm gia sư '{requestToDelete.Title}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = accountId,
            };

            await _context.Notifications.AddAsync(nofi);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Yêu cầu đã được xóa thành công",
            };
        }

        public async Task<ApiResponse<bool>> DeleteRequestByModerator(string idRequest)
        {
            var request = await _context.Requests.SingleOrDefaultAsync(x => x.Id == idRequest);

            if (request == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu"
                };
            }

            if (request.Status.ToLower() == "đang diễn ra")
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Yêu cầu đang diễn ra"
                };
            }

            var requestLearnings = await _context.RequestLearnings.Where(x => x.IdRequest == idRequest).ToListAsync();

            if (requestLearnings.Any())
            {
                _context.RequestLearnings.RemoveRange(requestLearnings);
            }

            var classRequests2 = await _context.ClassRequests.Where(x => x.IdRequest == idRequest).ToListAsync();

            if (classRequests2.Any())
            {
                _context.ClassRequests.RemoveRange(classRequests2);
            }

            _context.Requests.Remove(request);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Yêu cầu tìm gia sư '{request.Title}' của bạn đã bị xóa",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = request.IdAccount,
            };

            await _context.Notifications.AddAsync(nofi);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Xóa yêu cầu thành công"
            };
        }

        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetAllPendingRequests()
        {
            // Truy vấn danh sách các request có status là "chưa duyệt"
            var pendingRequests = await _context.Requests
                .Where(r => r.Status == "đang duyệt")
                .Select(r => new ViewRequestOfStudent
                {
                    Id = r.IdAccountNavigation.Id,
                    Title = r.Title,
                    Price = r.Price,
                    TotalSessions = r.TotalSession,
                    TimeTable = r.TimeTable,
                    Description = r.Description,
                    Subject = r.IdSubjectNavigation.SubjectName,
                    LearningMethod = r.LearningMethod,
                    Class = r.IdClassNavigation.ClassName,
                    TimeStart = r.TimeStart.HasValue ? r.TimeStart.Value.ToString("HH:mm") : null,
                    TimeEnd = r.TimeEnd.HasValue ? r.TimeEnd.Value.ToString("HH:mm") : null,
                    IdRequest = r.Id,
                    Status = r.Status,
                    FullName = r.IdAccountNavigation.FullName // Include Account Full Name
                }).ToListAsync();

            return new ApiResponse<List<ViewRequestOfStudent>>
            {
                Success = true,
                Message = "Yêu cầu đang chờ xử lý được truy xuất thành công",
                Data = pendingRequests
            };
        }
        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetAllApprovedRequests(string id)
        {
            var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.IdAccount == id);

            var pendingRequests = await _context.Requests.Include(r => r.RequestLearnings)
                                                   .Where(r => r.Status == "Đã duyệt")
                                                   .Select(r => new ViewRequestOfStudent
                                                   {
                                                       Id = r.IdAccountNavigation.Id,
                                                       Title = r.Title,
                                                       Status = r.Status,
                                                       Price = r.Price,
                                                       Description = r.Description,
                                                       Subject = r.IdSubjectNavigation.SubjectName,
                                                       LearningMethod = r.LearningMethod,
                                                       Class = r.IdClassNavigation.ClassName,
                                                       TimeTable = r.TimeTable,
                                                       TotalSessions = r.TotalSession,
                                                       TimeStart = r.TimeStart.ToString(), // Assuming you have TimeStart and TimeEnd in your Schedule model
                                                       TimeEnd = r.TimeEnd.ToString(),
                                                       IdRequest = r.Id, // Include Account ID
                                                       FullName = r.IdAccountNavigation.FullName, // Include Account Full Name
                                                       Current = tutor != null ? (r.RequestLearnings.FirstOrDefault(rl => rl.IdTutor == tutor.Id) == null ? "Chưa nhận" : "Đã nhận") : null
                                                   }).ToListAsync();

            // Format the Time string if needed
            foreach (var request in pendingRequests)
            {
                if (!string.IsNullOrEmpty(request.TimeStart))
                {
                    var timeOnly = TimeOnly.Parse(request.TimeStart);
                    request.TimeStart = timeOnly.ToString("HH:mm");
                }
            }
            foreach (var request in pendingRequests)
            {
                if (!string.IsNullOrEmpty(request.TimeEnd))
                {
                    var timeOnly = TimeOnly.Parse(request.TimeEnd);
                    request.TimeEnd = timeOnly.ToString("HH:mm");
                }
            }
            return new ApiResponse<List<ViewRequestOfStudent>>
            {
                Success = true,
                Message = "Yêu cầu đã xử lý được truy xuất thành công",
                Data = pendingRequests
            };
        }
        public async Task<ApiResponse<bool>> ApproveRequest(string requestId)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu nào",
                    Data = false
                };
            }

            request.Status = "Đã duyệt";
            _context.Requests.Update(request);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Yêu cầu tìm gia sư '{request.Title}' của bạn đã được duyệt",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = request.IdAccount,
            };

            await _context.Notifications.AddAsync(nofi);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Yêu cầu đã được duyệt",
            };
        }

        public async Task<ApiResponse<bool>> RejectRequest(string requestId, ReasonReject model)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
            {
                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Không tìm thấy yêu cầu nào",
                    Data = false
                };
            }

            request.Status = "Từ chối"; // Assuming "Rejected" is the correct status for rejection
            request.Reason = model.reason;
            _context.Requests.Update(request);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Yêu cầu tìm gia sư '{request.Title}' của bạn đã bị từ chối",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = request.IdAccount,
            };

            await _context.Notifications.AddAsync(nofi);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Yêu cầu của bạn không được duyệt",
                Data = true
            };
        }
        public async Task<ApiResponse<List<TutorListModel>>> GetAllTutorJoinRequest(string requestId)
        {
            // Tìm request theo requestId
            var request = await _context.Requests
                                        .Include(r => r.RequestLearnings)
                                        .ThenInclude(rl => rl.IdTutorNavigation)
                                            .ThenInclude(t => t.EducationalQualifications)
                                        .Include(r => r.RequestLearnings)
                                        .ThenInclude(rl => rl.IdTutorNavigation)
                                            .ThenInclude(t => t.IdAccountNavigation)
                                        .Include(r => r.RequestLearnings)
                                        .ThenInclude(rl => rl.IdTutorNavigation)
                                            .ThenInclude(t => t.TutorSubjects)
                                                .ThenInclude(ts => ts.IdSubjectNavigation)
                                        .Include(r => r.RequestLearnings)
                                        .ThenInclude(rl => rl.IdTutorNavigation)
                                            .ThenInclude(t => t.Reviews)
                                        .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
            {
                return new ApiResponse<List<TutorListModel>>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu nào với ID này",
                };
            }

            var classRequest = await _context.ClassRequests.FirstOrDefaultAsync(x => x.IdRequest == requestId);

            // Lấy danh sách gia sư tham gia yêu cầu
            var tutors = request.RequestLearnings.Where(x => x.IdTutor != classRequest?.IdTutor).Select(rl => new TutorListModel
            {
                id = rl.IdTutorNavigation.IdAccount,
                fullName = rl.IdTutorNavigation.IdAccountNavigation.FullName,
                introduction = rl.IdTutorNavigation.Introduction,
                gender = rl.IdTutorNavigation.IdAccountNavigation.Gender,
                avatar = rl.IdTutorNavigation.IdAccountNavigation.Avatar, // Bổ sung thuộc tính avatar
                specializedSkills = rl.IdTutorNavigation.SpecializedSkills,
                experience = rl.IdTutorNavigation.Experience,
                subject = string.Join("; ", _context.Tutors.Where(t => t.Id == rl.IdTutor)
                                                    .Join(_context.TutorSubjects.Join(_context.Subjects, ts => ts.IdSubject, s => s.Id, (ts, s) => new
                                                    {
                                                        TutorId = ts.IdTutor,
                                                        SubjectName = s.SubjectName
                                                    }), t => t.Id, ts => ts.TutorId, (t, ts) => ts.SubjectName)
                                                    .ToList()),
                imageQualification = rl.IdTutorNavigation.EducationalQualifications.FirstOrDefault()?.Img,
                qualifiCationName = rl.IdTutorNavigation.EducationalQualifications.FirstOrDefault()?.QualificationName,
                rating = rl.IdTutorNavigation.Reviews.Average(r => r.Rating) ?? 0,
            }).ToList();

            return new ApiResponse<List<TutorListModel>>
            {
                Success = true,
                Message = "Danh sách gia sư đã tham gia vào yêu cầu",
                Data = tutors
            };
        }

        public async Task<ApiResponse<SelectTutorModel>> SelectTutor(string idRequest, string idAccountTutor)
        {
            var request = await _context.Requests.SingleOrDefaultAsync(x => x.Id == idRequest);

            if (request == null)
            {
                return new ApiResponse<SelectTutorModel>
                {
                    Success = false,
                    Message = "Không tìm thấy request trong hệ thống",
                };
            }

            var existclassRequest = await _context.ClassRequests.SingleOrDefaultAsync(x => x.IdRequest == idRequest);

            if (existclassRequest != null)
            {
                return new ApiResponse<SelectTutorModel>
                {
                    Success = false,
                    Message = "Bạn đã chọn gia sư cho yêu cầu này",
                };
            }

            var accountTutor = await _context.Accounts.Include(x => x.Tutor).SingleOrDefaultAsync(x => x.Id == idAccountTutor);

            if (accountTutor == null || accountTutor.Roles.ToLower() != "gia sư" || accountTutor.Tutor == null)
            {
                return new ApiResponse<SelectTutorModel>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư trong hệ thống",
                };
            }

            var tutor = accountTutor.Tutor;

            var user = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == request.IdAccount);

            //if (user.AccountBalance < request.Price)
            //{
            //    return new ApiResponse<SelectTutorModel>
            //    {
            //        Success = false,
            //        Message = "Tài khoản user không đủ tiền yêu cầu",
            //        Data = null
            //    };
            //}            

            var rent = new Rent
            {
                Id = Guid.NewGuid().ToString(),
                Price = request.Price,
                CreateDate = DateTime.Now,
                IdAccount = request.IdAccount,
                IdTutor = tutor.Id
            };

            var classRequest = new ClassRequest
            {
                Id = Guid.NewGuid().ToString(),
                IdRequest = idRequest,
                IdTutor = tutor.Id
            };

            //user.AccountBalance = user.AccountBalance - request.Price;
            tutor.IdAccountNavigation.AccountBalance = tutor.IdAccountNavigation.AccountBalance - 50000;
            var accountAdmin = await _context.Accounts
                .SingleOrDefaultAsync(x => x.Roles.ToLower() == "quản trị viên");

            accountAdmin.AccountBalance += 50000;
            request.Status = "Đang diễn ra";
            await _context.AddAsync(rent);
            await _context.AddAsync(classRequest);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã chọn gia sư '{accountTutor.FullName}' cho yêu cầu tìm gia sư '{request.Title}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = request.IdAccount,
            };

            await _context.Notifications.AddAsync(nofi);

            var nofiSelectedTutor = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã được chọn cho yêu cầu tìm gia sư '{request.Title}'",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = accountTutor.Id,
            };

            await _context.Notifications.AddAsync(nofiSelectedTutor);

            var requestLearnings = await _context.RequestLearnings.Include(x => x.IdTutorNavigation).Where(x => x.IdRequest == idRequest && x.IdTutor != tutor.Id).ToListAsync();

            if (requestLearnings.Any())
            {
                foreach (var requestLearning in requestLearnings)
                {
                    var nofiTutor = new Notification
                    {
                        Id = Guid.NewGuid().ToString(),
                        Description = $"Bạn không được chọn cho yêu cầu tìm gia sư '{request.Title}'",
                        CreateDate = DateTime.Now,
                        Status = "Chưa xem",
                        IdAccount = requestLearning.IdTutorNavigation.IdAccount,
                    };

                    await _context.Notifications.AddAsync(nofiTutor);
                }
            }

            await _context.SaveChangesAsync();

            var data = new SelectTutorModel
            {
                Tutor = new
                {
                    Name = tutor.IdAccountNavigation.FullName,
                    Email = tutor.IdAccountNavigation.Email,
                    DateOfBirth = tutor.IdAccountNavigation.DateOfBirth,
                    Gender = tutor.IdAccountNavigation.Gender,
                    Avatar = tutor.IdAccountNavigation.Avatar,
                    Address = tutor.IdAccountNavigation.Address,
                    Phone = tutor.IdAccountNavigation.Phone
                },
                User = new
                {
                    Name = user.FullName,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    Avatar = user.Avatar,
                    Address = user.Address,
                    Phone = user.Phone
                }
            };

            return new ApiResponse<SelectTutorModel>
            {
                Success = true,
                Message = "Bạn đã chấp nhận gia sư cho yêu câu của mình.",
                Data = data
            };
        }


        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestPending()
        {
            try
            {
                var ListRequestPending = await _context.Requests
                    .Include(t => t.IdAccountNavigation)
                    .Include(t => t.IdClassNavigation)
                    .Include(t => t.IdSubjectNavigation)
                    .Where(t => t.Status == "Đang duyệt")
                    .Select(t => new ViewRequestOfStudent
                    {
                        IdRequest = t.Id,
                        Title = t.Title,
                        Price = t.Price,
                        Class = t.IdClassNavigation.ClassName,
                        TimeStart = t.TimeStart.HasValue ? t.TimeStart.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeEnd = t.TimeEnd.HasValue ? t.TimeEnd.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeTable = t.TimeTable,
                        TotalSessions = t.TotalSession,
                        Subject = t.IdSubjectNavigation.SubjectName,
                        FullName = t.IdAccountNavigation.FullName,
                        Description = t.Description,
                        Status = t.Status,
                        LearningMethod = t.LearningMethod,
                    }).ToListAsync();
                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = true,
                    Message = "Lấy danh sách yêu cầu chưa duyệt thành công",
                    Data = ListRequestPending
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListRequestPending: {ex.Message}");

                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách yêu cầu",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestApproved()
        {
            try
            {
                var ListRequestPending = await _context.Requests
                    .Include(t => t.IdAccountNavigation)
                    .Include(t => t.IdClassNavigation)
                    .Include(t => t.IdSubjectNavigation)
.Where(t => t.Status == "Đã duyệt")
                    .Select(t => new ViewRequestOfStudent
                    {
                        IdRequest = t.Id,
                        Title = t.Title,
                        Price = t.Price,
                        Class = t.IdClassNavigation.ClassName,
                        TimeStart = t.TimeStart.HasValue ? t.TimeStart.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeEnd = t.TimeEnd.HasValue ? t.TimeEnd.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeTable = t.TimeTable,
                        TotalSessions = t.TotalSession,
                        Subject = t.IdSubjectNavigation.SubjectName,
                        FullName = t.IdAccountNavigation.FullName,
                        Description = t.Description,
                        Status = t.Status,
                        LearningMethod = t.LearningMethod,
                    }).ToListAsync();
                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = true,
                    Message = "Lấy danh sách đã duyệt thành công",
                    Data = ListRequestPending
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListRequestPending: {ex.Message}");

                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách yêu cầu",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestReject()
        {
            try
            {
                var ListRequestPending = await _context.Requests
                    .Include(t => t.IdAccountNavigation)
                    .Include(t => t.IdClassNavigation)
                    .Include(t => t.IdSubjectNavigation)
                    .Where(t => t.Status == "Từ chối")
                    .Select(t => new ViewRequestOfStudent
                    {
                        IdRequest = t.Id,
                        Title = t.Title,
                        Price = t.Price,
                        Class = t.IdClassNavigation.ClassName,
                        TimeStart = t.TimeStart.HasValue ? t.TimeStart.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeEnd = t.TimeEnd.HasValue ? t.TimeEnd.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeTable = t.TimeTable,
                        TotalSessions = t.TotalSession,
                        Subject = t.IdSubjectNavigation.SubjectName,
                        FullName = t.IdAccountNavigation.FullName,
                        Description = t.Description,
                        Status = t.Status,
                        LearningMethod = t.LearningMethod,
                    }).ToListAsync();
                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = true,
                    Message = "Lấy danh sách yêu cầu bị từ chối thành công",
                    Data = ListRequestPending
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListRequestPending: {ex.Message}");

                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách yêu cầu",
                    Data = null
                };
            }
        }

    }
}
