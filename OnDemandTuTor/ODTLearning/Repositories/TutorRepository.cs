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

        ImageLibrary imgLib = new ImageLibrary();
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
            float requiredBalance = (serviceCount + 1) * costPerService;

            if (account.AccountBalance < requiredBalance)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Số dư tài khoản không đủ. Bạn cần ít nhất {requiredBalance} để tạo dịch vụ này.",
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
                Title = model.tittle,
                Description = model.Description,
                PricePerHour = model.PricePerHour,
                IdClass = classEntity.Id,
                IdSubject = subjectEntity.Id,
                IdTutor = account.Tutor.Id // Sử dụng Id của Tutor từ account
            };

            // Thêm Service vào context
            await _context.Services.AddAsync(serviceOfTutor);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Tạo dịch vụ thành công",
            };
        }
        public async Task<ApiResponse<bool>> CreateTimeAvailable(string id, TimeAvailableModel time)
        {
            // Find the account by Id and ensure it has the "gia sư" role
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

            // Split the TimeSlot string into individual time slots
            var timeSlots = time.TimeSlot.Split(';');
            Console.WriteLine("TimeSlots: " + string.Join(", ", timeSlots));

            // Create a list to hold the new Available entries
            var availableEntries = new List<Available>();

            foreach (var slot in timeSlots)
            {
                // Find the corresponding TimeSlot entity
                var timeSlotEntity = await _context.TimeSlots
                                                   .FirstOrDefaultAsync(ts => ts.TimeSlot1.ToString() == slot);

                if (timeSlotEntity == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = $"Không tìm thấy khung giờ nào phù hợp với giá trị: {slot}",
                    };
                }

                // Create a new Available entry
                var newAvailable = new Available
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = DateOnly.Parse(time.Date),
                    IdTutor = account.Tutor.Id,
                    IdTimeSlot = timeSlotEntity.Id
                };

                Console.WriteLine("New Available Entry: ");
                Console.WriteLine($"Id: {newAvailable.Id}");
                Console.WriteLine($"Date: {newAvailable.Date}");
                Console.WriteLine($"IdTutor: {newAvailable.IdTutor}");
                Console.WriteLine($"IdTimeSlot: {newAvailable.IdTimeSlot}");

                availableEntries.Add(newAvailable);
            }

            // Print all available entries
            Console.WriteLine("All Available Entries:");
            foreach (var entry in availableEntries)
            {
                Console.WriteLine($"Id: {entry.Id}, Date: {entry.Date}, IdTutor: {entry.IdTutor}, IdTimeSlot: {entry.IdTimeSlot}");
            }

            // Add the new entries to the context and save changes
            await _context.Availables.AddRangeAsync(availableEntries);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Tạo thời gian rảnh thành công",
            };
        }
        public async Task<ApiResponse<List<ServiceLearningModel>>> GetAllServicesByAccountId(string id)
        {
            // Tìm tài khoản theo IdAccount và vai trò "gia sư"
            var account = await _context.Accounts
                                  .Include(a => a.Tutor)
                                  .FirstOrDefaultAsync(a => a.Id == id && a.Roles.ToLower() == "gia sư");

            if (account == null || account.Tutor == null)
            {
                return new ApiResponse<List<ServiceLearningModel>>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản nào với ID này hoặc bạn chưa đăng ký làm gia sư!",
                    Data = null
                };
            }

            // Tìm tất cả các dịch vụ của gia sư
            var services = await _context.Services
                                 .Where(s => s.IdTutor == account.Tutor.Id)
                                 .ToListAsync();

            // Chuyển đổi các dịch vụ sang mô hình ServiceLearningModel
            var serviceModels = services.Select(s => new ServiceLearningModel
            {
                tittle = s.Title,
                Description = s.Description,
                PricePerHour = s.PricePerHour,
                Class = _context.Classes.FirstOrDefault(cl => cl.Id == s.IdClass)?.ClassName,
                subject = _context.Subjects.FirstOrDefault(sub => sub.Id == s.IdSubject)?.SubjectName
            }).ToList();

            return new ApiResponse<List<ServiceLearningModel>>
            {
                Success = true,
                Message = "Lấy danh sách dịch vụ thành công",
                Data = serviceModels
            };
        }
        public async Task<ApiResponse<bool>> DeleteServiceById(string serviceId)
        {
            var service = await _context.Services
                                        .Include(s => s.Bookings)
                                        .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy dịch vụ với ID này."
                };
            }

            var ongoingBooking = service.Bookings.Any(b => b.Status == "Đang diễn ra");
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
                                        .Include(s => s.Bookings)
                                        .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return new ApiResponse<ServiceLearningModel>
                {
                    Success = false,
                    Message = "Không tìm thấy dịch vụ với ID này."
                };
            }

            var ongoingBooking = service.Bookings.Any(b => b.Status == "Đang diễn ra");
            if (ongoingBooking)
            {
                return new ApiResponse<ServiceLearningModel>
                {
                    Success = false,
                    Message = "Không thể cập nhật dịch vụ vì có booking đang diễn ra."
                };
            }

            // Cập nhật thông tin dịch vụ
            service.Title = model.tittle;
            service.Description = model.Description;
            service.PricePerHour = model.PricePerHour;
            service.IdClass = (await _context.Classes.FirstOrDefaultAsync(c => c.ClassName == model.Class))?.Id;
            service.IdSubject = (await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectName == model.subject))?.Id;

            _context.Services.Update(service);
            await _context.SaveChangesAsync();

            // Tạo đối tượng ServiceLearningModel để trả về thông tin cập nhật
            var updatedServiceModel = new ServiceLearningModel
            {
                PricePerHour = service.PricePerHour,
                tittle = service.Title,
                subject = (await _context.Subjects.FirstOrDefaultAsync(s => s.Id == service.IdSubject))?.SubjectName,
                Class = (await _context.Classes.FirstOrDefaultAsync(c => c.Id == service.IdClass))?.ClassName,
                Description = service.Description,
                LearningMethod = model.LearningMethod // Assuming LearningMethod is updated or used somewhere else
            };

            return new ApiResponse<ServiceLearningModel>
            {
                Success = true,
                Message = "Cập nhật dịch vụ thành công.",
                Data = updatedServiceModel
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

            // Kiểm tra số dư tài khoản
            if (account.AccountBalance < 50000)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Bạn cần có 50.000 trong tài khoản để tham gia yêu cầu"
                };
            }

            var tutorId = tutor.Id;

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
            request.Status = "Đang chờ học sinh chấp nhận";

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

        public async Task<ApiResponse<bool>> CompledClass(string requestId)
        {
            // Tìm request theo IdRequest
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId && r.Status == "Đang diễn ra");

            if (request == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu nào với trạng thái 'Đang diễn ra' cho ID này",
                };
            }

            // Thay đổi trạng thái của request thành "Hoàn thành"
            request.Status = "Hoàn thành";

            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Bạn đã hoàn thành lớp học",
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu có xảy ra
                Console.WriteLine($"Error while saving changes: {ex.Message}");
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lưu dữ liệu",
                };
            }
        }


    }
}

