using Firebase.Auth;
using Microsoft.EntityFrameworkCore;
using ODTLearning.DAL.Entities;
using ODTLearning.BLL.Helpers;
using ODTLearning.Models;
using System.Globalization;
using System.Net;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ODTLearning.BLL.Repositories
{
    public class TutorRepository 
    {
        private readonly DbminiCapstoneContext _context;

        public TutorRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<TutorResponse>> SignUpOfTutor(string IdAccount, SignUpModelOfTutor model)
        {
            // Tìm kiếm account trong DB bằng id
            var existingUser = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == IdAccount);
            if (existingUser == null)
            {
                // Trường hợp không tìm thấy tài khoản
                return new ApiResponse<TutorResponse>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản",
                };
            }

            // Kiểm tra xem đã có gia sư nào với ID_Account này chưa
            var existingTutor = await _context.Tutors.FirstOrDefaultAsync(t => t.IdAccount == IdAccount);
            if (existingTutor != null)
            {
                return new ApiResponse<TutorResponse>
                {
                    Success = false,
                    Message = "Tài khoản đã đăng ký gia sư",
                };
            }

            // Tạo mới đối tượng tutor
            var tutor = new Tutor
            {
                Id = Guid.NewGuid().ToString(),
                SpecializedSkills = model.specializedSkills,
                Experience = model.experience,
                Status = "Đang duyệt",
                IdAccount = existingUser.Id,
                Introduction = model.introduction,
            };

            // Tạo mới đối tượng educationalqualification
            var educationalQualification = new EducationalQualification
            {
                Id = Guid.NewGuid().ToString(),
                IdTutor = tutor.Id,
                QualificationName = model.qualifiCationName,
                Type = model.type,
                Img = model.imageQualification
            };

            // Tìm môn học theo tên
            var subjectModel = await _context.Subjects.FirstOrDefaultAsync(lm => lm.SubjectName == model.subject);
            if (subjectModel == null)
            {
                return new ApiResponse<TutorResponse>
                {
                    Success = false,
                    Message = "Không tìm thấy môn học nào với tên này. Vui lòng thử lại!",
                };
            }

            // Tạo mới đối tượng TutorSubject
            var tutorSubject = new TutorSubject
            {
                Id = Guid.NewGuid().ToString(),
                IdSubject = subjectModel.Id,
                IdTutor = tutor.Id,
            };

            // Thêm các đối tượng vào DB
            await _context.Tutors.AddAsync(tutor);
            await _context.EducationalQualifications.AddAsync(educationalQualification);
            await _context.TutorSubjects.AddAsync(tutorSubject);
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = "Đăng ký gia sư thành công. Vui lòng chờ duyệt.",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = IdAccount
            };
            Console.WriteLine("Creating notification...");
            Console.WriteLine($"Notification Description: {nofi.Description}");

            Console.WriteLine("Before saving: " + nofi.Description);
            await _context.Notifications.AddAsync(nofi);
            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("After saving: " + nofi.Description);
                return new ApiResponse<TutorResponse>
                {
                    Success = true,
                    Message = "Đăng ký gia sư thành công. Bạn vui lòng chờ duyệt",
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu có xảy ra
                Console.WriteLine($"Error while saving changes: {ex.Message}");
                return new ApiResponse<TutorResponse>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lưu dữ liệu",
                    Data = null
                };
            }
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
                tutor.Reason,
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
        public async Task<ApiResponse<List<ListTutorToConfirm>>> GetListTutorsToConfirm()
        {
            try
            {
                var tutors = await _context.Tutors
                    .Include(t => t.IdAccountNavigation)
                    .Include(t => t.TutorSubjects)
                        .ThenInclude(ts => ts.IdSubjectNavigation)
                    .Include(t => t.EducationalQualifications)
                    .Where(t => t.Status == "Đang duyệt")
                    .Select(t => new ListTutorToConfirm
                    {
                        Id = t.IdAccount, // Sử dụng Id của Tutor
                        specializedSkills = t.SpecializedSkills,
                        introduction = t.Introduction,
                        date_of_birth = t.IdAccountNavigation.DateOfBirth,

                        fullName = t.IdAccountNavigation.FullName,
                        gender = t.IdAccountNavigation.Gender,
                        experience = t.Experience,
                        subject = t.TutorSubjects.FirstOrDefault().IdSubjectNavigation.SubjectName, // Lấy Subject từ TutorSubjects
                        qualifiCationName = t.EducationalQualifications.FirstOrDefault().QualificationName, // Lấy QualificationName từ 
                        type = t.EducationalQualifications.FirstOrDefault().Type, // Lấy Type từ EducationalQualifications
                        imageQualification = t.EducationalQualifications.FirstOrDefault().Img // Lấy ImageQualification từ EducationalQualifications
                    })
                    .ToListAsync();

                if (!tutors.Any())
                {
                    return new ApiResponse<List<ListTutorToConfirm>>
                    {
                        Success = true,
                        Message = "Không có gia sư nào cần xác nhận",
                        Data = []
                    };
                }

                return new ApiResponse<List<ListTutorToConfirm>>
                {
                    Success = true,
                    Message = "Lấy danh sách gia sư thành công",
                    Data = tutors
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListTutorsToConfirm: {ex.Message}");

                return new ApiResponse<List<ListTutorToConfirm>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách gia sư",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<bool>> ReSignUpOftutor(string id, SignUpModelOfTutor model)
        {
            // Tìm kiếm account trong DB bằng id
            var tutor = await _context.Tutors.Include(t => t.IdAccountNavigation)
                                             .Include(t => t.TutorSubjects).ThenInclude(ts => ts.IdSubjectNavigation)
                                             .Include(t => t.EducationalQualifications)
                                             .FirstOrDefaultAsync(t => t.IdAccount == id);

            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng"
                };
            }

            // Tạo mới đối tượng educationalqualification
            var educationalQualification = new EducationalQualification
            {
                Id = Guid.NewGuid().ToString(),
                IdTutor = tutor.Id,
                QualificationName = model.qualifiCationName,
                Type = model.type,
                Img = model.imageQualification
            };

            // Tìm môn học theo tên
            var subjectModel = await _context.Subjects.FirstOrDefaultAsync(lm => lm.SubjectName == model.subject);

            if (subjectModel == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy môn học nào với tên này. Vui lòng thử lại!",
                };
            }

            // Tạo mới đối tượng TutorSubject
            var tutorSubject = new TutorSubject
            {
                Id = Guid.NewGuid().ToString(),
                IdSubject = subjectModel.Id,
                IdTutor = tutor.Id,
            };

            var oldEducationalQualification = await _context.EducationalQualifications.FirstOrDefaultAsync(x => x.IdTutor == tutor.Id);
            var oldTutorSubject = await _context.TutorSubjects.FirstOrDefaultAsync(x => x.IdTutor == tutor.Id);

            tutor.Introduction = model.introduction;
            tutor.Experience = model.experience;
            tutor.SpecializedSkills = model.specializedSkills;
            tutor.Status = "Đang duyệt";
            tutor.Reason = null;

            _context.TutorSubjects.Remove(oldTutorSubject);
            _context.EducationalQualifications.Remove(oldEducationalQualification);
            await _context.EducationalQualifications.AddAsync(educationalQualification);
            await _context.TutorSubjects.AddAsync(tutorSubject);

            await _context.SaveChangesAsync();
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = "Bạn đã đăng ký gia sư thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = id,
            };

            await _context.Notifications.AddAsync(nofi);
            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Gửi đơn thành công. Bạn vui lòng chờ duyệt",
            };
        }
        public async Task<ApiResponse<bool>> ApproveProfileTutor(string id)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.IdAccount == id);

                if (tutor == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = true,
                        Message = "Không tìm thấy gia sư với ID tài khoản này",
                        Data = false
                    };
                }

                tutor.Status = "Đã duyệt";
                _context.Tutors.Update(tutor);

                var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == tutor.IdAccount);
                account.Roles = "gia sư";
                _context.Accounts.Update(account);

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Duyệt gia sư thành công",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in ApproveProfileTutor: {ex.Message}");

                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình duyệt gia sư",
                    Data = false
                };
            }
        }

        public async Task<ApiResponse<bool>> RejectProfileTutor(string id, ReasonReject model)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.IdAccount == id);

                if (tutor == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = true,
                        Message = "Không tìm thấy gia sư với tài khoản này",
                        Data = false
                    };
                }

                tutor.Status = "Từ chối";
                tutor.Reason = model.reason;
                _context.Tutors.Update(tutor);

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Từ chối yêu cầu của gia sư thành công",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in RejectProfileTutor: {ex.Message}");

                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình từ chối gia sư",
                    Data = false
                };
            }
        }
        public async Task<ApiResponse<object>> DeleteSignUpTutor(string id)
        {
            // Tìm gia sư trong cơ sở dữ liệu bằng IdAccount
            var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.IdAccount == id);

            if (tutor == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy đăng ký gia sư"
                };
            }
            // Kiểm tra trạng thái của gia sư
            if (tutor.Status != "Đang duyệt" && tutor.Status != "Từ chối")
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Chỉ có thể xóa đăng ký gia sư ở trạng thái 'Đang duyệt' hoặc 'Từ chối'"
                };
            }

            // Tìm các môn học của gia sư
            var tutorSubjects = await _context.TutorSubjects.Where(ts => ts.IdTutor == tutor.Id).ToListAsync();
            // Tìm các chứng chỉ học vấn của gia sư
            var qualifications = await _context.EducationalQualifications.Where(eq => eq.IdTutor == tutor.Id).ToListAsync();

            // Xóa các đối tượng liên quan
            _context.TutorSubjects.RemoveRange(tutorSubjects);
            _context.EducationalQualifications.RemoveRange(qualifications);
            _context.Tutors.Remove(tutor);

            try
            {
                await _context.SaveChangesAsync();
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Xóa đăng ký gia sư thành công"
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu có xảy ra
                Console.WriteLine($"Error while deleting tutor: {ex.Message}");
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình xóa dữ liệu",
                    Data = null
                };
            }
        }
        




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

            // Lấy list field của account
            var fields = _context.Tutors.Where(x => x.IdAccount == id).Join(_context.TutorSubjects.Join(_context.Subjects, tf => tf.IdSubject, f => f.Id, (tf, f) => new
            {
                AccountId = tf.IdTutor,
                Field = f.SubjectName
            }), t => t.Id, af => af.AccountId, (t, af) => af.Field).ToList();

            var subjects = string.Join("; ", fields);

            // Lấy list Qualification của account
            var qualificationsList = _context.Tutors.Where(x => x.IdAccount == id).Join(_context.EducationalQualifications, t => t.Id, eq => eq.IdTutor, (t, eq) => new
            {
                Id = eq.Id,
                Name = eq.QualificationName,
                Img = eq.Img,
                Type = eq.Type
            }).ToList();

            var qualifications = qualificationsList.Select(x => new
            {
                Id = x.Id,
                QualificationName = x.Name,
                Img = x.Img,
                Type = x.Type
            }).ToList();

            // Đưa vào model
            var data = new
            {
                Avatar = account.Avatar,
                SpecializedSkills = tutor.SpecializedSkills,
                Experience = tutor.Experience,
                Introduction = tutor.Introduction,
                Subjects = subjects,
                Qualifications = qualifications
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
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = "Bạn đã cập nhật hồ sơ thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = id,
            };

            await _context.Notifications.AddAsync(nofi);
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

            if (tutorSubject != null)
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
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã thêm môn học '{subjectName}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = id,
            };

            await _context.Notifications.AddAsync(nofi);
            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Thêm môn học thành công"
            };
        }

        public async Task<ApiResponse<bool>> AddQualification(string id, AddQualificationModel model)
        {
            var tutor = await _context.Tutors.SingleOrDefaultAsync(x => x.IdAccount == id && x.IdAccountNavigation.Roles.ToLower() == "gia sư");
            Console.WriteLine(" : " + model.Name);
            Console.WriteLine("imgQuatification : "+model.Img);
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
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã thêm '{model.Type}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = id,
            };

            await _context.Notifications.AddAsync(nofi);
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
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã xóa môn '{subjectName}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = id,
            };

            await _context.Notifications.AddAsync(nofi);
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
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã xóa '{qualification.QualificationName}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = id,
            };

            await _context.Notifications.AddAsync(nofi);

            return new ApiResponse<bool>
            {
                Success = true,
                Message = $"Xóa {qualification.Type} thành công"
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

        public async Task<ApiResponse<object>> GetRegisterTutor(string id)
        {
            var tutor = await _context.Tutors.Include(t => t.IdAccountNavigation)
                                             .Include(t => t.TutorSubjects).ThenInclude(ts => ts.IdSubjectNavigation)
                                             .Include(t => t.EducationalQualifications)
                                             .FirstOrDefaultAsync(t => t.IdAccount == id);

            if (tutor == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Bạn chưa đăng ký làm gia sư"
                };
            }

            var data = new
            {
                Id = tutor.IdAccount, // Sử dụng Id của Tutor
                specializedSkills = tutor.SpecializedSkills,
                introduction = tutor.Introduction,
                date_of_birth = tutor.IdAccountNavigation.DateOfBirth,
                fullName = tutor.IdAccountNavigation.FullName,
                gender = tutor.IdAccountNavigation.Gender,
                experience = tutor.Experience,
                subject = tutor.TutorSubjects.FirstOrDefault().IdSubjectNavigation.SubjectName, // Lấy Subject từ TutorSubjects
                qualifiCationName = tutor.EducationalQualifications.FirstOrDefault().QualificationName, // Lấy QualificationName từ 
                type = tutor.EducationalQualifications.FirstOrDefault().Type, // Lấy Type từ EducationalQualifications
                imageQualification = tutor.EducationalQualifications.FirstOrDefault().Img, // Lấy ImageQualification từ EducationalQualifications
                Status = tutor.Status,
                Reason = tutor.Reason,
            };

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Lấy đơn đăng ký gia sư thành công",
                Data = data
            };
        }

       
    }
}

