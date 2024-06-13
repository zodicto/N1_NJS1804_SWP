using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;




namespace ODTLearning.Repositories
{
    public class ModeratorRepository : IModeratorRepository
    {

        private readonly DbminiCapstoneContext _context;

        public ModeratorRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }
        public async Task<object> GetTutorProfileToConFirm(string id)
        {
            var accountDetails = _context.Accounts
                .Where(a => a.Id == id)
                .Select(a => new
                {
                    Username = a.FullName,
                    TutorDetails = a.Tutors.Select(t => new
                    {
                        SpecializedSkills = t.SpecializedSkills,
                        Experience = t.Experience,
                        Status = t.Status,
                        Fields = t.TutorSubjects.Select(tf => new
                        {
                            FieldId = tf.IdSubject,
                            FieldName = tf.IdSubjectNavigation.SubjectName
                        }),
                        EducationalQualifications = t.EducationalQualifications.Select(eq => new
                        {
                            CertificateName = eq.QualificationName,
                            Type = eq.Type
                        })
                    })
                }).ToList();

            return accountDetails;
        }
        public async Task<string> ChangeRequestLearningStatus(string requestId, string status)
        {
            // Tìm yêu cầu học tập theo IdRequest
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
            {
                return "Request not found";
            }

            if (status.ToLower() == "approve")
            {
                request.Status = "approved";
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
                return "Request approved successfully";
            }
            else if (status.ToLower() == "reject")
            {
                return "Request rejected, no changes saved";
            }
            else
            {
                return "Invalid status provided";
            }
        }
        public async Task<bool> ConFirmProfileTutor(string idTutor, string status)
        {
            var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.Id == idTutor);
            if (tutor == null)
            {
                return false;
            }

            tutor.Status = status;
            _context.Tutors.Update(tutor);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
