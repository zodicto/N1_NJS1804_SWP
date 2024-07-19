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

        public async Task<ApiResponse<object>> GetTransactionByMonth(int year)
        {
            var transactions = await _context.Transactions.Where(x => x.CreateDate.Year == year).GroupBy(x => x.CreateDate.Month).Select(x => new
            {
                Name = $"Tháng {x.Key}",
                Data = x.Sum(r => r.Amount)
            }).ToListAsync();

            if (!transactions.Any())
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = $"Không có giao dịch nào trong năm {year}"
                };
            }
            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = transactions
            };
        }

        public async Task<ApiResponse<object>> GetTransactionByWeek(int month, int year)
        {
            var transactions = _context.Transactions.Where(x => x.CreateDate.Month == month && x.CreateDate.Year == year);

            if (!transactions.Any())
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = $"Không có giao dịch nào trong {month}/{year}"
                };
            }

            var data = new List<object>();
            float price1 = 0;
            float price2 = 0;
            float price3 = 0;
            float price4 = 0;

            foreach (var x in transactions)
            {
                if (x.CreateDate.Day <= 7)
                {
                    price1 += (float)x.Amount;
                }
                else if (x.CreateDate.Day > 7 && x.CreateDate.Day <= 14)
                {
                    price2 += (float)x.Amount;
                }
                else if (x.CreateDate.Day > 14 && x.CreateDate.Day <= 21)
                {
                    price3 += (float)x.Amount;
                }
                else
                {
                    price4 += (float)x.Amount;
                }
            }

            var monthData = "";

            if (month < 10)
            {
                monthData = $"0{month}";
            }
            else
            {
                monthData = $"{month}";
            }

            var lastDay = new DateTime(year, month, 1).AddMonths(1).AddDays(-1).Day;

            var week1 = new
            {
                Name = "Tuần 1",
                Date = $"01/{monthData}/{year} - 07/{monthData}/{year}",
                Revenue = price1,
                Month = month,
                Year = year
            };

            var week2 = new
            {
                Name = "Tuần 2",
                Date = $"08/{monthData}/{year} - 14/{monthData}/{year}",
                Revenue = price2,
                Month = month,
                Year = year
            };

            var week3 = new
            {
                Name = "Tuần 3",
                Date = $"15/{monthData}/{year} - 21/{monthData}/{year}",
                Revenue = price3,
                Month = month,
                Year = year
            };

            var week4 = new
            {
                Name = "Tuần 4",
                Date = $"22/{monthData}/{year} - {lastDay}/{monthData}/{year}",
                Revenue = price4,
                Month = month,
                Year = year
            };

            data.Add(week1);
            data.Add(week2);
            data.Add(week3);
            data.Add(week4);

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = data
            };
        }

        public async Task<ApiResponse<object>> GetTransactionByYear(int year)
        {
            var data = new List<object>();

            for (int month = 1; month <= 12; month++)
            {
                var transactions = _context.Transactions.Where(x => x.CreateDate.Month == month && x.CreateDate.Year == year);

                float price1 = 0;
                float price2 = 0;
                float price3 = 0;
                float price4 = 0;

                foreach (var x in transactions)
                {
                    if (x.CreateDate.Day <= 7)
                    {
                        price1 += (float)x.Amount;
                    }
                    else if (x.CreateDate.Day > 7 && x.CreateDate.Day <= 14)
                    {
                        price2 += (float)x.Amount;
                    }
                    else if (x.CreateDate.Day > 14 && x.CreateDate.Day <= 21)
                    {
                        price3 += (float)x.Amount;
                    }
                    else
                    {
                        price4 += (float)x.Amount;
                    }
                }

                var monthData = "";

                if (month < 10)
                {
                    monthData = $"0{month}";
                }
                else
                {
                    monthData = $"{month}";
                }

                var lastDay = new DateTime(year, month, 1).AddMonths(1).AddDays(-1).Day;

                var week1 = new
                {
                    Name = "Tuần 1",
                    Date = $"01/{monthData}/{year} - 07/{monthData}/{year}",
                    Revenue = price1,
                    Month = month,
                    Year = year
                };

                var week2 = new
                {
                    Name = "Tuần 2",
                    Date = $"08/{monthData}/{year} - 14/{monthData}/{year}",
                    Revenue = price2,
                    Month = month,
                    Year = year
                };

                var week3 = new
                {
                    Name = "Tuần 3",
                    Date = $"15/{monthData}/{year} - 21/{monthData}/{year}",
                    Revenue = price3,
                    Month = month,
                    Year = year
                };

                var week4 = new
                {
                    Name = "Tuần 4",
                    Date = $"22/{monthData}/{year} - {lastDay}/{monthData}/{year}",
                    Revenue = price4,
                    Month = month,
                    Year = year
                };

                data.Add(week1);
                data.Add(week2);
                data.Add(week3);
                data.Add(week4);
            }

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = data
            };
        }

        public async Task<ApiResponse<object>> GetTransactionThisMonth()
        {
            var now = DateTime.Now;

            var transactions = await _context.Transactions.Where(x => x.CreateDate.Year == now.Year && x.CreateDate.Month == now.Month).ToListAsync();

            if (!transactions.Any())
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "không có giao dịch nào trong tháng này"
                };
            }

            float? revenue = 0;
            foreach (var x in transactions)
            {
                revenue += x.Amount;
            }

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = revenue
            };
        }

        public async Task<ApiResponse<object>> GetTransactionToday()
        {
            var revenue = await _context.Transactions.Where(x => DateOnly.FromDateTime(x.CreateDate) == DateOnly.FromDateTime(DateTime.Now)).SumAsync(x => x.Amount);

            if (revenue == 0)
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "không có giao dịch nào trong tháng này",
                    Data = revenue
                };
            }

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = revenue
            };
        }

    }
}
