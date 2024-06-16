﻿using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using ODTLearning.Models;




namespace ODTLearning.Repositories
{
    public class ModeratorRepository : IModeratorRepository
    {

        private readonly DbminiCapstoneContext _context;

        public ModeratorRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        public async Task<List<ListTutorToConfirm>?> GetListTutorsToCofirm()
        {
            var tutors = await _context.Tutors
                .Include(t => t.IdAccountNavigation)
                .Where(t => t.Status == "Operating")
                .Select(t => new ListTutorToConfirm
                {
                    Id = t.Id,
                    fullName = t.IdAccountNavigation.FullName
                })
                .ToListAsync();

            // Kiểm tra nếu danh sách tutor rỗng thì trả về null
            if (!tutors.Any())
            {
                return null;
            }

            return tutors;
        }
        public async Task<object?> GetTutorProfileToConfirm(string id)
        {
            var tutorDetails = await _context.Tutors
                .Include(t => t.IdAccountNavigation)
                .Include(t => t.TutorSubjects)
                    .ThenInclude(ts => ts.IdSubjectNavigation)
                .Include(t => t.EducationalQualifications)
                .Where(t => t.Id == id)
                .Select(t => new
                {
                    TutorId = t.Id,
                    SpecializedSkills = t.SpecializedSkills,
                    Experience = t.Experience,
                    Status = t.Status,
                    Account = new
                    {
                        Id = t.IdAccountNavigation.Id,
                        FullName = t.IdAccountNavigation.FullName,
                        Email = t.IdAccountNavigation.Email,
                        DateOfBirth = t.IdAccountNavigation.DateOfBirth,
                        Gender = t.IdAccountNavigation.Gender,
                        Roles = t.IdAccountNavigation.Roles,
                        Avatar = t.IdAccountNavigation.Avatar,
                        Address = t.IdAccountNavigation.Address,
                        Phone = t.IdAccountNavigation.Phone,
                        AccountBalance = t.IdAccountNavigation.AccountBalance
                    },
                    Fields = t.TutorSubjects.Select(ts => new
                    {
                        FieldId = ts.IdSubject,
                        FieldName = ts.IdSubjectNavigation.SubjectName
                    }),
                    EducationalQualifications = t.EducationalQualifications.Select(eq => new
                    {
                        CertificateName = eq.QualificationName,
                        Type = eq.Type,
                        Img = eq.Img
                    })
                })
                .FirstOrDefaultAsync();

            return tutorDetails;
        }
        public async Task<ApiResponse<bool>> ConfirmRequest(string requestId, string status)
        {
            var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tim thấy yêu cầu nào",
                    Data = false
                };
            }

            if (status.ToLower() == "approved")
            {
                request.Status = "approved";
                _context.Requests.Update(request);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Yêu cầu đã được duyệt",
                    Data = true
                };
            }
            else if (status.ToLower() == "reject")
            {
                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Yêu cầu của bạn không được duyệt",
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
        public async Task<bool> ConfirmProfileTutor(string idTutor, string status)
        {
            var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.Id == idTutor);
            if (tutor == null)
            {
                return false;
            }

            if (status.ToLower() == "approved" || status.ToLower() == "reject")
            {
                tutor.Status = status.ToLower();
                _context.Tutors.Update(tutor);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetPendingRequests()
        {
            var pendingRequests = await _context.Requests
                                                 .Where(r => r.Status == "Pending")
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
            foreach (var request in pendingRequests)
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
                Message = "Yêu cầu đang chờ xử lý được truy xuất thành công",
                Data = pendingRequests
            };
        }





    }
}
