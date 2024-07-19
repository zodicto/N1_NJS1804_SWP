
using ODTLearning.Models;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;
using ODTLearning.DAL;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ODTLearning.BLL.Helpers;
using ODTLearning.DAL.Entities;


namespace ODTLearning.BLL.Repositories
{
    public class RevenueRepository : IRevenueRepository
    {
        private readonly DbminiCapstoneContext _context;

        public RevenueRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        MyLibrary myLib = new MyLibrary();
               
        public async Task<ApiResponse<object>> GetRevenueByYear(int year)
        {
            var data = new List<object>();

            for (int month = 1; month <= 12; month++)
            {
                var requests = await _context.Requests.Where(x => x.CreateDate.Month == month && x.CreateDate.Year == year && (x.Status.ToLower() == "đang diễn ra" || x.Status.ToLower() == "hoàn thành")).ToListAsync();

                var bookings = await _context.Bookings.Include(x => x.IdTimeSlotNavigation.IdDateNavigation).Where(x => x.IdTimeSlotNavigation.IdDateNavigation.Date1.Month == month && x.IdTimeSlotNavigation.IdDateNavigation.Date1.Year == year && x.Status.ToLower() == "hoàn thành").ToListAsync();

                float price1 = 0;
                float price2 = 0;
                float price3 = 0;
                float price4 = 0;

                if (requests.Any())
                {
                    foreach (var x in requests)
                    {
                        if (x.CreateDate.Day <= 7)
                        {
                            price1 += 50000;
                        }
                        else if (x.CreateDate.Day > 7 && x.CreateDate.Day <= 14)
                        {
                            price2 += 50000;
                        }
                        else if (x.CreateDate.Day > 14 && x.CreateDate.Day <= 21)
                        {
                            price3 += 50000;
                        }
                        else
                        {
                            price4 += 50000;
                        }
                    }
                }

                if (bookings.Any())
                {
                    foreach (var x in bookings)
                    {
                        if (x.IdTimeSlotNavigation.IdDateNavigation.Date1.Day <= 7)
                        {
                            price1 += (float)x.Price / 10;
                        }
                        else if (x.IdTimeSlotNavigation.IdDateNavigation.Date1.Day > 7 && x.IdTimeSlotNavigation.IdDateNavigation.Date1.Day <= 14)
                        {
                            price2 += (float)x.Price / 10;
                        }
                        else if (x.IdTimeSlotNavigation.IdDateNavigation.Date1.Day > 14 && x.IdTimeSlotNavigation.IdDateNavigation.Date1.Day <= 21)
                        {
                            price3 += (float)x.Price / 10;
                        }
                        else
                        {
                            price4 += (float)x.Price / 10;
                        }
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

        public async Task<ApiResponse<object>> GetRevenueThisMonth()
        {
            var now = DateTime.Now;

            var countRequest = await _context.Requests.Where(x => x.CreateDate.Month == now.Month && x.CreateDate.Year == now.Year && (x.Status.ToLower() == "đang diễn ra" || x.Status.ToLower() == "hoàn thành")).CountAsync();

            var bookings = await _context.Bookings.Include(x => x.IdTimeSlotNavigation.IdDateNavigation).Where(x => x.IdTimeSlotNavigation.IdDateNavigation.Date1.Month == now.Month && x.IdTimeSlotNavigation.IdDateNavigation.Date1.Year == now.Year && x.Status.ToLower() == "hoàn thành").ToListAsync();            

            float revenue = 0;

            if (countRequest > 0)
            {
                revenue += countRequest * 50000;
            }

            if (bookings.Any())
            {
                foreach (var x in bookings)
                {
                    revenue += (float)x.Price / 10;
                }
            }
            
            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = revenue
            };
        }              

        public async Task<ApiResponse<object>> GetRevenueToday()
        {            
            var countRequest = await _context.Requests.Where(x => DateOnly.FromDateTime(x.CreateDate) == DateOnly.FromDateTime(DateTime.Now) && (x.Status.ToLower() == "đang diễn ra" || x.Status.ToLower() == "hoàn thành")).CountAsync();

            var bookings = await _context.Bookings.Include(x => x.IdTimeSlotNavigation.IdDateNavigation).Where(x => x.IdTimeSlotNavigation.IdDateNavigation.Date1 == DateOnly.FromDateTime(DateTime.Now) && x.Status.ToLower() == "hoàn thành").ToListAsync();

            float revenue = 0;

            if (countRequest > 0)
            {
                revenue += countRequest * 50000;
            }

            if (bookings.Any())
            {
                foreach (var x in bookings)
                {
                    revenue += (float)x.Price / 10;
                }
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
