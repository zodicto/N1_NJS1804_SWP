
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using ODTLearning.DAL.Entities;
using ODTLearning.BLL.Helpers;
using ODTLearning.Models;
using System.Globalization;
using System.Linq;
using System.Net;


namespace ODTLearning.BLL.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbminiCapstoneContext _context;
        public StudentRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }
        
        public async Task<ApiResponse<List<ListAllStudent>>> GetListStudent()
        {
            try
            {
                var ListStudent = await _context.Accounts
                   .Where(t => t.Roles == "Học sinh")
                    .Select(t => new ListAllStudent
                    {
                        id = t.Id,
                        email = t.Email,
                        password = t.Password,
                        date_of_birth = t.DateOfBirth,
                        fullName = t.FullName,
                        gender = t.Gender,
                        phone = t.Phone,
                        roles = t.Roles,
                    }).ToListAsync();
                return new ApiResponse<List<ListAllStudent>>
                {
                    Success = true,
                    Message = "Lấy danh sách học sinh thành công",
                    Data = ListStudent
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListsToConfirm: {ex.Message}");

                return new ApiResponse<List<ListAllStudent>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách gia sư",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<int>> GetAmountStudent()
        {
            var count = _context.Accounts.Count(x => x.Roles.ToLower() == "học sinh");

            return new ApiResponse<int>
            {
                Success = true,
                Message = "Thành công",
                Data = count
            };
        }


    }
}