using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
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

        

        public async Task<TutorProfileModel> GetTutorProfile(string id)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id);

            if (account == null)
            {
                return null;
            }
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
                Img = eq.Img,
                Type = eq.Type
            }).ToList();

            //dua vao model            
            return new TutorProfileModel
            {
                Id = id,              
                Gmail = account.Email,
                Birthdate = account.DateOfBirth,
                Gender = account.Gender,
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

        public async Task<List<TutorListModel>> SearchTutorList(SearchTutorModel model)
        {
            //list all
            var accountQuerry = _context.Accounts.Where(x => x.Roles == "Tutor");

            //list search by name
            if (!string.IsNullOrEmpty(model.Name))
            {
                accountQuerry = accountQuerry.Where(x => x.FullName.Contains(model.Name));
                if (!accountQuerry.Any())
                {
                    return null;
                }
            }

            //list search by field
            if (!string.IsNullOrEmpty(model.Field))
            {
                //lay field can search
                var field = await _context.Subjects.FirstOrDefaultAsync(x => x.SubjectName == model.Field);
                if (field == null)
                {
                    return null;
                }
                accountQuerry = _context.TutorSubjects.Where(x => x.IdSubject == field.Id).Join(_context.Tutors, tf => tf.IdTutor, t => t.Id, (tf, t) => t).Join(accountQuerry, t => t.IdAccount, aq => aq.Id, (t, aq) => aq);
            }

            if (!accountQuerry.Any())
            {
                return null;
            }
            //lay id cua cac account can search
            var idAccountQuerry = accountQuerry.Select(x => x.Id).ToList();

            var list = new List<TutorListModel>();

            foreach (var id in idAccountQuerry)
            {
                //lay list cac ten field cua tung account
                var fields = _context.Tutors.Where(x => x.IdAccount == id).Join(_context.TutorSubjects.Join(_context.Subjects, tf => tf.IdSubject, f => f.Id, (tf, f) => new
                {
                    AccountId = tf.IdTutor,
                    Field = f.SubjectName
                }), t => t.Id, af => af.AccountId, (t, af) => af.Field).ToList();

                //dua vao model
                var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id);

                var k = new TutorListModel
                {
                    FirstName = account.FullName,
                    Gmail = account.Email,
                    Birthdate = account.DateOfBirth,
                    Gender = account.Gender,
                    Field = fields
                };

                list.Add(k);
            }
            return list;
        }

        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetApprovedRequests()
        {
            var appeovedRequests = await _context.Requests
                                                 .Where(r => r.Status == "approved")
                                                 .Select(r => new ViewRequestOfStudent
                                                 {
                                                     Title = r.Title,
                                                     Price = r.Price,
                                                     Description = r.Description,
                                                     LearningMethod = r.LearningMethod,
                                                     LearningModel = r.IdLearningModelsNavigation.NameModel,
                                                     Date = r.Schedules.FirstOrDefault().Date,
                                                     Time = r.Schedules.FirstOrDefault().Time.ToString() // Use ToString() without parameters
                                                 }).ToListAsync();

            // Format the Time string if needed
            foreach (var request in appeovedRequests)
            {
                if (!string.IsNullOrEmpty(request.Time))
                {
                    var timeOnly = TimeOnly.Parse(request.Time);
                    request.Time = timeOnly.ToString("HH:mm");
                }
            }

            return new ApiResponse<List<ViewRequestOfStudent>>
            {
                Success = true,
                Message = "Yêu cầu đã xử lý được truy xuất thành công",
                Data = appeovedRequests
            };
        }

        public async Task<ApiResponse<bool>> JoinRequest(string requestId, string tutorId, JoinRequestModel joinRequestModel)
        {
            // Tìm yêu cầu theo IdRequest
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

            // Tìm gia sư theo IdTutor
            var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.Id == tutorId);

            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư nào",
                    Data = false
                };
            }

            if (joinRequestModel.status?.ToLower() == "joined")
            {
                request.Status = "Joined";
                _context.Requests.Update(request);

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
                    Data = true
                };
            }
            else
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Sai trạng thái duyệt",
                    Data = false
                };
            }
        }



    }
}

