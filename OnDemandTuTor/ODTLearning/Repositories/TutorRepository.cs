using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using ODTLearning.Helpers;
using ODTLearning.Models;


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

        public async Task<object> GetTutorProfile(string id)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id && x.Roles == "gia sư");

            if (account == null)
            {
                return null;
            }

            var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.IdAccount == id);

            //lay list field cua account
            var fields = _context.Tutors.Where(x => x.IdAccount == id).Join(_context.TutorSubjects.Join(_context.Subjects, tf => tf.IdSubject, f => f.Id, (tf, f) => new
            {
                AccountId = tf.IdTutor,
                Field = f.SubjectName
            }), t => t.Id, af => af.AccountId, (t, af) => af.Field).ToList();

            //lay list Qualification cua account
            var qualifications = _context.Tutors.Where(x => x.IdAccount == id).Join(_context.EducationalQualifications, t => t.Id, eq => eq.IdTutor, (t, eq) => new
            {
                Name = eq.QualificationName,
                Img = imgLib.GetImanges(eq.Img),
                Type = eq.Type
            }).ToList();

            //dua vao model            
            return new
            {
                Id = id,
                Gmail = account.Email,
                Birthdate = account.DateOfBirth,
                Gender = account.Gender,
                Avatar = account.Avatar,
                SpeacializedSkill = tutor.SpecializedSkills,
                Experience = tutor.Experience,
                Fields = fields,
                Qualifications = qualifications,
            };
        }

        public async Task<bool> UpdateTutorProfile(string idTutor, TutorProfileToUpdate model)
        {
            var tutor = await _context.Tutors
                .Include(t => t.IdAccountNavigation)
                .Include(t => t.TutorSubjects)
                .ThenInclude(tf => tf.IdSubjectNavigation)
                .Include(t => t.EducationalQualifications)
                .FirstOrDefaultAsync(x => x.Id == idTutor);

            if (tutor == null)
            {
                return false;
            }

            tutor.SpecializedSkills = model.SpecializedSkills;
            tutor.Experience = model.Experience;

            if (tutor.IdAccountNavigation != null)
            {
                tutor.IdAccountNavigation.FullName = model.Fullname;
                tutor.IdAccountNavigation.Email = model.Gmail;
                tutor.IdAccountNavigation.Gender = model.Gender;
            }


            var existingField = tutor.TutorSubjects.FirstOrDefault(tf => tf.IdTutorNavigation.Id == model.FieldName);
            if (existingField == null && !string.IsNullOrEmpty(model.FieldName))
            {
                var newField = new TutorSubject
                {
                    IdTutor = idTutor,
                    IdSubject = _context.Subjects.FirstOrDefault(f => f.SubjectName == model.FieldName).Id ?? Guid.NewGuid().ToString(),
                };
                tutor.TutorSubjects.Add(newField);
            }


            var existingQualification = tutor.EducationalQualifications.FirstOrDefault(eq => eq.QualificationName == model.FieldName);
            if (existingQualification == null && !string.IsNullOrEmpty(model.FieldName))
            {
                var newQualification = new EducationalQualification
                {
                    Id = Guid.NewGuid().ToString(),
                    QualificationName = model.FieldName,
                    IdTutor = idTutor
                };
                tutor.EducationalQualifications.Add(newQualification);
            }

            _context.Tutors.Update(tutor);
            await _context.SaveChangesAsync();

            return true;
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
                                                       Subject = r.IdSubjectNavigation.SubjectName, // Assuming you have a Subject property in your Request model
                                                       Learningmethod = r.LearningMethod,
                                                       Class = r.IdClassNavigation.ClassName,
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

        public async Task<ApiResponse<bool>> JoinRequest(string requestId, string idAccount)
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


            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == idAccount && a.Roles.ToLower() == "gia sư");

            if (account == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư nào với tài khoản này",
                };
            }

            // Tìm gia sư theo IdAccount
            var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.IdAccount == idAccount);


            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư nào",
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

