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
    public class ComplaintRepository
    {
        private readonly DbminiCapstoneContext _context;

        public ComplaintRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<bool>> CreateComplaint(ComplaintModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.IdUser);

            if (user == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng",
                };
            }

            var accountTutor = await _context.Accounts.Include(x => x.Tutor).SingleOrDefaultAsync(x => x.Id == model.IdAccountTutor);

            if (accountTutor == null || accountTutor.Roles.ToLower() != "gia sư" || accountTutor.Tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư trong hệ thống",
                };
            }

            var tutorId = accountTutor.Tutor.Id;

            var complaint = new Complaint
            {
                Id = Guid.NewGuid().ToString(),
                Description = model.Description,
                IdAccount = model.IdUser,
                IdTutor = tutorId, // Sử dụng Id của Tutor
            };

            await _context.Complaints.AddAsync(complaint);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã tố cáo gia sư '{accountTutor.FullName}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = model.IdUser,
            };

            await _context.Notifications.AddAsync(nofi);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Thành công"
            };
        }

        public async Task<ApiResponse<object>> GetAllComplaint()
        {
            var complaints = await _context.Complaints.Include(x => x.IdAccountNavigation)
                                               .Include(x => x.IdTutorNavigation).ThenInclude(x => x.IdAccountNavigation)
                                               .Select(c => new
                                               {
                                                   User = new
                                                   {
                                                       Id = c.IdAccountNavigation.Id,
                                                       FullName = c.IdAccountNavigation.FullName,
                                                       Email = c.IdAccountNavigation.Email,
                                                       Date_of_birth = c.IdAccountNavigation.DateOfBirth,
                                                       Gender = c.IdAccountNavigation.Gender,
                                                       Avatar = c.IdAccountNavigation.Avatar,
                                                       Address = c.IdAccountNavigation.Address,
                                                       Phone = c.IdAccountNavigation.Phone,
                                                       Roles = c.IdAccountNavigation.Roles
                                                   },

                                                   Description = c.Description,

                                                   Tutor = new
                                                   {
                                                       Id = c.IdTutorNavigation.IdAccountNavigation.Id,
                                                       FullName = c.IdTutorNavigation.IdAccountNavigation.FullName,
                                                       Email = c.IdTutorNavigation.IdAccountNavigation.Email,
                                                       Date_of_birth = c.IdTutorNavigation.IdAccountNavigation.DateOfBirth,
                                                       Gender = c.IdTutorNavigation.IdAccountNavigation.Gender,
                                                       Avatar = c.IdTutorNavigation.IdAccountNavigation.Avatar,
                                                       Address = c.IdTutorNavigation.IdAccountNavigation.Address,
                                                       Phone = c.IdTutorNavigation.IdAccountNavigation.Phone,
                                                       Roles = c.IdTutorNavigation.IdAccountNavigation.Roles
                                                   }


                                               }).ToListAsync();


            if (!complaints.Any())
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = "Không có khiếu nại nào"
                };
            }

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = complaints
            };
        }
    }
}
