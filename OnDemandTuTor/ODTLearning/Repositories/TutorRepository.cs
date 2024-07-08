using Firebase.Auth;
using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using ODTLearning.Helpers;
using ODTLearning.Models;
using System.Globalization;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ODTLearning.Repositories
{
    public class TutorRepository : ITutorRepository
    {
        private readonly DbminiCapstoneContext _context;

        public TutorRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        MyLibrary myLib = new MyLibrary();

        public async Task<ApiResponse<object>> GetTutorProfile(string id)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id && x.Roles.ToLower() == "gia sư");

            if (account == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư"
                };
            }

            var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.IdAccount == id);

            //lay list field cua account
            var fields = _context.Tutors.Where(x => x.IdAccount == id).Join(_context.TutorSubjects.Join(_context.Subjects, tf => tf.IdSubject, f => f.Id, (tf, f) => new
            {
                AccountId = tf.IdTutor,
                Field = f.SubjectName
            }), t => t.Id, af => af.AccountId, (t, af) => af.Field).ToList();

            var subjects = "";

            foreach (var x in fields)
            {
                subjects += x + ";";
            }

            subjects = subjects.Substring(0, subjects.Length - 1);

            //lay list Qualification cua account
            var qualificationsList = _context.Tutors.Where(x => x.IdAccount == id).Join(_context.EducationalQualifications, t => t.Id, eq => eq.IdTutor, (t, eq) => new
            {
                Id = eq.Id,
                Name = eq.QualificationName,
                Img = eq.Img,
                Type = eq.Type
            }).ToList();

            var idQualifications = "";
            var nameQualifications = "";
            var imgQualifications = "";
            var typeQualifications = "";

            foreach (var x in qualificationsList)
            {
                idQualifications += x.Id + ";";
                nameQualifications += x.Name + ";";
                imgQualifications += x.Img + ";";
                typeQualifications += x.Type + ";";
            }

            idQualifications = myLib.DeleteLastIndexString(idQualifications);
            nameQualifications = myLib.DeleteLastIndexString(nameQualifications);
            imgQualifications = myLib.DeleteLastIndexString(imgQualifications);
            typeQualifications = myLib.DeleteLastIndexString(typeQualifications);

            var qualifications = new
            {
                Id = idQualifications,
                Name = nameQualifications,
                Img = imgQualifications,
                Type = typeQualifications
            };

            //dua vao model
            var data = new
            {
                SpeacializedSkill = tutor.SpecializedSkills,
                Experience = tutor.Experience,
                Introduction = tutor.Introduction,
                Subjects = subjects,
                Qualifications = qualifications,
            };

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Lấy thông tin gia sư thành công",
                Data = data
            };
            
        }

        public async Task<ApiResponse<bool>> UpdateTutorProfile(string id, TutorProfileToUpdate model)
        {
            var tutor = await _context.Tutors
                .Include(t => t.IdAccountNavigation)
                .Include(t => t.TutorSubjects)
                .ThenInclude(tf => tf.IdSubjectNavigation)
                .Include(t => t.EducationalQualifications)
                .FirstOrDefaultAsync(x => x.IdAccount == id && x.IdAccountNavigation.Roles.ToLower() == "gia sư");

            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư"
                };
            }

            tutor.SpecializedSkills = model.SpecializedSkill;
            tutor.Experience = model.Experience;
            tutor.Introduction = model.Introduction;

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Cập nhật thông tin gia sư thành công"
            };
        }

        public async Task<ApiResponse<bool>> AddSubject(string id, string subjectName)
        {
            var tutor = await _context.Tutors.SingleOrDefaultAsync(x => x.IdAccount == id && x.IdAccountNavigation.Roles.ToLower() == "gia sư");

            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư"
                };
            }

            var subject = await _context.Subjects.SingleOrDefaultAsync(x => x.SubjectName == subjectName);

            if (subject == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy môn học"
                };
            }

            var tutorSubject = await _context.TutorSubjects.FirstOrDefaultAsync(x => x.IdTutor == tutor.Id && x.IdSubject == subject.Id);

            if (tutorSubject == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Gia sư đã có môn học này trong chương trình"
                };
            }

            tutorSubject = new TutorSubject
            {
                Id = Guid.NewGuid().ToString(),
                IdTutor = tutor.Id,
                IdSubject = subject.Id,
            };

            await _context.TutorSubjects.AddAsync(tutorSubject);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Thêm môn học thành công"
            };
        }

        public async Task<ApiResponse<bool>> AddQualification(string id, AddQualificationModel model)
        {
            var tutor = await _context.Tutors.SingleOrDefaultAsync(x => x.IdAccount == id && x.IdAccountNavigation.Roles.ToLower() == "gia sư");

            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư"
                };
            }

            var qualification = await _context.EducationalQualifications.FirstOrDefaultAsync(x => x.QualificationName == model.Name && x.Type == model.Type);

            if (qualification != null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"{model.Type} đã tồn tại"
                };
            }

            qualification = new EducationalQualification
            {
                Id = Guid.NewGuid().ToString(),
                QualificationName = model.Name,
                Img = model.Img,
                Type = model.Type,
                IdTutor = tutor.Id                
            };

            await _context.EducationalQualifications.AddAsync(qualification);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = $"Thêm {model.Type} thành công"
            };
        }

        public async Task<ApiResponse<bool>> DeleteSubject(string id, string subjectName)
        {
            var tutor = await _context.Tutors.SingleOrDefaultAsync(x => x.IdAccount == id && x.IdAccountNavigation.Roles.ToLower() == "gia sư");

            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư"
                };
            }

            var tutorSubject = await _context.TutorSubjects.Include(x => x.IdSubjectNavigation).FirstOrDefaultAsync(x => x.IdSubjectNavigation.SubjectName == subjectName);

            if (tutorSubject == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Gia sư không dạy môn học này"
                };
            }

            _context.TutorSubjects.Remove(tutorSubject);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Xóa môn học thành công"
            };
        }

        public async Task<ApiResponse<bool>> DeleteQualification(string id, string idQualification)
        {
            var tutor = await _context.Tutors.SingleOrDefaultAsync(x => x.IdAccount == id && x.IdAccountNavigation.Roles.ToLower() == "gia sư");

            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư"
                };
            }

            var qualification = await _context.EducationalQualifications.FirstOrDefaultAsync(x => x.Id == idQualification);

            if (qualification == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Chứng chỉ/Bằng cấp không tồn tại"
                };
            }

            _context.EducationalQualifications.Remove(qualification);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = $"Xóa {qualification.Type} thành công"
            };
        }

        public async Task<ApiResponse<bool>> CreateServiceLearning(string id, ServiceLearningModel model)
        {
            // Tìm tài khoản theo IdAccount và vai trò "gia sư"
            var account = await _context.Accounts
                                   .Include(a => a.Tutor)
                                   .FirstOrDefaultAsync(a => a.Id == id && a.Roles.ToLower() == "gia sư");

            if (account == null || account.Tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản nào với ID này hoặc bạn chưa đăng ký làm gia sư!",
                };
            }

            // Kiểm tra số dư tài khoản
            const float costPerService = 50000;
            int serviceCount = await _context.Services.CountAsync(s => s.IdTutor == account.Tutor.Id);
            float remainingBalance = account.AccountBalance ?? 0;

            if (remainingBalance < costPerService)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Số dư tài khoản không đủ. Bạn cần ít nhất {costPerService} để tạo dịch vụ này.",
                };
            }

            int maxServicesThatCanBeCreated = (int)(remainingBalance / costPerService);
            int remainingServicesThatCanBeCreated = maxServicesThatCanBeCreated - serviceCount;

            if (remainingServicesThatCanBeCreated <= 0)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Số dư tài khoản không đủ để tạo thêm dịch vụ. Bạn cần nạp thêm!",
                };
            }

            // Tìm lớp học theo tên
            var classEntity = await _context.Classes.FirstOrDefaultAsync(cl => cl.ClassName == model.Class);

            if (classEntity == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp nào với tên này. Vui lòng chọn lớp 10, 11, 12",
                };
            }

            // Tìm môn học theo tên
            var subjectEntity = await _context.Subjects.FirstOrDefaultAsync(sub => sub.SubjectName == model.subject);

            if (subjectEntity == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy môn học nào với tên này. Vui lòng chọn lại!",
                };
            }

            // Tạo một đối tượng Service mới
            var serviceOfTutor = new Service
            {
                Id = Guid.NewGuid().ToString(),
                Title = model.Title,
                Description = model.Description,
                LearningMethod = model.LearningMethod,
                PricePerHour = model.PricePerHour,
                IdClass = classEntity.Id,
                IdSubject = subjectEntity.Id,
                IdTutor = account.Tutor.Id // Sử dụng Id của Tutor từ account
            };

            // Thêm Service vào context
            await _context.Services.AddAsync(serviceOfTutor);
            await _context.SaveChangesAsync();

            // Thêm Date và TimeSlot vào context
            foreach (var dateModel in model.Schedule)
            {
                var dateEntity = new ODTLearning.Entities.Date
                {
                    Id = Guid.NewGuid().ToString(),
                    Date1 = DateOnly.Parse(dateModel.Date),
                    IdService = serviceOfTutor.Id
                };

                await _context.Dates.AddAsync(dateEntity);
                await _context.SaveChangesAsync();

                foreach (var timeSlot in dateModel.TimeSlots)
                {
                    var timeSlotEntity = new TimeSlot
                    {
                        Id = Guid.NewGuid().ToString(),
                        TimeSlot1 = TimeOnly.Parse(timeSlot),
                        Status = "Chưa đặt",
                        IdDate = dateEntity.Id
                    };

                    await _context.TimeSlots.AddAsync(timeSlotEntity);
                }
            }

            // Cập nhật lại số dư tài khoản sau khi tạo dịch vụ
            await _context.SaveChangesAsync();

            // Tính lại số dịch vụ có thể tạo sau khi trừ số dư tài khoản
            float newRemainingBalance = account.AccountBalance ?? 0;
            int newMaxServicesThatCanBeCreated = (int)(newRemainingBalance / costPerService);
            int newRemainingServicesThatCanBeCreated = newMaxServicesThatCanBeCreated - serviceCount;

            return new ApiResponse<bool>
            {
                Success = true,
                Message = $"Tạo dịch vụ thành công. Bạn có thể tạo thêm {newRemainingServicesThatCanBeCreated} dịch vụ nữa.",
            };
        }



        public async Task<ApiResponse<List<object>>> GetAllServicesByAccountId(string id)
        {
            // Tìm tài khoản theo IdAccount và vai trò "gia sư"
            var account = await _context.Accounts
                                  .Include(a => a.Tutor)
                                  .FirstOrDefaultAsync(a => a.Id == id && a.Roles.ToLower() == "gia sư");

            if (account == null || account.Tutor == null)
            {
                return new ApiResponse<List<object>>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản nào với ID này hoặc bạn chưa đăng ký làm gia sư!",
                };
            }

            // Tìm tất cả các dịch vụ của gia sư
            var services = await _context.Services
                                 .Where(s => s.IdTutor == account.Tutor.Id)
                                 .Include(s => s.Dates)
                                 .ThenInclude(d => d.TimeSlots)
                                 .ToListAsync();

            // Chuyển đổi các dịch vụ sang mô hình ServiceLearningModel và bao gồm ID của dịch vụ
            var serviceModels = services.Select(s => new
            {
                Id = s.Id, // Truyền ID của service
                ServiceDetails = new ServiceLearningModel
                {
                    Title = s.Title,
                    Description = s.Description,
                    PricePerHour = s.PricePerHour,
                    Class = _context.Classes.FirstOrDefault(cl => cl.Id == s.IdClass)?.ClassName,
                    subject = _context.Subjects.FirstOrDefault(sub => sub.Id == s.IdSubject)?.SubjectName,
                    LearningMethod = s.LearningMethod,
                    Schedule = s.Dates.Select(d => new ServiceDateModel
                    {
                        Date = d.Date1.HasValue ? d.Date1.Value.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd") : null,
                        TimeSlots = d.TimeSlots
                             .Where(ts => ts.TimeSlot1.HasValue)
                             .Select(ts => ts.TimeSlot1.Value.ToString("HH:mm"))
                             .ToList()
                    }).ToList()
                }
            }).ToList();

            // Kiểm tra các giá trị trong serviceModels
            foreach (var serviceModel in serviceModels)
            {
                Console.WriteLine($"Service ID: {serviceModel.Id}");
                Console.WriteLine($"Title: {serviceModel.ServiceDetails.Title}");
                Console.WriteLine($"LearningMethod: {serviceModel.ServiceDetails.LearningMethod}");
                Console.WriteLine($"Schedule: {serviceModel.ServiceDetails.Schedule}");
            }

            return new ApiResponse<List<object>>
            {
                Success = true,
                Message = "Lấy danh sách dịch vụ thành công",
                Data = serviceModels.Cast<object>().ToList()
            };
        }

        public async Task<ApiResponse<bool>> DeleteServiceById(string serviceId)
        {
            var service = await _context.Services
                                         .Include(s => s.Dates)
                                         .ThenInclude(d => d.TimeSlots)
                                         .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy dịch vụ với ID này."
                };
            }

            var ongoingBooking = service.Dates
                                        .SelectMany(d => d.TimeSlots)
                                        .Any(ts => ts.Bookings.Any(b => b.Status == "Đang diễn ra" || b.Status == "Hoàn thành"));

            if (ongoingBooking)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không thể xóa dịch vụ vì có booking đang diễn ra."
                };
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Xóa dịch vụ thành công."
            };
        }

        public async Task<ApiResponse<ServiceLearningModel>> UpdateServiceById(string serviceId, ServiceLearningModel model)
        {
            // Tìm dịch vụ theo serviceId
            var service = await _context.Services
                                        .Include(s => s.IdTutorNavigation)
                                        .ThenInclude(t => t.IdAccountNavigation)
                                        .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return new ApiResponse<ServiceLearningModel>
                {
                    Success = false,
                    Message = "Không tìm thấy dịch vụ với ID này."
                };
            }

            var account = service.IdTutorNavigation?.IdAccountNavigation;
            if (account == null)
            {
                return new ApiResponse<ServiceLearningModel>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản liên kết với dịch vụ này."
                };
            }

            // Cập nhật thông tin dịch vụ
            service.Title = model.Title;
            service.Description = model.Description;
            service.PricePerHour = model.PricePerHour;
            service.IdClass = (await _context.Classes.FirstOrDefaultAsync(c => c.ClassName == model.Class))?.Id;
            service.IdSubject = (await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectName == model.subject))?.Id;

            _context.Services.Update(service);
            await _context.SaveChangesAsync();

            // Kiểm tra số dư tài khoản sau khi cập nhật dịch vụ
            const float costPerService = 50000;
            int serviceCount = await _context.Services.CountAsync(s => s.IdTutor == service.IdTutor);
            float remainingBalance = account.AccountBalance ?? 0;

            if (remainingBalance < costPerService)
            {
                return new ApiResponse<ServiceLearningModel>
                {
                    Success = true,
                    Message = "Cập nhật dịch vụ thành công. Tuy nhiên, số dư tài khoản không đủ để tạo thêm dịch vụ mới.",
                    Data = new ServiceLearningModel
                    {
                        PricePerHour = service.PricePerHour,
                        Title = service.Title,
                        subject = (await _context.Subjects.FirstOrDefaultAsync(s => s.Id == service.IdSubject))?.SubjectName,
                        Class = (await _context.Classes.FirstOrDefaultAsync(c => c.Id == service.IdClass))?.ClassName,
                        Description = service.Description,
                        LearningMethod = model.LearningMethod
                    }
                };
            }

            int maxServicesThatCanBeCreated = (int)(remainingBalance / costPerService);
            int remainingServicesThatCanBeCreated = maxServicesThatCanBeCreated - serviceCount;

            return new ApiResponse<ServiceLearningModel>
            {
                Success = true,
                Message = $"Cập nhật dịch vụ thành công. Bạn có thể tạo thêm {remainingServicesThatCanBeCreated} dịch vụ nữa.",
                Data = new ServiceLearningModel
                {
                    PricePerHour = service.PricePerHour,
                    Title = service.Title,
                    subject = (await _context.Subjects.FirstOrDefaultAsync(s => s.Id == service.IdSubject))?.SubjectName,
                    Class = (await _context.Classes.FirstOrDefaultAsync(c => c.Id == service.IdClass))?.ClassName,
                    Description = service.Description,
                    LearningMethod = model.LearningMethod
                }
            };
        }



        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetApprovedRequests()
        {
            var pendingRequests = await _context.Requests
                                                   .Where(r => r.Status == "Đã duyệt")
                                                   .Select(r => new ViewRequestOfStudent
                                                   {
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
                                                       FullName = r.IdAccountNavigation.FullName // Include Account Full Name
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

        public async Task<ApiResponse<List<RequestLearningResponse>>> GetClassProcess(string accountId)
        {
            // Check roles
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId && a.Roles.ToLower() == "gia sư");
            if (account == null)
            {
                return new ApiResponse<List<RequestLearningResponse>>
                {
                    Success = false,
                    Message = "Không tìm thấy nào với ID tài khoản này hoặt bạn phải là gia sư ",
                };
            }

            // Tìm gia sư theo IdAccount
            var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.IdAccount == accountId);
            if (tutor == null)
            {
                return new ApiResponse<List<RequestLearningResponse>>
                {
                    Success = false,
                    Message = "Bạn chưa đăng ký trở thành gia sư hoặt đơn đăng ký của bạn đang được duyệt",
                };
            }

            // Lấy tất cả các RequestLearning có IdTutor là tutor.Id
            var requestLearnings = await _context.RequestLearnings
                .Include(rl => rl.IdRequestNavigation)
                    .ThenInclude(r => r.IdSubjectNavigation)
                .Include(rl => rl.IdRequestNavigation)
                    .ThenInclude(r => r.IdClassNavigation)
                .Where(rl => rl.IdTutor == tutor.Id && rl.IdRequestNavigation.Status == "Đang diễn ra")
                .ToListAsync();

            if (requestLearnings == null || !requestLearnings.Any())
            {
                return new ApiResponse<List<RequestLearningResponse>>
                {
                    Success = true,
                    Message = "Không tìm thấy yêu cầu nào với trạng thái 'Đang diễn ra' mà gia sư đã tham gia",
                };
            }

            // Chuyển đổi danh sách requestLearnings thành danh sách RequestLearningModel
            var requestLearningModels = requestLearnings.Select(rl => new RequestLearningResponse
            {
                IdRequest = rl.IdRequestNavigation.Id,
                Title = rl.IdRequestNavigation.Title,
                Price = rl.IdRequestNavigation.Price,
                Description = rl.IdRequestNavigation.Description,
                Subject = rl.IdRequestNavigation.IdSubjectNavigation?.SubjectName,
                LearningMethod = rl.IdRequestNavigation.LearningMethod,
                Class = rl.IdRequestNavigation.IdClassNavigation?.ClassName,
                TimeStart = rl.IdRequestNavigation.TimeStart.HasValue ? rl.IdRequestNavigation.TimeStart.Value.ToString("HH:mm") : null,
                TimeEnd = rl.IdRequestNavigation.TimeEnd.HasValue ? rl.IdRequestNavigation.TimeEnd.Value.ToString("HH:mm") : null,
                TimeTable = rl.IdRequestNavigation.TimeTable,
                Status = rl.IdRequestNavigation.Status,
                TotalSessions = rl.IdRequestNavigation.TotalSession
            }).ToList();

            return new ApiResponse<List<RequestLearningResponse>>
            {
                Success = true,
                Message = "Danh sách lớp đang diễn ra được truy xuất thành công",
                Data = requestLearningModels
            };
        }
      
        public async Task<ApiResponse<object>> GetReview(string id)
        {
            var tutor = await _context.Tutors.SingleOrDefaultAsync(x => x.IdAccount == id && x.IdAccountNavigation.Roles.ToLower() == "gia sư");

            if (tutor == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư"
                };
            }

            var reviews = await _context.Reviews.Include(x => x.IdAccountNavigation)
                                               .Include(x => x.IdTutorNavigation).ThenInclude(x => x.IdAccountNavigation)
                                               .Where(x => x.IdTutorNavigation.IdAccount == id).ToListAsync();

            if (!reviews.Any())
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Gia sư chưa có đánh giá nào",
                };
            }

            var list = new List<object>();

            foreach (var review in reviews)
            {
                var data = new
                {
                    IdReview = review.Id,

                    User = new
                    {
                        Id = review.IdAccountNavigation.Id,
                        FullName = review.IdAccountNavigation.FullName,
                        Email = review.IdAccountNavigation.Email,
                        Date_of_birth = review.IdAccountNavigation.DateOfBirth,
                        Gender = review.IdAccountNavigation.Gender,
                        Avatar = review.IdAccountNavigation.Avatar,
                        Address = review.IdAccountNavigation.Address,
                        Phone = review.IdAccountNavigation.Phone
                    },

                    Rating = review.Rating,
                    Feedback = review.Feedback,
                };

                list.Add(data);
            }

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = list
            };
        }
    }
}

