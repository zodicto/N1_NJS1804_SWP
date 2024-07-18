using Microsoft.EntityFrameworkCore;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODTLearning.BLL.Repositories
{
    public class TransactionRepository
    {
        private readonly DbminiCapstoneContext _context;

        public TransactionRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<object>> GetAllTransaction()
        {
            var transactions = _context.Transactions.Include(x => x.IdAccountNavigation).Select(x => new
            {
                Id = x.Id,
                Amount = x.Amount,
                CreateDate = x.CreateDate,
                Status = x.Status,
                User = new
                {
                    Id = x.IdAccountNavigation.Id,
                    FullName = x.IdAccountNavigation.FullName,
                    Email = x.IdAccountNavigation.Email,
                    Date_of_birth = x.IdAccountNavigation.DateOfBirth,
                    Gender = x.IdAccountNavigation.Gender,
                    Avatar = x.IdAccountNavigation.Avatar,
                    Address = x.IdAccountNavigation.Address,
                    Phone = x.IdAccountNavigation.Phone,
                    Roles = x.IdAccountNavigation.Roles
                }
            }).ToList();

            if (!transactions.Any())
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Không có giao dịch nào"
                };
            }

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = transactions
            };
        }

    }
}
