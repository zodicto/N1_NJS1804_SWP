using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ODTLearning.Entities;


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

                    if (exsitAccount.Role == "Student")
                    {
                        _context.Accounts.Remove(exsitAccount);
                        await _context.SaveChangesAsync();
                        result = true;
                    }
                    else if (exsitAccount.Role == "Tutor")
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
    }
}
