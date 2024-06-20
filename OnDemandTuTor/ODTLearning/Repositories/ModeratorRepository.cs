using Microsoft.EntityFrameworkCore;
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
        public async Task<ApiResponse<bool>> ApproveRequest(string requestId)
        {
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

            request.Status = "Đã duyệt";
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Yêu cầu đã được duyệt",
                Data = true
            };
        }

        public async Task<ApiResponse<bool>> RejectRequest(string requestId)
        {
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

            request.Status = "Từ chối"; // Assuming "Rejected" is the correct status for rejection
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Yêu cầu của bạn không được duyệt",
                Data = true
            };
        }

        public async Task<bool> ConfirmProfileTutor(string idTutor, string status)
        {
            var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.Id == idTutor);
            if (tutor == null)
            {
                return false;
            }

            if (status.ToLower() == "đã duyệt" || status.ToLower() == "từ chối")
            {
                tutor.Status = status.ToLower();
                _context.Tutors.Update(tutor);

                if (status.ToLower() == "đã duyệt")
                {
                    var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == tutor.IdAccount);
                    account.Roles = "Tutor";
                    _context.Accounts.Update(account);
                }

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetPendingRequests()
        {
            // Truy vấn danh sách các request có status là "chưa duyệt"
            var pendingRequests = await _context.Requests
                .Where(r => r.Status == "chưa duyệt")
                .Select(r => new ViewRequestOfStudent
                {
                    Title = r.Title,
                    Price = r.Price,
                    Description = r.Description,
                    Subject = r.IdSubjectNavigation.SubjectName, // Assuming you have a Subject property in your Request model
                    LearningMethod = r.LearningMethod,
                    Class = r.IdClassNavigation.ClassName,
                    Date = r.Schedules.FirstOrDefault().Date,
                    TimeStart = r.Schedules.FirstOrDefault().TimeStart.ToString(),
                    TimeEnd = r.Schedules.FirstOrDefault().TimeEnd.ToString(),
                    IdRequest = r.Id,
                    Status = r.Status,
                    FullName = r.IdAccountNavigation.FullName // Include Account Full Name
                }).ToListAsync();

            // Format the Time string if needed
            foreach (var request in pendingRequests)
            {
                if (!string.IsNullOrEmpty(request.TimeStart))
                {
                    var timeOnly = TimeOnly.Parse(request.TimeStart);
                    request.TimeStart = timeOnly.ToString("HH:mm");
                }
                if (!string.IsNullOrEmpty(request.TimeEnd))
                {
                    var timeOnly = TimeOnly.Parse(request.TimeEnd);
                    request.TimeEnd = timeOnly.ToString("HH:mm");
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
