using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using ODTLearning.Helpers;
using ODTLearning.Models;
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
                Name = eq.QualificationName,
                Img = eq.Img,
                Type = eq.Type
            }).ToList();            

            var nameQualifications = "";
            var imgQualifications = "";
            var typeQualifications = "";

            foreach (var x in qualificationsList)
            {
                nameQualifications += x.Name + ";";
                imgQualifications += x.Img + ";";
                typeQualifications += x.Type + ";";
            }

            nameQualifications = nameQualifications.Substring(0, nameQualifications.Length - 1);
            imgQualifications = imgQualifications.Substring(0, imgQualifications.Length - 1);
            typeQualifications = typeQualifications.Substring(0, typeQualifications.Length - 1);

            var qualifications = new
            {
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

                                                       Price = r.Price,
                                                       Description = r.Description,
                                                       Subject = r.IdSubjectNavigation.SubjectName, 
                                                       Learningmethod = r.LearningMethod,
                                                       Class = r.IdClassNavigation.ClassName,
                                                       Timetable = r.TimeTable,
                                                       Totalsession = r.TotalSession,
                                                       Timestart = r.TimeStart.ToString(), // Assuming you have TimeStart and TimeEnd in your Schedule model
                                                      Timeend = r.TimeEnd.ToString(),
                                                       Idrequest = r.Id, // Include Account ID
                                                       Fullname = r.IdAccountNavigation.FullName // Include Account Full Name
                                                   }).ToListAsync();


            // Format the Time string if needed
            foreach (var request in pendingRequests)
            {
                if (!string.IsNullOrEmpty(request.Timestart))
                {
                    var timeOnly = TimeOnly.Parse(request.Timestart);
                    request.Timestart = timeOnly.ToString("HH:mm");
                }
            }
            foreach (var request in pendingRequests)
            {
                if (!string.IsNullOrEmpty(request.Timeend))
                {
                    var timeOnly = TimeOnly.Parse(request.Timeend);
                    request.Timeend = timeOnly.ToString("HH:mm");
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
            // Tìm yêu cầu theo IdRequest
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu nào",
                };
            }


            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.Roles.ToLower() == "Gia sư");

            if (account == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư nào với tài khoản này",
                };
            }

            // Tìm gia sư theo IdAccount
            var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.IdAccount == id);


            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư nào",
                };
            }   

            if (account.AccountBalance < 50000)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Bạn cần có 50.000 trong tài khoản để tham gia yêu cầu",
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
                    Message = "Gia sư đã tham gia vào yêu cầu này rồi",
                };
            }

            // Tạo bản ghi mới trong bảng RequestLearning
            var requestLearning = new RequestLearning
            {
                Id = Guid.NewGuid().ToString(),
                IdTutor = tutorId,
                IdRequest = requestId
            };

            _context.RequestLearnings.Add(requestLearning);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Bạn đã tham gia vào yêu cầu của học sinh",
            };
        }

    }
}

