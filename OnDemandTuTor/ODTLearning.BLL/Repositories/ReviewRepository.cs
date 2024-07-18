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
    public class ReviewRepository
    {
        private readonly DbminiCapstoneContext _context;

        public ReviewRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<bool>> CreateReviewRequest(ReviewRequestModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.IdUser);
            Console.WriteLine("userid : " + user?.Id);
            if (user == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng",
                };
            }

            var classRequest = await _context.ClassRequests.Include(x => x.IdRequestNavigation).SingleOrDefaultAsync(x => x.Id == model.IdClassRequest);

            if (classRequest == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp học",
                };
            }

            var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation)
                                             .FirstOrDefaultAsync(x => x.Id == classRequest.IdTutor && x.IdAccountNavigation.Roles.ToLower() == "gia sư");
            Console.WriteLine("tutorid : " + tutor?.Id);
            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư",
                };
            }

            var review = new Review
            {
                Id = Guid.NewGuid().ToString(),
                Feedback = model.FeedBack,
                Rating = model.Rating,
                IdAccount = model.IdUser,
                IdTutor = tutor.Id
            };

            await _context.Reviews.AddAsync(review);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã đánh giá gia sư '{tutor.IdAccountNavigation.FullName}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = model.IdUser,
            };

            await _context.Notifications.AddAsync(nofi);

            var nofiTutor = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn nhận được 1 đánh giá thông qua lớp '{classRequest.IdRequestNavigation.Title}'",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = tutor.IdAccount,
            };

            await _context.Notifications.AddAsync(nofiTutor);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Đánh giá thành công"
            };
        }

        public async Task<ApiResponse<bool>> CreateReviewService(ReviewServiceModel model)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == model.IdUser);
            Console.WriteLine("userid : " + user?.Id);
            if (user == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng",
                };
            }

            var booking = await _context.Bookings.Include(x => x.IdTimeSlotNavigation).ThenInclude(x => x.IdDateNavigation).ThenInclude(x => x.IdServiceNavigation)
                .SingleOrDefaultAsync(x => x.Id == model.IdBooking);

            if (booking == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp học",
                };
            }

            var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation)
                                             .FirstOrDefaultAsync(x => x.Id == booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.IdTutor && x.IdAccountNavigation.Roles.ToLower() == "gia sư");
            Console.WriteLine("tutorid : " + tutor?.Id);
            if (tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư",
                };
            }

            var review = new Review
            {
                Id = Guid.NewGuid().ToString(),
                Feedback = model.FeedBack,
                Rating = model.Rating,
                IdAccount = model.IdUser,
                IdTutor = tutor.Id
            };

            await _context.Reviews.AddAsync(review);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã đánh giá gia sư '{tutor.IdAccountNavigation.FullName}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = model.IdUser,
            };

            await _context.Notifications.AddAsync(nofi);

            var nofiTutor = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn nhận được 1 đánh giá thông qua lớp '{booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.Title}'",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = tutor.IdAccount,
            };

            await _context.Notifications.AddAsync(nofiTutor);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Đánh giá thành công"
            };
        }

        public async Task<ApiResponse<object>> GetReview(string id)
        {
            var tutor = await _context.Tutors.SingleOrDefaultAsync(x => x.IdAccount == id && x.IdAccountNavigation.Roles.ToLower() == "gia sư");

            if (tutor == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy gia sư"
                };
            }

            var reviews = await _context.Reviews.Include(x => x.IdAccountNavigation)
                                               .Include(x => x.IdTutorNavigation).ThenInclude(x => x.IdAccountNavigation)
                                               .Where(x => x.IdTutorNavigation.IdAccount == id).ToListAsync();

            if (!reviews.Any())
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Gia sư chưa có đánh giá nào",
                };
            }

            var list = new List<object>();

            foreach (var review in reviews)
            {
                var data = new
                {
                    IdReview = review.Id,

                    User = new
                    {
                        Id = review.IdAccountNavigation.Id,
                        FullName = review.IdAccountNavigation.FullName,
                        Email = review.IdAccountNavigation.Email,
                        Date_of_birth = review.IdAccountNavigation.DateOfBirth,
                        Gender = review.IdAccountNavigation.Gender,
                        Avatar = review.IdAccountNavigation.Avatar,
                        Address = review.IdAccountNavigation.Address,
                        Phone = review.IdAccountNavigation.Phone
                    },

                    Rating = review.Rating,
                    Feedback = review.Feedback,
                };

                list.Add(data);
            }

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = list
            };
        }

        public async Task<ApiResponse<object>> GetAllReview()
        {
            var complaints = await _context.Reviews.Include(x => x.IdAccountNavigation)
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

                                                   c.Rating,
                                                   c.Feedback,

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
