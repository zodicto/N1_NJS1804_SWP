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

        public async Task<ApiResponse<List<ListTutorToConfirmFB>>> GetListTutorsToConfirm()
        {
            try
            {
                var tutors = await _context.Tutors
                    .Include(t => t.IdAccountNavigation)
                    .Include(t => t.TutorSubjects)
                        .ThenInclude(ts => ts.IdSubjectNavigation)
                    .Include(t => t.EducationalQualifications)
                    .Where(t => t.Status == "Chưa duyệt")
                    .Select(t => new ListTutorToConfirmFB
                    {
                        Id = t.IdAccount, // Sử dụng Id của Tutor
                        SpecializedSkills = t.SpecializedSkills,
                        Experience = t.Experience,
                        Subject = t.TutorSubjects.FirstOrDefault().IdSubjectNavigation.SubjectName, // Lấy Subject từ TutorSubjects
                        QualificationName = t.EducationalQualifications.FirstOrDefault().QualificationName, // Lấy QualificationName từ EducationalQualifications
                        Type = t.EducationalQualifications.FirstOrDefault().Type, // Lấy Type từ EducationalQualifications
                        ImageQualification = t.EducationalQualifications.FirstOrDefault().Img // Lấy ImageQualification từ EducationalQualifications
                    })
                    .ToListAsync();

                if (!tutors.Any())
                {
                    return new ApiResponse<List<ListTutorToConfirmFB>>
                    {
                        Success = false,
                        Message = "Không có gia sư nào cần xác nhận",
                        Data = null
                    };
                }

                return new ApiResponse<List<ListTutorToConfirmFB>>
                {
                    Success = true,
                    Message = "Lấy danh sách gia sư thành công",
                    Data = tutors
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListTutorsToConfirm: {ex.Message}");

                return new ApiResponse<List<ListTutorToConfirmFB>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách gia sư",
                    Data = null
                };
            }
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

        public async Task<ApiResponse<bool>> ApproveProfileTutor(string id)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.IdAccount == id);

                if (tutor == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy gia sư với ID tài khoản này",
                        Data = false
                    };
                }

                tutor.Status = "Đã duyệt";
                _context.Tutors.Update(tutor);

                var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == tutor.IdAccount);
                account.Roles = "Gia sư";
                _context.Accounts.Update(account);

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Duyệt gia sư thành công",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in ApproveProfileTutor: {ex.Message}");

                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình duyệt gia sư",
                    Data = false
                };
            }
        }

        public async Task<ApiResponse<bool>> RejectProfileTutor(string id)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.IdAccount == id);

                if (tutor == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Không tìm thấy gia sư với ID tài khoản này",
                        Data = false
                    };
                }

                tutor.Status = "Từ chối";
                _context.Tutors.Update(tutor);

                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Từ chối gia sư thành công",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in RejectProfileTutor: {ex.Message}");

                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình từ chối gia sư",
                    Data = false
                };
            }
        }



        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetPendingRequests()
        {
            // Truy vấn danh sách các request có status là "chưa duyệt"
            var pendingRequests = await _context.Requests
                .Where(r => r.Status == "Chưa duyệt")
                .Select(r => new ViewRequestOfStudent
                {
                    Title = r.Title,
                    Price = r.Price,
                    TotalSession = r.TotalSession,
                    TimeTable = r.TimeTable,
                    Description = r.Description,
                    Subject = r.IdSubjectNavigation.SubjectName, // Assuming you have a Subject property in your Request model
                    LearningMethod = r.LearningMethod,
                    Class = r.IdClassNavigation.ClassName,
                    TimeStart = r.TimeStart.HasValue ? r.TimeStart.Value.ToString("HH:mm") : null,
                    TimeEnd = r.TimeEnd.HasValue ? r.TimeEnd.Value.ToString("HH:mm") : null,
                    IdRequest = r.Id,
                    Status = r.Status,
                    FullName = r.IdAccountNavigation.FullName // Include Account Full Name
                }).ToListAsync();

            return new ApiResponse<List<ViewRequestOfStudent>>
            {
                Success = true,
                Message = "Yêu cầu đang chờ xử lý được truy xuất thành công",
                Data = pendingRequests
            };
        }







    }
}
