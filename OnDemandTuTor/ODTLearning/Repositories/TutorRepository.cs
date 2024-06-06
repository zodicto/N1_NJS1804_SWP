using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using ODTLearning.Models;


namespace ODTLearning.Repositories
{
    public class TutorRepository : ITutorRepository
    {
        private readonly DbminiCapstoneContext _context;

        public bool UpdateTutorProfile(string idTutor, TutorProfileToUpdate model)
        {
            var tutor = _context.Tutors
                .Include(t => t.IdAccountNavigation)
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
                    IdField = _context.Fields.FirstOrDefault(f => f.FieldName == model.FieldName)?.IdField ?? Guid.NewGuid().ToString(),
                };
                tutor.TutorFields.Add(newField);
            }


            var existingQualification = tutor.EducationalQualifications.FirstOrDefault(eq => eq.CertificateName == model.FieldName);
            if (existingQualification == null && !string.IsNullOrEmpty(model.FieldName))
            {
                var newQualification = new EducationalQualification
                {
                    IdEducationalEualifications = Guid.NewGuid().ToString(),
                    CertificateName = model.FieldName,
                    IdTutor = idTutor
                };
                tutor.EducationalQualifications.Add(newQualification);
            }

            _context.Tutors.Update(tutor);
            _context.SaveChanges();

            return true;
        }
    }
}
