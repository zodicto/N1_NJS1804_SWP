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
            if ( parsedTimeStart.HasValue && parsedTimeEnd.HasValue)
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
                    TotalSession = model.TotalSession,
                    Description = model.Description,
                    Status = "Chưa duyệt",
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
            if (requestToUpdate.Status != "Chưa duyệt")
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
            requestToUpdate.TotalSession = model.TotalSession?? requestToUpdate.TotalSession;
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


        public async Task<ApiResponse<bool>> DeleteRequestLearning(string requestId)
        {
            // Tìm request theo requestId
            var requestToDelete = await _context.Requests
                                                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (requestToDelete == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu nào với ID này!"
                };
            }

            // Kiểm tra trạng thái của yêu cầu
            if (requestToDelete.Status != "chưa duyệt")
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Chỉ có thể xóa các yêu cầu ở trạng thái 'chưa duyệt'!"
                };
            }


            // Xóa request
            _context.Requests.Remove(requestToDelete);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Yêu cầu đã được xóa thành công",
                Data = true
            };
        }



        public async Task<ApiResponse<List<RequestLearningResponse>>> GetPendingRequestsByAccountId(string accountId)
        {
            // Truy vấn danh sách các request có status là "Từ chối" và idAccount là accountId
            var requests = await _context.Requests
                .Include(r => r.IdSubjectNavigation)
                .Include(r => r.IdClassNavigation)
                .Where(r => r.IdAccount == accountId && r.Status == "Chưa duyệt")
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
                TotalSession = r.TotalSession
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
                TotalSession = r.TotalSession
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
                TotalSession = r.TotalSession
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
                    Data = null
                };
            }

            // Lấy danh sách gia sư tham gia yêu cầu
            var tutors = request.RequestLearnings.Select(rl => new TutorListModel
            {
                id = rl.IdTutorNavigation.IdAccount,
                fullName = rl.IdTutorNavigation.IdAccountNavigation.FullName,
                gender = rl.IdTutorNavigation.IdAccountNavigation.Gender,
                specializedskills= rl.IdTutorNavigation.SpecializedSkills,
                experience = rl.IdTutorNavigation.Experience,
                subject  = rl.IdTutorNavigation.TutorSubjects.FirstOrDefault()?.IdSubjectNavigation.SubjectName,
                qualificationname= rl.IdTutorNavigation.EducationalQualifications.FirstOrDefault()?.QualificationName
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
                    Data = null
                };
            }

            var tutor = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == idAccountTutor);

            if (tutor == null || tutor.Roles.ToLower() != "gia sư")
            {
                return new ApiResponse<SelectTutorModel>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư trong hệ thống",
                    Data = null
                };
            }

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
                IdSubject = request.IdSubject,
                IdRequest = idRequest,
                IdAccount = request.IdAccount,
                IdTutor = idAccountTutor                
            };

            //user.AccountBalance = user.AccountBalance - request.Price;
            tutor.AccountBalance = tutor.AccountBalance - 50000;
            request.Status = "Đã thuê";
            await _context.AddAsync(rent);
            await _context.SaveChangesAsync();

            var data = new SelectTutorModel
            {
                Tutor = new
                {
                    Name = tutor.FullName,
                    Email = tutor.Email,
                    DateOfBirth = tutor.DateOfBirth,
                    Gender = tutor.Gender,
                    Avatar = tutor.Avatar,
                    Address = tutor.Address,
                    Phone = tutor.Phone
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

        public async Task<ApiResponse<ComplaintResponse>> CreateComplaint(ComplaintModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.IdUser);

            if (user == null)
            {
                return new ApiResponse<ComplaintResponse>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng",
                };
            }

            var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation).FirstOrDefaultAsync(x => x.Id == model.IdAccountTutor && x.IdAccountNavigation.Roles.ToLower() == "gia sư");

            if (tutor == null)
            {
                return new ApiResponse<ComplaintResponse>
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

            var data = new ComplaintResponse
            {
                User = new
                {
                    Name = user.FullName,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    Gender = user.Gender,
                    Avatar = user.Avatar,
                    Address = user.Address,
                    Phone = user.Phone
                },

                Description = model.Description,

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
            };

            return new ApiResponse<ComplaintResponse>
            {
                Success = true,
                Message = "Thành công",
                Data = data
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
    }
}