using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;

namespace ODTLearning.Repositories
{
    public class TutorRepository : ITutorRepository
    {
        private readonly DbminiCapstoneContext _context;
        private readonly IConfiguration _configuration;

        public TutorRepository(DbminiCapstoneContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public object GetTutorProfileToConFirm(string id)
        {
            var accountDetails = _context.Accounts
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
                            Organization = eq.Organization,
                            Type = eq.Type
                        })
                    })
                }).ToList();

            return accountDetails;
        }

        public bool ConFirmProfileTutor(string idTutor,string status)
        {
            bool result = false;
            var StatusTutor = _context.Tutors.Where(x => x.IdTutor == idTutor);
            foreach (var item in StatusTutor)
            {
                item.Status = status;
                result = true;

            }
            return result;
        }
        

        

    }
}
