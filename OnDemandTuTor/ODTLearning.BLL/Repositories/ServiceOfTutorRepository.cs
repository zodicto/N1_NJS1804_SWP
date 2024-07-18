using Microsoft.EntityFrameworkCore;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODTLearning.BLL.Repositories
{
    public class ServiceOfTutorRepository
    {
        private readonly DbminiCapstoneContext _context;
        public ServiceOfTutorRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<bool>> CreateServiceLearning(string id, ServiceLearningModel model)
        {
            // Tìm tài khoản theo IdAccount và vai trò "gia sư"
            var account = await _context.Accounts
                                   .Include(a => a.Tutor)
                                   .FirstOrDefaultAsync(a => a.Id == id && a.Roles.ToLower() == "gia sư");

            if (account == null || account.Tutor == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản nào với ID này hoặc bạn chưa đăng ký làm gia sư!",
                };
            }


            // Tìm lớp học theo tên
            var classEntity = await _context.Classes.FirstOrDefaultAsync(cl => cl.ClassName == model.Class);

            if (classEntity == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp nào với tên này. Vui lòng chọn lớp 10, 11, 12",
                };
            }

            // Tìm môn học theo tên
            var subjectEntity = await _context.Subjects.FirstOrDefaultAsync(sub => sub.SubjectName == model.subject);

            if (subjectEntity == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy môn học nào với tên này. Vui lòng chọn lại!",
                };
            }

            // Tạo một đối tượng Service mới
            var serviceOfTutor = new Service
            {
                Id = Guid.NewGuid().ToString(),
                Title = model.Title,
                Description = model.Description,
                LearningMethod = model.LearningMethod,
                PricePerHour = model.PricePerHour,
                IdClass = classEntity.Id,
                IdSubject = subjectEntity.Id,
                IdTutor = account.Tutor.Id // Sử dụng Id của Tutor từ account
            };

            // Thêm Service vào context
            await _context.Services.AddAsync(serviceOfTutor);

            // Thêm Date và TimeSlot vào context
            foreach (var dateModel in model.Schedule)
            {
                var dateEntity = new ODTLearning.DAL.Entities.Date
                {
                    Id = Guid.NewGuid().ToString(),
                    Date1 = DateOnly.Parse(dateModel.Date),
                    IdService = serviceOfTutor.Id
                };

                await _context.Dates.AddAsync(dateEntity);
                //await _context.SaveChangesAsync();
                if (dateModel.Date.Equals(DateTime.Today.ToString("yyyy-MM-dd")))
                {
                    foreach (var timeSlot in dateModel.TimeSlots)
                    {
                        if (DateTime.TryParseExact(timeSlot, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                        {
                            if (dateTime.TimeOfDay < DateTime.Now.TimeOfDay)
                            {
                                return new ApiResponse<bool>
                                {
                                    Success = false,
                                    Message = $"Thời gian bạn chọn không phù hợp. Vui lòng tạo lại."
                                };
                            }
                        }
                    }
                }

                foreach (var timeSlot in dateModel.TimeSlots)
                {
                    var timeSlotEntity = new TimeSlot
                    {
                        Id = Guid.NewGuid().ToString(),
                        TimeSlot1 = TimeOnly.Parse(timeSlot),
                        Status = "Chưa đặt",
                        IdDate = dateEntity.Id
                    };

                    await _context.TimeSlots.AddAsync(timeSlotEntity);
                }
            }

            await _context.SaveChangesAsync();
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã tạo dịch vụ học tập '{model.Title}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = id,
            };

            await _context.Notifications.AddAsync(nofi);

            return new ApiResponse<bool>
            {
                Success = true,
                Message = $"Tạo dịch vụ thành công."
            };
        }
        public async Task<ApiResponse<List<object>>> GetAllServicesByAccountId(string id)
        {
            // Tìm tài khoản theo IdAccount và vai trò "gia sư"
            var account = await _context.Accounts
                                  .Include(a => a.Tutor)
                                  .FirstOrDefaultAsync(a => a.Id == id && a.Roles.ToLower() == "gia sư");

            if (account == null || account.Tutor == null)
            {
                return new ApiResponse<List<object>>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản nào với ID này hoặc bạn chưa đăng ký làm gia sư!",
                };
            }

            // Tìm tất cả các dịch vụ của gia sư
            var services = await _context.Services
                                 .Where(s => s.IdTutor == account.Tutor.Id)
                                 .Include(s => s.Dates)
                                 .ThenInclude(d => d.TimeSlots)
                                 .ToListAsync();

            // Chuyển đổi các dịch vụ sang mô hình ServiceLearningModel và bao gồm ID của dịch vụ
            var serviceModels = services.Select(s => new
            {
                Id = s.Id, // Truyền ID của service
                ServiceDetails = new ServiceLearningModel
                {
                    Title = s.Title,
                    Description = s.Description,
                    PricePerHour = s.PricePerHour,
                    Class = _context.Classes.FirstOrDefault(cl => cl.Id == s.IdClass)?.ClassName,
                    subject = _context.Subjects.FirstOrDefault(sub => sub.Id == s.IdSubject)?.SubjectName,
                    LearningMethod = s.LearningMethod,
                    Schedule = s.Dates.Select(d => new ServiceDateModel
                    {
                        Date = d.Date1.ToString("yyyy-MM-dd"),
                        TimeSlots = d.TimeSlots
                             .Where(ts => ts.TimeSlot1.HasValue)
                             .Select(ts => ts.TimeSlot1.Value.ToString("HH:mm"))
                             .ToList()
                    }).ToList()
                }
            }).ToList();

            return new ApiResponse<List<object>>
            {
                Success = true,
                Message = "Lấy danh sách dịch vụ thành công",
                Data = serviceModels.Cast<object>().ToList()
            };
        }
        public async Task<ApiResponse<List<object>>> GetAllServices()
        {
            // Lấy tất cả các dịch vụ từ cơ sở dữ liệu, bao gồm các thông tin liên quan
            var services = await _context.Services
                .Include(s => s.IdClassNavigation)
                .Include(s => s.IdSubjectNavigation)
                .Include(s => s.IdTutorNavigation)
                    .ThenInclude(t => t.IdAccountNavigation)
                .Include(s => s.Dates)
                    .ThenInclude(d => d.TimeSlots)
                .ToListAsync();

            Console.WriteLine("Total services fetched: " + services.Count);

            // Kiểm tra xem có dịch vụ nào được tìm thấy hay không
            if (!services.Any())
            {
                return new ApiResponse<List<object>>
                {
                    Success = false,
                    Message = "Không có dịch vụ nào được tìm thấy",
                };
            }

            // Chuyển đổi các dịch vụ sang mô hình ServiceLearningModel và bao gồm ID của dịch vụ
            var serviceModels = services.Select(service =>
            {
                var schedule = service.Dates.Select(date =>
                {
                    var timeSlots = date.TimeSlots
                        .Where(slot => slot.TimeSlot1.HasValue && slot.Status.ToLower() == "chưa đặt") // Lọc các TimeSlot có trạng thái "Chưa đặt"
                                                                                                       //.Select(slot => slot.TimeSlot1.Value.ToString("HH:mm"))
                        .ToList();

                    Console.WriteLine("Date: " + (date.Date1.ToString("yyyy-MM-dd")) + " TimeSlots count: " + timeSlots.Count);

                    var timeSlotsAfterCheck = new List<string>();

                    foreach (var timeSlot in timeSlots)
                    {
                        var check = true;

                        var bookings = _context.Bookings.Include(x => x.IdTimeSlotNavigation).Where(x => x.IdTimeSlotNavigation.IdDate == date.Id);

                        foreach (var booking in bookings)
                        {
                            TimeOnly time = (TimeOnly)booking.IdTimeSlotNavigation.TimeSlot1;
                            var timeAfter = time.AddMinutes((double)booking.Duration);

                            if (timeSlot.TimeSlot1 >= time && timeSlot.TimeSlot1 <= timeAfter)
                            {
                                check = false;
                                break;
                            }
                        }

                        if (check)
                        {
                            timeSlotsAfterCheck.Add(timeSlot.TimeSlot1.Value.ToString("HH:mm"));
                        }
                    }

                    return new
                    {
                        Date = date.Date1.ToString("yyyy-MM-dd"), // Định dạng chuỗi cho Date
                        TimeSlots = timeSlotsAfterCheck
                    };
                })
                .Where(schedule => schedule.TimeSlots.Any()) // Chỉ lấy các Date có TimeSlot chưa được đặt
                .ToList();

                Console.WriteLine("Service ID: " + service.Id + " Schedule count: " + schedule.Count);

                return new
                {
                    IdService = service.Id, // Bao gồm Id của dịch vụ
                    IdAccountTutor = service.IdTutorNavigation.IdAccountNavigation.Id,
                    PricePerHour = service.PricePerHour,
                    Title = service.Title,
                    Description = service.Description,
                    LearningMethod = service.LearningMethod,
                    Class = service.IdClassNavigation?.ClassName,
                    Subject = service.IdSubjectNavigation?.SubjectName,
                    NameTutor = service.IdTutorNavigation.IdAccountNavigation.FullName,
                    Schedule = schedule
                };
            })
            .Where(service => service.Schedule.Any()) // Chỉ lấy các dịch vụ có lịch trình với TimeSlot chưa được đặt
            .Cast<object>()
            .ToList();

            Console.WriteLine("Filtered serviceModels count: " + serviceModels.Count);

            return new ApiResponse<List<object>>
            {
                Success = true,
                Message = "Lấy danh sách dịch vụ thành công",
                Data = serviceModels
            };
        }
        public async Task<ApiResponse<BookingServiceModel>> BookingServiceLearning(string id, string idService, BookingServiceLearingModels model)
        {
            // Tìm tài khoản theo id
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);
            if (account == null)
            {
                return new ApiResponse<BookingServiceModel>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản nào với ID này!",
                };
            }
            Console.WriteLine("Account ID: " + account.Id);

            // Tìm dịch vụ theo idService
            var service = await _context.Services
                                        .Include(s => s.IdTutorNavigation)
                                        .ThenInclude(t => t.IdAccountNavigation)
                                        .Include(s => s.Dates)
                                        .ThenInclude(d => d.TimeSlots)
                                        .FirstOrDefaultAsync(s => s.Id == idService);
            if (service == null)
            {
                return new ApiResponse<BookingServiceModel>
                {
                    Success = false,
                    Message = "Không tìm thấy dịch vụ nào với ID này!",
                };
            }
            Console.WriteLine("Service ID: " + service.Id);

            // Tìm ngày và khung giờ phù hợp
            var selectedDate = service.Dates.FirstOrDefault(d => d.Date1 == model.date);
            if (selectedDate == null)
            {
                return new ApiResponse<BookingServiceModel>
                {
                    Success = false,
                    Message = "Không tìm thấy ngày nào phù hợp với dịch vụ này!",
                };
            }

            // Tìm khung giờ dựa vào ID của date và so sánh với bảng timeslot
            foreach (var ts in selectedDate.TimeSlots)
            {
                var timeSlot24Hour = DateTime.Parse(ts.TimeSlot1.ToString()).ToString("HH:mm");
                Console.WriteLine("TimeSlot1 (24-hour): " + timeSlot24Hour);
                Console.WriteLine("model.timeAvalable: " + model.timeAvalable);

                if (timeSlot24Hour == model.timeAvalable)
                {
                    Console.WriteLine("Matched TimeSlot: " + ts.Id);

                    var selectedTimeSlot = ts;

                    //check thời gian học lấn sang timeslot khác
                    var checkTimeslots = await _context.Bookings.Include(x => x.IdTimeSlotNavigation).Where(x => x.IdTimeSlotNavigation.IdDate == selectedDate.Id && x.IdTimeSlotNavigation.TimeSlot1 > selectedTimeSlot.TimeSlot1).ToListAsync();

                    var timeStart = (TimeOnly)selectedTimeSlot.TimeSlot1;
                    var timeEnd = timeStart.AddMinutes((double)model.Duration);

                    foreach (var x in checkTimeslots)
                    {
                        if (timeEnd > x.IdTimeSlotNavigation.TimeSlot1)
                        {
                            return new ApiResponse<BookingServiceModel>
                            {
                                Success = false,
                                Message = "Thời gian học của bạn bị lấn lịch học khác của gia sư. Vui lòng điều chỉnh lại thời gian học hoặc chọn buổi học khác!",
                            };
                        }
                    }

                    // Tạo đối tượng Booking mới
                    var newBooking = new Booking
                    {
                        Id = Guid.NewGuid().ToString(),
                        IdAccount = id,
                        Duration = model.Duration,
                        Price = model.Price,
                        Status = "Đang diễn ra",
                        IdTimeSlot = selectedTimeSlot.Id
                    };

                    // Thêm Booking vào context
                    await _context.Bookings.AddAsync(newBooking);

                    // Cập nhật status của TimeSlot
                    selectedTimeSlot.Status = "Đã đặt";

                    // Trừ tiền đặt lịch từ AccountBalance của user
                    if (account.AccountBalance < model.Price)
                    {
                        return new ApiResponse<BookingServiceModel>
                        {
                            Success = false,
                            Message = "Bạn không đủ tiền trong tài khoản để đặt lịch. Vui lòng nạp thêm!",
                        };
                    }
                    account.AccountBalance -= model.Price;
                    var accountAdmin = await _context.Accounts
                        .SingleOrDefaultAsync(x => x.Roles.ToLower() == "quản trị viên");

                    

                    accountAdmin.AccountBalance += (float)0.1 * model.Price;


                    var nofi = new Notification
                    {
                        Id = Guid.NewGuid().ToString(),
                        Description = $"Bạn đã đặt dịch vụ '{service.Title}' thành công",
                        CreateDate = DateTime.Now,
                        Status = "Chưa xem",
                        IdAccount = id,
                    };

                    await _context.Notifications.AddAsync(nofi);

                    var nofiTutor = new Notification
                    {
                        Id = Guid.NewGuid().ToString(),
                        Description = $"Dịch vụ '{service.Title}' của bạn đã có 1 học sinh đặt",
                        CreateDate = DateTime.Now,
                        Status = "Chưa xem",
                        IdAccount = service.IdTutorNavigation.IdAccount,
                    };

                    await _context.Notifications.AddAsync(nofiTutor);

                    // Lưu thay đổi vào context
                    await _context.SaveChangesAsync();

                    // Chuẩn bị dữ liệu trả về
                    var data = new BookingServiceModel
                    {
                        Tutor = new
                        {
                            Name = service.IdTutorNavigation.IdAccountNavigation.FullName,
                            Email = service.IdTutorNavigation.IdAccountNavigation.Email,
                            DateOfBirth = service.IdTutorNavigation.IdAccountNavigation.DateOfBirth,
                            Gender = service.IdTutorNavigation.IdAccountNavigation.Gender,
                            Avatar = service.IdTutorNavigation.IdAccountNavigation.Avatar,
                            Address = service.IdTutorNavigation.IdAccountNavigation.Address,
                            Phone = service.IdTutorNavigation.IdAccountNavigation.Phone
                        },
                        User = new
                        {
                            Name = account.FullName,
                            Email = account.Email,
                            DateOfBirth = account.DateOfBirth,
                            Gender = account.Gender,
                            Avatar = account.Avatar,
                            Address = account.Address,
                            Phone = account.Phone
                        },
                        Service = new
                        {
                            Title = service.Title,
                            Description = service.Description,
                            PricePerHour = service.PricePerHour,
                            LearningMethod = service.LearningMethod,
                            Class = service.IdClassNavigation?.ClassName,
                            Subject = service.IdSubjectNavigation?.SubjectName
                        },
                        Booking = new
                        {
                            Date = selectedDate.Date1,
                            TimeSlot = selectedTimeSlot.TimeSlot1,
                            Duration = newBooking.Duration,
                            Price = newBooking.Price,
                            Status = newBooking.Status
                        }
                    };

                    return new ApiResponse<BookingServiceModel>
                    {
                        Success = true,
                        Message = "Đặt dịch vụ thành công .",
                        Data = data
                    };
                }
            }

            return new ApiResponse<BookingServiceModel>
            {
                Success = false,
                Message = "Không tìm thấy khung giờ nào phù hợp với dịch vụ này!",
            };
        }
        public async Task<ApiResponse<bool>> DeleteServiceById(string serviceId)
        {
            var service = await _context.Services
                .Include(s => s.IdTutorNavigation)
                .ThenInclude(s => s.IdAccountNavigation)
                                        .Include(s => s.Dates)
                                        .ThenInclude(d => d.TimeSlots)
                                        .ThenInclude(ts => ts.Bookings)
                                        .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy dịch vụ với ID này."
                };
            }

            var ongoingOrCompletedBooking = service.Dates
                                                   .SelectMany(d => d.TimeSlots)
                                                   .SelectMany(ts => ts.Bookings)
                                                   .Any(b => b.Status == "Đang diễn ra" || b.Status == "Hoàn thành");

            if (ongoingOrCompletedBooking)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không thể xóa dịch vụ vì có booking đang diễn ra hoặc đã hoàn thành."
                };
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã xóa dịch vụ học tập '{service.Title}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = service.IdTutorNavigation.IdAccountNavigation.Id,
            };

            await _context.Notifications.AddAsync(nofi);
            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Xóa dịch vụ thành công."
            };
        }
        public async Task<ApiResponse<object>> UpdateServiceById(string serviceId, ServiceLearningModel model)
        {
            // Tìm dịch vụ theo serviceId
            var service = await _context.Services
                                        .Include(s => s.IdTutorNavigation)
                                        .ThenInclude(t => t.IdAccountNavigation)
                                        .Include(s => s.Dates)
                                        .ThenInclude(d => d.TimeSlots)
                                        .FirstOrDefaultAsync(s => s.Id == serviceId);

            if (service == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy dịch vụ với ID này."
                };
            }

            var account = service.IdTutorNavigation?.IdAccountNavigation;
            if (account == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản liên kết với dịch vụ này."
                };
            }

            // Kiểm tra và định dạng ngày và time slots
            foreach (var schedule in model.Schedule ?? new List<ServiceDateModel>())
            {
                if (!DateTime.TryParseExact(schedule.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Định dạng ngày không hợp lệ. Vui lòng sử dụng định dạng yyyy-MM-dd."
                    };
                }

                foreach (var timeSlot in schedule.TimeSlots ?? new List<string>())
                {
                    if (!DateTime.TryParseExact(timeSlot, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    {
                        return new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Định dạng time slot không hợp lệ. Vui lòng sử dụng định dạng HH:mm."
                        };
                    }
                }
            }

            // Kiểm tra lớp học
            var classEntity = await _context.Classes.FirstOrDefaultAsync(cl => cl.ClassName == model.Class);
            if (classEntity == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy lớp nào với tên này. Vui lòng chọn lớp 10, 11, 12",
                };
            }

            // Kiểm tra môn học
            var subjectEntity = await _context.Subjects.FirstOrDefaultAsync(sub => sub.SubjectName == model.subject);
            if (subjectEntity == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy môn học nào với tên này. Vui lòng chọn lại!",
                };
            }

            // Cập nhật thông tin dịch vụ
            service.Title = model.Title;
            service.Description = model.Description;
            service.LearningMethod = model.LearningMethod;
            service.PricePerHour = model.PricePerHour;
            service.IdClass = classEntity.Id;
            service.IdSubject = subjectEntity.Id;
            // Cập nhật thông tin dịch vụ
            service.Title = model.Title;
            service.Description = model.Description;
            service.PricePerHour = model.PricePerHour;
            service.IdClass = classEntity.Id;
            service.IdSubject = subjectEntity.Id;

            _context.Services.Update(service);

            // Xóa các lịch trình hiện có
            var existingDates = _context.Dates.Where(d => d.IdService == serviceId).ToList();
            foreach (var date in existingDates)
            {
                var timeSlots = _context.TimeSlots.Where(ts => ts.IdDate == date.Id).ToList();
                _context.TimeSlots.RemoveRange(timeSlots);
            }
            _context.Dates.RemoveRange(existingDates);

            // Thêm Date và TimeSlot mới vào context
            foreach (var dateModel in model.Schedule)
            {
                var dateEntity = new ODTLearning.DAL.Entities.Date
                {
                    Id = Guid.NewGuid().ToString(),
                    Date1 = DateOnly.Parse(dateModel.Date),
                    IdService = service.Id
                };

                await _context.Dates.AddAsync(dateEntity);

                foreach (var timeSlot in dateModel.TimeSlots)
                {
                    var timeSlotEntity = new TimeSlot
                    {
                        Id = Guid.NewGuid().ToString(),
                        TimeSlot1 = TimeOnly.Parse(timeSlot),
                        Status = "Chưa đặt",
                        IdDate = dateEntity.Id
                    };

                    await _context.TimeSlots.AddAsync(timeSlotEntity);
                }
            }

            await _context.SaveChangesAsync();
            _context.Services.Update(service);
            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Bạn đã câp nhật dịch vụ học tập '{service.Title}' thành công",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = service.IdTutorNavigation.IdAccountNavigation.Id,
            };

            await _context.Notifications.AddAsync(nofi);
            await _context.SaveChangesAsync();

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Cập nhật dịch vụ thành công. ",
                Data = new
                {
                    Id = service.Id,
                    ServiceDetails = new ServiceLearningModel
                    {
                        PricePerHour = service.PricePerHour,
                        Title = service.Title,
                        subject = subjectEntity.SubjectName,
                        Class = classEntity.ClassName,
                        Description = service.Description,
                        LearningMethod = model.LearningMethod,
                        Schedule = service.Dates.Select(d => new ServiceDateModel
                        {
                            Date = d.Date1.ToString("yyyy-MM-dd"),
                            TimeSlots = d.TimeSlots
                                .Where(ts => ts.TimeSlot1.HasValue)
                                .Select(ts => ts.TimeSlot1.Value.ToString("HH:mm"))
                                .ToList()
                        }).ToList()
                    }
                }
            };

        }
    }
}
