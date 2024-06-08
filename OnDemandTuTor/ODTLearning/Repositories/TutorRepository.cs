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

        public TutorProfileModel GetTutorProfile(string id)
        {
            var account = _context.Accounts.SingleOrDefault(x => x.Id == id);

            if (account == null)
            {
                return null;
            }
            //lay list field cua account
            var fields = _context.Tutors.Where(x => x.IdAccount == id).Join(_context.TutorFields.Join(_context.Fields, tf => tf.IdField, f => f.Id, (tf, f) => new
            {
                AccountId = tf.IdTutor,
                Field = f.FieldName
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
                FirstName = account.FirstName,
                LastName = account.LastName,
                Gmail = account.Gmail,
                Birthdate = account.Birthdate,
                Gender = account.Gender,
                Fields = fields,
                Qualifications = qualifications,
            };
        }

        public bool UpdateTutorProfile(string idTutor, TutorProfileToUpdate model)
        {
            var tutor = _context.Tutors
                .Include(t => t.IdAccountNavigation)
                .Include(t => t.TutorFields)
                .ThenInclude(tf => tf.IdFieldNavigation)
                .Include(t => t.EducationalQualifications)
                .FirstOrDefault(x => x.Id == idTutor);

            if (tutor == null)
            {
                return false;
            }

            tutor.SpecializedSkills = model.SpecializedSkills;
            tutor.Experience = model.Experience;

            if (tutor.IdAccountNavigation != null)
            {
                tutor.IdAccountNavigation.FirstName = model.FisrtName;
                tutor.IdAccountNavigation.LastName = model.LastName;
                tutor.IdAccountNavigation.Gmail = model.Gmail;
                tutor.IdAccountNavigation.Gender = model.Gender;
            }


            var existingField = tutor.TutorFields.FirstOrDefault(tf => tf.IdFieldNavigation.FieldName == model.FieldName);
            if (existingField == null && !string.IsNullOrEmpty(model.FieldName))
            {
                var newField = new TutorField
                {
                    IdTutor = idTutor,
                    IdField = _context.Fields.FirstOrDefault(f => f.FieldName == model.FieldName)?.Id ?? Guid.NewGuid().ToString(),
                };
                tutor.TutorFields.Add(newField);
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
            _context.SaveChanges();

            return true;
        }

        public List<TutorListModel> SearchTutorList(SearchTutorModel model)
        {
            //list all
            var accountQuerry = _context.Accounts.Where(x => x.Role == "Tutor");

            //list search by name
            if (!string.IsNullOrEmpty(model.Name))
            {
                accountQuerry = accountQuerry.Where(x => x.FirstName.Contains(model.Name) || x.LastName.Contains(model.Name));
                if (!accountQuerry.Any())
                {
                    return null;
                }
            }

            //list search by field
            if (!string.IsNullOrEmpty(model.Field))
            {
                //lay field can search
                var field = _context.Fields.FirstOrDefault(x => x.FieldName == model.Field);
                if (field == null)
                {
                    return null;
                }
                accountQuerry = _context.TutorFields.Where(x => x.IdField == field.Id).Join(_context.Tutors, tf => tf.IdTutor, t => t.Id, (tf, t) => t).Join(accountQuerry, t => t.IdAccount, aq => aq.Id, (t, aq) => aq);
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
                var fields = _context.Tutors.Where(x => x.IdAccount == id).Join(_context.TutorFields.Join(_context.Fields, tf => tf.IdField, f => f.Id, (tf, f) => new
                {
                    AccountId = tf.IdTutor,
                    Field = f.FieldName
                }), t => t.Id, af => af.AccountId, (t, af) => af.Field).ToList();

                //dua vao model
                var account = _context.Accounts.SingleOrDefault(x => x.Id == id);

                var k = new TutorListModel
                {
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    Gmail = account.Gmail,
                    Birthdate = account.Birthdate,
                    Gender = account.Gender,
                    Field = fields
                };

                list.Add(k);
            }
            return list;
        }
    }
}
