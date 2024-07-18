using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODTLearning.BLL.Repositories
{
    public class NotificationRepository
    {
        private readonly DbminiCapstoneContext _context;

        public NotificationRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<object>> GetAllNotification(string id)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng"
                };
            }

            var nofies = await _context.Notifications.Where(x => x.IdAccount == id)
                                                     .Select(x => new
                                                     {
                                                         IdNotification = x.Id,
                                                         x.Description,
                                                         x.CreateDate,
                                                         x.Status,
                                                     })
                                                     .ToListAsync();

            if (nofies == null)
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Bạn không có thông báo"
                };
            }

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = nofies
            };
        }

        public async Task<ApiResponse<object>> UpdateStatusNotification(string id)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng"
                };
            }

            var nofies = await _context.Notifications.Where(x => x.IdAccount == id).ToListAsync();

            if (nofies == null)
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Bạn không có thông báo"
                };
            }

            foreach (var nofi in nofies)
            {
                nofi.Status = "Đã xem";
            }

            await _context.SaveChangesAsync();

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
            };
        }

    }
}
