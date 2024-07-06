using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using ODTLearning.Helpers;
using ODTLearning.Models;
using System.Globalization;
using System.Net;


namespace ODTLearning.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbminiCapstoneContext _context;
        public StudentRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        ImageLibrary imgLib = new ImageLibrary();

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
                var requestOfStudent = new Request
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
                await _context.SaveChangesAsync();
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Tạo yêu cầu thành công",
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



        public async Task<ApiResponse<List<TutorListModel>>> ViewAllTutorJoinRequest(string requestId)
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
                                        .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
            {
                return new ApiResponse<List<TutorListModel>>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu nào với ID này",
                };
            }

            // Lấy danh sách gia sư tham gia yêu cầu
            var tutors = request.RequestLearnings.Select(rl => new TutorListModel
            {
                id = rl.IdTutorNavigation.IdAccount,
                fullName = rl.IdTutorNavigation.IdAccountNavigation.FullName,
                introduction = rl.IdTutorNavigation.Introduction,
                gender = rl.IdTutorNavigation.IdAccountNavigation.Gender,
                avatar = rl.IdTutorNavigation.IdAccountNavigation.Avatar, // Bổ sung thuộc tính avatar
                specializedSkills = rl.IdTutorNavigation.SpecializedSkills,
                experience = rl.IdTutorNavigation.Experience,
                subject = rl.IdTutorNavigation.TutorSubjects.FirstOrDefault()?.IdSubjectNavigation.SubjectName,
                imageQualification = rl.IdTutorNavigation.EducationalQualifications.FirstOrDefault()?.Img,
                qualifiCationName = rl.IdTutorNavigation.EducationalQualifications.FirstOrDefault()?.QualificationName
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
            request.Status = "Đang diễn ra";
            await _context.AddAsync(rent);
            await _context.AddAsync(classRequest);
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
                Message = "Thành công",
                Data = data
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

            var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation).FirstOrDefaultAsync(x => x.Id == model.IdAccountTutor && x.IdAccountNavigation.Roles.ToLower() == "gia sư");

            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư",
                };
            }

            var complaint = new Complaint
            {
                Id = Guid.NewGuid().ToString(),
                Description = model.Description,
                IdAccount = model.IdUser,
                IdTutor = tutor.Id,
            };

            await _context.Complaints.AddAsync(complaint);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Thành công"
            };
        }

        public async Task<ApiResponse<bool>> CreateReview(ReviewModel model)
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

            var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation).FirstOrDefaultAsync(x => x.Id == model.IdAccountTutor && x.IdAccountNavigation.Roles.ToLower() == "gia sư");

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
                IdAccount = model.IdUser,
                IdTutor = tutor.Id,
            };

            await _context.Reviews.AddAsync(review);
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


        public async Task<ApiResponse<object>> GetSignUpTutor(string id)
        {
            var tutor = await _context.Tutors.Include(t => t.IdAccountNavigation).FirstOrDefaultAsync(t => t.IdAccount == id);

            if (tutor == null)
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Không có đơn đăng ký gia sư"
                };
            }

            var subjects = await _context.Tutors.Where(x => x.IdAccount == id).Join(_context.TutorSubjects.Join(_context.Subjects, tf => tf.IdSubject, f => f.Id, (tf, f) => new
            {
                AccountId = tf.IdTutor,
                Field = f.SubjectName
            }), t => t.Id, af => af.AccountId, (t, af) => af.Field).ToListAsync();

            var qualifications = await _context.Tutors.Where(x => x.IdAccount == id).Join(_context.EducationalQualifications, t => t.Id, eq => eq.IdTutor, (t, eq) => new
            {
                Id = eq.Id,
                Name = eq.QualificationName,
                Img = eq.Img,
                Type = eq.Type
            }).ToListAsync();

            var data = new
            {
                Id = tutor.IdAccount,
                tutor.IdAccountNavigation.FullName,
                tutor.IdAccountNavigation.Gender,
                Date_of_birth = tutor.IdAccountNavigation.DateOfBirth,
                tutor.IdAccountNavigation.Email,
                tutor.IdAccountNavigation.Avatar,
                tutor.IdAccountNavigation.Address,
                tutor.IdAccountNavigation.Phone,
                tutor.SpecializedSkills,
                tutor.Introduction,
                tutor.Experience,
                Subjects = subjects,
                Qualifications = qualifications,
                tutor.Status
            };

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Lấy danh sách gia sư thành công",
                Data = data
            };
        }
        public async Task<ApiResponse<BookingServiceModel>> BookingServiceLearning(string id, string idService, BookingServiceLearingModels model)
        {
            // Tìm tài khoản theo id
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
            if (account == null)
            {
                return new ApiResponse<BookingServiceModel>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản nào với ID này!",
                };
            }
            Console.WriteLine("Account ID: " + account.Id);

            // Tìm dịch vụ theo idService
            var service = await _context.Services
                                        .Include(s => s.IdTutorNavigation)
                                        .ThenInclude(t => t.IdAccountNavigation)
                                        .Include(s => s.Dates)
                                        .ThenInclude(d => d.TimeSlots)
                                        .FirstOrDefaultAsync(s => s.Id == idService);
            if (service == null)
            {
                return new ApiResponse<BookingServiceModel>
                {
                    Success = false,
                    Message = "Không tìm thấy dịch vụ nào với ID này!",
                };
            }
            Console.WriteLine("Service ID: " + service.Id);

            // Tìm ngày và khung giờ phù hợp
            var selectedDate = service.Dates.FirstOrDefault(d => d.Date1 == model.date);
            if (selectedDate == null)
            {
                return new ApiResponse<BookingServiceModel>
                {
                    Success = false,
                    Message = "Không tìm thấy ngày nào phù hợp với dịch vụ này!",
                };
            }
            Console.WriteLine("Selected Date ID: " + selectedDate.Id);
            Console.WriteLine("Service ID from Date: " + selectedDate.IdService);

            // Tìm khung giờ dựa vào ID của date và so sánh với bảng timeslot
            foreach (var ts in selectedDate.TimeSlots)
            {
                var timeSlot24Hour = DateTime.Parse(ts.TimeSlot1.ToString()).ToString("HH:mm");
                Console.WriteLine("TimeSlot1 (24-hour): " + timeSlot24Hour);
                Console.WriteLine("model.timeAvalable: " + model.timeAvalable);

                if (timeSlot24Hour == model.timeAvalable)
                {
                    Console.WriteLine("Matched TimeSlot: " + ts.Id);

                    var selectedTimeSlot = ts;

                    // Tạo đối tượng Booking mới
                    var newBooking = new Booking
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdAccount = id,
                        Duration = model.Duration,
                        Price = model.Price,
                        Status = "Đang diễn ra",
                        IdTimeSlot = selectedTimeSlot.Id
                    };

                    // Thêm Booking vào context
                    await _context.Bookings.AddAsync(newBooking);

                    // Cập nhật status của TimeSlot
                    selectedTimeSlot.Status = "Đã đặt";

                    // Trừ 50000 từ AccountBalance của tutor
                    service.IdTutorNavigation.IdAccountNavigation.AccountBalance -= 50000;
                    Console.WriteLine("service.IdTutorNavigation.IdAccountNavigation.AccountBalance: " + service.IdTutorNavigation.IdAccountNavigation.AccountBalance);

                    // Lưu thay đổi vào context
                    await _context.SaveChangesAsync();

                    // Chuẩn bị dữ liệu trả về
                    var data = new BookingServiceModel
                    {
                        Tutor = new
                        {
                            Name = service.IdTutorNavigation.IdAccountNavigation.FullName,
                            Email = service.IdTutorNavigation.IdAccountNavigation.Email,
                            DateOfBirth = service.IdTutorNavigation.IdAccountNavigation.DateOfBirth,
                            Gender = service.IdTutorNavigation.IdAccountNavigation.Gender,
                            Avatar = service.IdTutorNavigation.IdAccountNavigation.Avatar,
                            Address = service.IdTutorNavigation.IdAccountNavigation.Address,
                            Phone = service.IdTutorNavigation.IdAccountNavigation.Phone
                        },
                        User = new
                        {
                            Name = account.FullName,
                            Email = account.Email,
                            DateOfBirth = account.DateOfBirth,
                            Gender = account.Gender,
                            Avatar = account.Avatar,
                            Address = account.Address,
                            Phone = account.Phone
                        },
                        Service = new
                        {
                            Title = service.Title,
                            Description = service.Description,
                            PricePerHour = service.PricePerHour,
                            LearningMethod = service.LearningMethod,
                            Class = service.IdClassNavigation?.ClassName,
                            Subject = service.IdSubjectNavigation?.SubjectName
                        },
                        Booking = new
                        {
                            Date = selectedDate.Date1,
                            TimeSlot = selectedTimeSlot.TimeSlot1,
                            Duration = newBooking.Duration,
                            Price = newBooking.Price,
                            Status = newBooking.Status
                        }
                    };

                    return new ApiResponse<BookingServiceModel>
                    {
                        Success = true,
                        Message = "Đặt dịch vụ thành công và tài khoản của gia sư đã bị trừ 50000.",
                        Data = data
                    };
                }
            }

            return new ApiResponse<BookingServiceModel>
            {
                Success = false,
                Message = "Không tìm thấy khung giờ nào phù hợp với dịch vụ này!",
            };
        }

        public async Task<ApiResponse<List<object>>> GetAllServices()
        {
            var services = await _context.Services
                .Include(s => s.IdClassNavigation)
                .Include(s => s.IdSubjectNavigation)
                .Include(s => s.IdTutorNavigation)
                    .ThenInclude(t => t.IdAccountNavigation)
                .Include(s => s.Dates)
                    .ThenInclude(d => d.TimeSlots)
                .ToListAsync();

            if (!services.Any())
            {
                return new ApiResponse<List<object>>
                {
                    Success = false,
                    Message = "Không có dịch vụ nào được tìm thấy",
                };
            }

            var serviceModels = services.Select(service => new
            {
                Id = service.Id, // Bao gồm Id của dịch vụ
                PricePerHour = service.PricePerHour,
                tittle = service.Title,
                Description = service.Description,
                LearningMethod = service.LearningMethod,
                Class = service.IdClassNavigation?.ClassName,
                subject = service.IdSubjectNavigation?.SubjectName,
                Schedule = service.Dates.Select(date => new
                {
                    Date = date.Date1.HasValue ? date.Date1.Value.ToString("yyyy-MM-dd") : null, // Định dạng chuỗi cho Date
                    TimeSlots = date.TimeSlots.Select(slot => slot.TimeSlot1.HasValue && slot.Status.ToLower() == "chưa đặt" ? slot.TimeSlot1.Value.ToString("HH:mm") : null).ToList() // Định dạng chuỗi cho TimeSlot
                }).ToList()
            }).Cast<object>().ToList();

            return new ApiResponse<List<object>>
            {
                Success = true,
                Message = "Lấy danh sách dịch vụ thành công",
                Data = serviceModels
            };
        }

    }
}