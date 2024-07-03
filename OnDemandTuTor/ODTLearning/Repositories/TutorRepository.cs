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
            // Tìm sinh viên theo IdStudent
            var tutor = await _context.Accounts
                                  .Include(s => s.Requests)
                                  .Include(s=>s.Tutor)
                                  .FirstOrDefaultAsync(s => s.Id == id);

            if (tutor == null)
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
                                              .FirstOrDefaultAsync(lm => lm.SubjectName == model.subject);

            if (subjectModel == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy môn học nào với tên này. Vui lòng chọn lại!"
                };
            }


            // Tạo một đối tượng Schedule mới nếu có thông tin về lịch trình
                var requestOfStudent = new Service 
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = model.tittle,
                    Description = model.Description,
                    PricePerHour = model.PricePerHour,
                    IdClass = model.Class,
                    IdSubject = model.subject,
                  
                };

                // Thêm Request vào context
                //await _context.Requests.AddAsync(requestOfStudent);
                await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Tạo yêu cầu thành công",
            };
        }
        //public async Task<List<TutorListModel>> SearchTutorList(SearchTutorModel model)
        //{
        //    //list all
        //    var accountQuerry = _context.Accounts.Where(x => x.Roles == "Tutor");

        //    //list search by name
        //    if (!string.IsNullOrEmpty(model.Name))
        //    {
        //        accountQuerry = accountQuerry.Where(x => x.FullName.Contains(model.Name));
        //        if (!accountQuerry.Any())
        //        {
        //            return null;
        //        }
        //    }

        //    //list search by field
        //    if (!string.IsNullOrEmpty(model.Field))
        //    {
        //        //lay field can search
        //        var field = await _context.Subjects.FirstOrDefaultAsync(x => x.SubjectName == model.Field);
        //        if (field == null)
        //        {
        //            return null;
        //        }
        //        accountQuerry = _context.TutorSubjects.Where(x => x.IdSubject == field.Id).Join(_context.Tutors, tf => tf.IdTutor, t => t.Id, (tf, t) => t).Join(accountQuerry, t => t.IdAccount, aq => aq.Id, (t, aq) => aq);
        //    }

        //    if (!accountQuerry.Any())
        //    {
        //        return null;
        //    }
        //    //lay id cua cac account can search
        //    var idAccountQuerry = accountQuerry.Select(x => x.Id).ToList();

        //    var list = new List<TutorListModel>();

        //    foreach (var id in idAccountQuerry)
        //    {
        //        //lay list cac ten field cua tung account
        //        var fields = _context.Tutors.Where(x => x.IdAccount == id).Join(_context.TutorSubjects.Join(_context.Subjects, tf => tf.IdSubject, f => f.Id, (tf, f) => new
        //        {
        //            AccountId = tf.IdTutor,
        //            Field = f.SubjectName
        //        }), t => t.Id, af => af.AccountId, (t, af) => af.Field).ToList();

        //        //dua vao model
        //        var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id);

        //        var k = new TutorListModel
        //        {
        //            FirstName = account.FullName,
        //            Gmail = account.Email,
        //            Birthdate = account.DateOfBirth,
        //            Gender = account.Gender,
        //            Field = fields
        //        };

        //        list.Add(k);
        //    }
        //    return list;
        //}

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
                                                       TotalSession = r.TotalSession,
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
                TotalSession = rl.IdRequestNavigation.TotalSession
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

