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
        public object GetTutorProfileToConFirm(string id)
        {
            var accountDetails = _context.Acounts
                .Where(a => a.IdAccount == id)
                .Select(a => new
                {
                    Username = a.Username,
                    TutorDetails = a.Tutors.Select(t => new
                    {
                        SpecializedSkills = t.SpecializedSkills,
                        Experience = t.Experience,
                        Status = t.Status,
                        Fields = t.TutorFields.Select(tf => new
                        {
                            FieldId = tf.IdField,
                            FieldName = tf.IdFieldNavigation.FieldName
                        }),
                        EducationalQualifications = t.EducationalQualifications.Select(eq => new
                        {
                            CertificateName = eq.CertificateName,
                            Type = eq.Type
                        })
                    })
                }).ToList();

            return accountDetails;
        }

        public bool ConFirmProfileTutor(string idTutor, string status)
        {
            var tutor = _context.Tutors.FirstOrDefault(x => x.IdTutor == idTutor);
            if (tutor == null)
            {
                return false;
            }

            tutor.Status = status;
            _context.Tutors.Update(tutor);
            _context.SaveChanges();

            return true;
        }

        public bool UpdateTutorProfile(string idTutor, TutorProfileMVModel model)
        {
            var tutor = _context.Tutors.Include(t => t.IdAccountNavigation)
                                       .Include(t => t.TutorFields)
                                       .ThenInclude(tf => tf.IdFieldNavigation)
                                       .Include(t => t.EducationalQualifications)
                                       .FirstOrDefault(x => x.IdTutor == idTutor);

            if (tutor == null)
            {
                return false;
            }

            tutor.SpecializedSkills = model.SpecializedSkills;
            tutor.Experience = model.Experience;
            tutor.Status = model.Status;

            if (tutor.IdAccountNavigation != null)
            {
                tutor.IdAccountNavigation.FirstName = model.FisrtName;
                tutor.IdAccountNavigation.LastName = model.LastName;
                tutor.IdAccountNavigation.Gmail = model.Gmail;
                tutor.IdAccountNavigation.Birthdate = model.Birthdate;
                tutor.IdAccountNavigation.Gender = model.Gender;
            }

            // Update TutorFields if needed
            // Assuming FieldName is unique, update or add TutorField
            var existingField = tutor.TutorFields.FirstOrDefault(tf => tf.IdFieldNavigation.FieldName == model.FieldName);
            if (existingField == null)
            {
                var newField = new TutorField
                {
                    IdTutor = idTutor,
                    IdField = _context.Fields.FirstOrDefault(f => f.FieldName == model.FieldName)?.IdField ?? Guid.NewGuid().ToString(),
                };
                tutor.TutorFields.Add(newField);
            }

            // Update EducationalQualifications if needed
            // Assuming ImgQualifications represents the CertificateName
            var existingQualification = tutor.EducationalQualifications.FirstOrDefault(eq => eq.CertificateName == model.ImgQualifications);
            if (existingQualification == null)
            {
                var newQualification = new EducationalQualification
                {
                    IdTutor = idTutor,
                    CertificateName = model.ImgQualifications,
                    Type = "Some type", // Update with correct type if available
                };
                tutor.EducationalQualifications.Add(newQualification);
            }

            _context.Tutors.Update(tutor);
            _context.SaveChanges();

            return true;
        }


    }
}
