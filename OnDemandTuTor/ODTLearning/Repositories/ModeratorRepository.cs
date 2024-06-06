﻿using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;

namespace ODTLearning.Repositories
{
    public class ModeratorRepository : IModaretorRepository
    {

        private readonly DbminiCapstoneContext _context;

        public ModeratorRepository(DbminiCapstoneContext context)
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
        public string ChangeRequestLearningStatus(string requestId, string status)
        {
            // Tìm yêu cầu học tập theo IdRequest
            var request = _context.Requests.FirstOrDefault(r => r.IdPost == requestId);

            if (request == null)
            {
                return "Request not found";
            }

            if (status.ToLower() == "approve")
            {
                request.Status = "approved";
                _context.Requests.Update(request);
                _context.SaveChanges();
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
    }
}