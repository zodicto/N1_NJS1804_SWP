using Firebase.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ODTLearning.Entities;
using ODTLearning.Models;


namespace ODTLearning.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DbminiCapstoneContext _context;

        public AdminRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAccount(string IDAccount)
        {
            bool result = false;

            try
            {
                var exsitAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == IDAccount);
                if (exsitAccount != null)
                {

                    if (exsitAccount.Roles.ToLower() == "học sinh")
                    {
                        _context.Accounts.Remove(exsitAccount);
                        await _context.SaveChangesAsync();
                        result = true;
                    }
                    else if (exsitAccount.Roles.ToLower() == "gia sư")
                    {
                        var tutor = _context.Tutors.FirstOrDefault(x => x.IdAccount == IDAccount);
                        // Xóa các đối tượng educational qualifications liên quan đến tutor
                        var educationalQualifications = _context.EducationalQualifications.Where(eq => eq.IdTutor == tutor.Id).ToList();
                        if (educationalQualifications.Any())
                        {
                            _context.EducationalQualifications.RemoveRange(educationalQualifications);
                        }

                        // Xóa các đối tượng tutor fields liên quan đến tutor
                        var tutorFields = _context.TutorSubjects.Where(tf => tf.IdTutor == tutor.Id).ToList();
                        if (tutorFields.Any())
                        {
                            _context.TutorSubjects.RemoveRange(tutorFields);
                        }

                        // Xóa đối tượng tutor
                        _context.Tutors.Remove(tutor);
                        await _context.SaveChangesAsync();
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu có xảy ra
                // Sử dụng logging framework như NLog, Serilog, hoặc bất kỳ framework nào bạn đang sử dụng
                Console.WriteLine($"Error while deleting account: {ex.Message}");
            }

            return result;
        }

        public async Task<ApiResponse<object>> ViewRent(string Condition)
        {
            var rent = _context.Rents.AsQueryable();

            DateTime? date = null;

            if (Condition == "1 tuần") 
            {
                date = DateTime.Now.AddDays(-7);
            }
            if (Condition == "1 tháng")
            {
                date = DateTime.Now.AddMonths(-1);
            }

            if (date != null)
            {
                rent = rent.Where(x => x.CreateDate >= date);
            }

            var data = rent.Include(x => x.IdRequestNavigation).ThenInclude(x => x.IdSubjectNavigation)
                           .Include(x => x.IdRequestNavigation).ThenInclude(x => x.IdAccountNavigation)
                           .Join(_context.Tutors.Include(t => t.IdAccountNavigation), r => r.IdTutor, t => t.Id, (r, t) => new
                           {
                               User = new
                               {
                                   Name = r.IdRequestNavigation.IdAccountNavigation.FullName,
                                   Email = r.IdRequestNavigation.IdAccountNavigation.Email,
                                   DateOfBirth = r.IdRequestNavigation.IdAccountNavigation.DateOfBirth,
                                   Gender = r.IdRequestNavigation.IdAccountNavigation.Gender,
                                   Avatar = r.IdRequestNavigation.IdAccountNavigation.Avatar,
                                   Address = r.IdRequestNavigation.IdAccountNavigation.Address,
                                   Phone = r.IdRequestNavigation.IdAccountNavigation.Phone
                               },
                               Subject = r.IdRequestNavigation.IdSubjectNavigation.SubjectName,
                               //Price = x.Price,
                               CreateDate = r.CreateDate,
                               Tutor = new
                               {
                                   Name = t.IdAccountNavigation.FullName,
                                   Email = t.IdAccountNavigation.Email,
                                   DateOfBirth = t.IdAccountNavigation.DateOfBirth,
                                   Gender = t.IdAccountNavigation.Gender,
                                   Avatar = t.IdAccountNavigation.Avatar,
                                   Address = t.IdAccountNavigation.Address,
                                   Phone = t.IdAccountNavigation.Phone
                               },
                           }).ToList();

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = data
            };
        }
    }
}
