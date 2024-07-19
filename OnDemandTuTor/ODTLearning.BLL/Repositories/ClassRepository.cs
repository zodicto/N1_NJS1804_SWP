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
    public class ClassRepository
    {
        private readonly DbminiCapstoneContext _context;

        public ClassRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<object>> GetClassRequest(string id)
        {
            var existingUser = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);

            if (existingUser == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng"
                };
            }

            if (existingUser.Roles.ToLower() == "học sinh")
            {
                var classRequests = await _context.ClassRequests.Include(x => x.IdRequestNavigation).ThenInclude(x => x.IdClassNavigation)
                                                                .Include(x => x.IdRequestNavigation).ThenInclude(x => x.IdSubjectNavigation)
                                                                .Where(x => x.IdRequestNavigation.IdAccount == id).ToListAsync();

                if (!classRequests.Any())
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Không có lớp học nào"
                    };
                }

                var list = new List<object>();

                foreach (var classRequest in classRequests)
                {
                    var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation).FirstOrDefaultAsync(x => x.Id == classRequest.IdTutor);

                    var data = new
                    {
                        IdClassRequest = classRequest.Id,
                        Title = classRequest.IdRequestNavigation?.Title,
                        Subject = classRequest.IdRequestNavigation.IdSubjectNavigation?.SubjectName,
                        TotalSession = classRequest.IdRequestNavigation.TotalSession,
                        Price = classRequest.IdRequestNavigation.Price,
                        Description = classRequest.IdRequestNavigation.Description,
                        Class = classRequest.IdRequestNavigation.IdClassNavigation?.ClassName,
                        LearningMethod = classRequest.IdRequestNavigation.LearningMethod,
                        TimeTable = classRequest.IdRequestNavigation.TimeTable,
                        TimeStart = classRequest.IdRequestNavigation.TimeStart,
                        TimeEnd = classRequest.IdRequestNavigation.TimeEnd,
                        Status = classRequest.IdRequestNavigation.Status,

                        User = new
                        {
                            idUser = existingUser.Id,
                            Name = existingUser.FullName,
                            existingUser.Roles,
                            Email = existingUser.Email,
                            Date_of_birth = existingUser.DateOfBirth,
                            Gender = existingUser.Gender,
                            Avatar = existingUser.Avatar,
                            Address = existingUser.Address,
                            Phone = existingUser.Phone
                        },

                        Tutor = new
                        {
                            idAccountTutor = tutor.IdAccountNavigation.Id,
                            Name = tutor.IdAccountNavigation.FullName,
                            Email = tutor.IdAccountNavigation.Email,
                            tutor.IdAccountNavigation.Roles,
                            Date_of_birth = tutor.IdAccountNavigation.DateOfBirth,
                            Gender = tutor.IdAccountNavigation.Gender,
                            Avatar = tutor.IdAccountNavigation.Avatar,
                            Address = tutor.IdAccountNavigation.Address,
                            Phone = tutor.IdAccountNavigation.Phone
                        }
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

            if (existingUser.Roles.ToLower() == "gia sư")
            {
                var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation).FirstOrDefaultAsync(x => x.IdAccountNavigation.Id == id);

                var classRequests = await _context.ClassRequests.Include(x => x.IdRequestNavigation).ThenInclude(x => x.IdClassNavigation)
                                                                .Include(x => x.IdRequestNavigation).ThenInclude(x => x.IdSubjectNavigation)
                                                                .Where(x => x.IdTutor == tutor.Id).ToListAsync();

                if (!classRequests.Any())
                {
                    return new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Không có lớp học nào"
                    };
                }

                var list = new List<object>();

                foreach (var classRequest in classRequests)
                {
                    var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == classRequest.IdRequestNavigation.IdAccount);

                    var data = new
                    {
                        IdClassRequest = classRequest.Id,
                        Title = classRequest.IdRequestNavigation?.Title,
                        Subject = classRequest.IdRequestNavigation.IdSubjectNavigation?.SubjectName,
                        TotalSession = classRequest.IdRequestNavigation.TotalSession,
                        Price = classRequest.IdRequestNavigation.Price,
                        Description = classRequest.IdRequestNavigation.Description,
                        Class = classRequest.IdRequestNavigation.IdClassNavigation?.ClassName,
                        LearningMethod = classRequest.IdRequestNavigation.LearningMethod,
                        TimeTable = classRequest.IdRequestNavigation.TimeTable,
                        TimeStart = classRequest.IdRequestNavigation.TimeStart,
                        TimeEnd = classRequest.IdRequestNavigation.TimeEnd,
                        Status = classRequest.IdRequestNavigation.Status,

                        User = new
                        {

                            FullName = user.FullName,
                            Email = user.Email,
                            user.Roles,
                            Date_of_birth = user.DateOfBirth,
                            Gender = user.Gender,
                            Avatar = user.Avatar,
                            Address = user.Address,
                            Phone = user.Phone
                        },

                        Tutor = new
                        {
                            FullName = tutor.IdAccountNavigation.FullName,
                            Email = tutor.IdAccountNavigation.Email,
                            tutor.IdAccountNavigation.Roles,
                            Date_of_birth = tutor.IdAccountNavigation.DateOfBirth,
                            Gender = tutor.IdAccountNavigation.Gender,
                            Avatar = tutor.IdAccountNavigation.Avatar,
                            Address = tutor.IdAccountNavigation.Address,
                            Phone = tutor.IdAccountNavigation.Phone
                        }
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

            return new ApiResponse<object>
            {
                Success = false,
                Message = "Người dùng không phải học sinh hay gia sư"
            };
        }
        public async Task<ApiResponse<object>> GetClassService(string id)
        {
            var existingUser = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);

            if (existingUser == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng"
                };
            }

            if (existingUser.Roles.ToLower() == "học sinh")
            {
                var bookings = await _context.Bookings.Include(x => x.IdAccountNavigation)
                                                      .Include(x => x.IdTimeSlotNavigation).ThenInclude(x => x.IdDateNavigation).ThenInclude(x => x.IdServiceNavigation)
                                                                                                                                .ThenInclude(x => x.IdClassNavigation)
                                                      .Include(x => x.IdTimeSlotNavigation).ThenInclude(x => x.IdDateNavigation).ThenInclude(x => x.IdServiceNavigation)
                                                                                                                                .ThenInclude(x => x.IdSubjectNavigation)
                                                      .Where(x => x.IdAccount == id)
                                                      .ToListAsync();


                if (!bookings.Any())
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Bạn không có lớp học",
                    };
                }

                var list = new List<object>();

                foreach (var booking in bookings)
                {
                    //var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation).FirstOrDefaultAsync(x => x.Id == booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.IdTutor);

                    var tutorId = booking?.IdTimeSlotNavigation?.IdDateNavigation?.IdServiceNavigation?.IdTutor;

                    if (tutorId == null)
                    {
                        continue;
                    }

                    var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation).FirstOrDefaultAsync(x => x.Id == tutorId);

                    if (tutor == null)
                    {
                        continue;
                    }

                    var data = new
                    {
                        IdBooking = booking.Id,
                        Title = booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.Title,
                        Subject = booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.IdSubjectNavigation?.SubjectName,
                        Price = booking.Price,
                        Duration = booking.Duration,
                        Description = booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.Description,
                        Class = booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.IdClassNavigation?.ClassName,
                        LearningMethod = booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.LearningMethod,
                        Date = booking.IdTimeSlotNavigation.IdDateNavigation.Date1,
                        TimeSlot = booking.IdTimeSlotNavigation.TimeSlot1,
                        Status = booking.Status,

                        User = new
                        {
                            idUser = existingUser.Id,
                            Name = existingUser.FullName,
                            existingUser.Roles,
                            Email = existingUser.Email,
                            Date_of_birth = existingUser.DateOfBirth,
                            Gender = existingUser.Gender,
                            Avatar = existingUser.Avatar,
                            Address = existingUser.Address,
                            Phone = existingUser.Phone
                        },

                        Tutor = new
                        {
                            idAccountTutor = tutor.IdAccountNavigation.Id,
                            Name = tutor.IdAccountNavigation.FullName,
                            Email = tutor.IdAccountNavigation.Email,
                            tutor.IdAccountNavigation.Roles,
                            Date_of_birth = tutor.IdAccountNavigation.DateOfBirth,
                            Gender = tutor.IdAccountNavigation.Gender,
                            Avatar = tutor.IdAccountNavigation.Avatar,
                            Address = tutor.IdAccountNavigation.Address,
                            Phone = tutor.IdAccountNavigation.Phone
                        }
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

            if (existingUser.Roles.ToLower() == "gia sư")
            {
                var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation).FirstOrDefaultAsync(x => x.IdAccountNavigation.Id == id);

                var bookings = await _context.Bookings.Include(x => x.IdAccountNavigation)
                                                      .Include(x => x.IdTimeSlotNavigation).ThenInclude(x => x.IdDateNavigation).ThenInclude(x => x.IdServiceNavigation)
                                                                                                                                .ThenInclude(x => x.IdClassNavigation)
                                                      .Include(x => x.IdTimeSlotNavigation).ThenInclude(x => x.IdDateNavigation).ThenInclude(x => x.IdServiceNavigation)
                                                                                                                                .ThenInclude(x => x.IdSubjectNavigation)
                                                      .Where(x => x.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.IdTutor == tutor.Id)
                                                      .ToListAsync();


                if (!bookings.Any())
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Bạn không có lớp học",
                    };
                }

                var list = new List<object>();

                foreach (var booking in bookings)
                {
                    var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == booking.IdAccount);

                    var data = new
                    {
                        IdBooking = booking.Id,
                        Title = booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.Title,
                        Subject = booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.IdSubjectNavigation?.SubjectName,
                        Price = booking.Price,
                        Duration = booking.Duration,
                        Description = booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.Description,
                        Class = booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.IdClassNavigation?.ClassName,
                        LearningMethod = booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.LearningMethod,
                        Date = booking.IdTimeSlotNavigation.IdDateNavigation.Date1,
                        TimeSlot = booking.IdTimeSlotNavigation.TimeSlot1,
                        Status = booking.Status,

                        User = new
                        {
                            Name = booking.IdAccountNavigation.FullName,
                            Email = booking.IdAccountNavigation.Email,
                            booking.IdAccountNavigation.Roles,
                            Date_of_birth = booking.IdAccountNavigation.DateOfBirth,
                            Gender = booking.IdAccountNavigation.Gender,
                            Avatar = booking.IdAccountNavigation.Avatar,
                            Address = booking.IdAccountNavigation.Address,
                            Phone = booking.IdAccountNavigation.Phone
                        },

                        Tutor = new
                        {
                            Name = tutor.IdAccountNavigation.FullName,
                            Email = tutor.IdAccountNavigation.Email,
                            tutor.IdAccountNavigation.Roles,
                            Date_of_birth = tutor.IdAccountNavigation.DateOfBirth,
                            Gender = tutor.IdAccountNavigation.Gender,
                            Avatar = tutor.IdAccountNavigation.Avatar,
                            Address = tutor.IdAccountNavigation.Address,
                            Phone = tutor.IdAccountNavigation.Phone
                        }
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

            return new ApiResponse<object>
            {
                Success = false,
                Message = "Người dùng không phải học sinh hay gia sư"
            };
        }
        public async Task<ApiResponse<bool>> CompleteClassRequest(string idClassRequest)
        {
            var classRequest = await _context.ClassRequests.Include(x => x.IdRequestNavigation).SingleOrDefaultAsync(x => x.Id == idClassRequest);

            if (classRequest == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp học"
                };
            }

            classRequest.IdRequestNavigation.Status = "Hoàn thành";
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Bạn đã hoàn thành lớp học"
            };
        }

        public async Task<ApiResponse<bool>> CompleteClassService(string idBooking)
        {
            var booking = await _context.Bookings.Include(x => x.IdTimeSlotNavigation)
                                                    .ThenInclude(x => x.IdDateNavigation)
                                                        .ThenInclude(x => x.IdServiceNavigation)
                                                 .SingleOrDefaultAsync(x => x.Id == idBooking);

            if (booking == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp học"
                };
            }

            var tutor = await _context.Tutors.Include(x => x.IdAccountNavigation).FirstOrDefaultAsync(x => x.Id == booking.IdTimeSlotNavigation.IdDateNavigation.IdServiceNavigation.IdTutor);

            tutor.IdAccountNavigation.AccountBalance += (float)(booking.Price * 0.9);

            booking.Status = "Hoàn thành";
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Bạn đã hoàn thành lớp học"
            };
        }

    }
}
