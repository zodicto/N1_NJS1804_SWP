
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using ODTLearning.DAL.Entities;
using ODTLearning.BLL.Helpers;
using ODTLearning.Models;
using System.Globalization;
using System.Linq;
using System.Net;


namespace ODTLearning.BLL.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbminiCapstoneContext _context;
        public StudentRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }


        public async Task<ApiResponse<bool>> CreateRequestLearning(string id, RequestLearningModel model)
        {
            // Tìm sinh viên theo IdStudent
            var student = await _context.Accounts
                                  .Include(s => s.Requests)
                                  .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản nào với ID này!"
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
                    Message = "Chỉ có thể cập nhật yêu cầu ở trạng thái 'Chưa duyệt'!"
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

        public async Task<ApiResponse<bool>> DeleteRequestLearning(string accountId, string requestId)
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



       

        public async Task<ApiResponse<bool>> CreateComplaint(ComplaintModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.IdUser);

            if (user == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng",
                };
            }

            var accountTutor = await _context.Accounts.Include(x => x.Tutor).SingleOrDefaultAsync(x => x.Id == model.IdAccountTutor);

            if (accountTutor == null || accountTutor.Roles.ToLower() != "gia sư" || accountTutor.Tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư trong hệ thống",
                };
            }

            var tutorId = accountTutor.Tutor.Id;

            var complaint = new Complaint
            {
                Id = Guid.NewGuid().ToString(),
                Description = model.Description,
                IdAccount = model.IdUser,
                IdTutor = tutorId, // Sử dụng Id của Tutor
            };

            await _context.Complaints.AddAsync(complaint);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã tố cáo gia sư '{accountTutor.FullName}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = model.IdUser,
            };

            await _context.Notifications.AddAsync(nofi);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Thành công"
            };
        }


        public async Task<ApiResponse<bool>> CreateReviewRequest(ReviewRequestModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.IdUser);
            Console.WriteLine("userid : " + user?.Id);
            if (user == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng",
                };
            }

            var classRequest = await _context.ClassRequests.Include(x => x.IdRequestNavigation).SingleOrDefaultAsync(x => x.Id == model.IdClassRequest);

            if (classRequest == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp học",
                };
            }

            var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation)
                                             .FirstOrDefaultAsync(x => x.Id == classRequest.IdTutor && x.IdAccountNavigation.Roles.ToLower() == "gia sư");
            Console.WriteLine("tutorid : " + tutor?.Id);
            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư",
                };
            }

            var review = new Review
            {
                Id = Guid.NewGuid().ToString(),
                Feedback = model.FeedBack,
                Rating = model.Rating,
                IdAccount = model.IdUser,
                IdTutor = tutor.Id
            };

            await _context.Reviews.AddAsync(review);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã đánh giá gia sư '{tutor.IdAccountNavigation.FullName}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = model.IdUser,
            };

            await _context.Notifications.AddAsync(nofi);

            var nofiTutor = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn nhận được 1 đánh giá thông qua lớp '{classRequest.IdRequestNavigation.Title}'",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = tutor.IdAccount,
            };

            await _context.Notifications.AddAsync(nofiTutor);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Đánh giá thành công"
            };
        }

        public async Task<ApiResponse<bool>> CreateReviewService(ReviewServiceModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.IdUser);
            Console.WriteLine("userid : " + user?.Id);
            if (user == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng",
                };
            }

            var booking = await _context.Bookings.Include(x => x.IdTimeSlotNavigation).ThenInclude(x => x.IdDateNavigation).ThenInclude(x => x.IdServiceNavigation)
                .SingleOrDefaultAsync(x => x.Id == model.IdBooking);

            if (booking == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp học",
                };
            }

            var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation)
                                             .FirstOrDefaultAsync(x => x.Id == booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.IdTutor && x.IdAccountNavigation.Roles.ToLower() == "gia sư");
            Console.WriteLine("tutorid : " + tutor?.Id);
            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư",
                };
            }

            var review = new Review
            {
                Id = Guid.NewGuid().ToString(),
                Feedback = model.FeedBack,
                Rating = model.Rating,
                IdAccount = model.IdUser,
                IdTutor = tutor.Id
            };

            await _context.Reviews.AddAsync(review);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã đánh giá gia sư '{tutor.IdAccountNavigation.FullName}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = model.IdUser,
            };

            await _context.Notifications.AddAsync(nofi);

            var nofiTutor = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn nhận được 1 đánh giá thông qua lớp '{booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.Title}'",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = tutor.IdAccount,
            };

            await _context.Notifications.AddAsync(nofiTutor);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Đánh giá thành công"
            };
        }


        public async Task<ApiResponse<List<RequestLearningResponse>>> GetClassProcess(string id)
        {
            // Truy vấn danh sách các request có status là "Từ chối" và idAccount là accountId
            var requests = await _context.Requests
                .Include(r => r.IdSubjectNavigation)
                .Include(r => r.IdClassNavigation)
                .Where(r => r.IdAccount == id && r.Status == "Đang diễn ra")
                .ToListAsync();

            if (requests == null || !requests.Any())
            {
                return new ApiResponse<List<RequestLearningResponse>>
                {
                    Success = true,
                    Message = "Không tìm thấy yêu cầu nào với trạng thái 'Đang diễn ra' cho tài khoản này",
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
                Message = "Danh sách lớp đang diễn được truy xuất thành công",
                Data = requestLearningModels
            };
        }
        public async Task<ApiResponse<List<RequestLearningResponse>>> GetClassCompled(string accountId)
        {
            // Tìm kiếm tài khoản theo ID
            var account = await _context.Accounts
                .Include(a => a.Requests)
                    .ThenInclude(r => r.IdSubjectNavigation)
                .Include(a => a.Requests)
                    .ThenInclude(r => r.IdClassNavigation)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null)
            {
                return new ApiResponse<List<RequestLearningResponse>>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản với ID này",
                };
            }

            // Lọc các request có status là "Hoàn thành"
            var completedRequests = account.Requests
                .Where(r => r.Status == "Hoàn thành")
                .ToList();

            if (!completedRequests.Any())
            {
                return new ApiResponse<List<RequestLearningResponse>>
                {
                    Success = true,
                    Message = "Không tìm thấy yêu cầu nào với trạng thái 'Hoàn thành' cho tài khoản này",
                };
            }

            // Chuyển đổi danh sách requests thành danh sách RequestLearningModel
            var requestLearningModels = completedRequests.Select(r => new RequestLearningResponse
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
                Message = "Danh sách lớp hoàn thành được truy xuất thành công",
                Data = requestLearningModels
            };
        }

    }
}