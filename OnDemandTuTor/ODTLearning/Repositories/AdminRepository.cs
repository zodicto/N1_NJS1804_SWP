using Firebase.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ODTLearning.Entities;
using ODTLearning.Helpers;
using ODTLearning.Models;
using System.Runtime.ConstrainedExecution;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ODTLearning.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DbminiCapstoneContext _context;

        public AdminRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        MyLibrary myLib = new MyLibrary();

        public async Task<ApiResponse<bool>> DeleteAccount(string id)
        {        
            var exsitAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id);

            if (exsitAccount == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng"
                };
            }

            var tutor = await _context.Tutors.FirstOrDefaultAsync(x => x.IdAccount == id);

            if(tutor != null)
            {
                var educationalQualifications = await _context.EducationalQualifications.Where(x => x.IdTutor == tutor.Id).ToListAsync();
                _context.EducationalQualifications.RemoveRange(educationalQualifications);


                var requestLearnings = await _context.RequestLearnings.Where(x => x.IdTutor == tutor.Id).ToListAsync();
                _context.RequestLearnings.RemoveRange(requestLearnings);

                var tutorSubjects = await _context.TutorSubjects.Where(x => x.IdTutor == tutor.Id).ToListAsync();
                _context.TutorSubjects.RemoveRange(tutorSubjects);

                var complaints = await _context.Complaints.Where(x => x.IdTutor == tutor.Id).ToListAsync();
                _context.Complaints.RemoveRange(complaints);   

                _context.Tutors.Remove(tutor);
            }

            var complaints2 = await _context.Complaints.Where(x => x.IdAccount == id).ToListAsync();
            _context.Complaints.RemoveRange(complaints2);

            var transactions = await _context.Transactions.Where(x => x.IdAccount == id).ToListAsync();
            _context.Transactions.RemoveRange(transactions);

            var requests = await _context.Requests.Where(x => x.IdAccount == id).ToListAsync();
            _context.Requests.RemoveRange(requests);

            _context.Accounts.Remove(exsitAccount);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Xóa người dùng thành công"
            };                      
        }

        public async Task<ApiResponse<List<ListAccount>>> GetListStudent()
        {
            try
            {
                var ListStudent = await _context.Accounts
                   .Where(t => t.Roles == "Học sinh")
                    .Select(t => new ListAccount
                    {
                        id = t.Id, // Sử dụng Id của Tutor
                        email = t.Email,
                        date_of_birth = t.DateOfBirth,
                        fullName = t.FullName,
                        gender = t.Gender,
                        phone = t.Phone,
                        roles = t.Roles,
                    }).ToListAsync();
                return new ApiResponse<List<ListAccount>>
                {
                    Success = true,
                    Message = "Lấy danh sách học sinh thành công",
                    Data = ListStudent
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListTutorsToConfirm: {ex.Message}");

                return new ApiResponse<List<ListAccount>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách gia sư",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<ListAccount>>> GetListTutor()
        {
            try
            {
                var ListStudent = await _context.Accounts
                   .Where(t => t.Roles == "Gia sư")
                    .Select(t => new ListAccount
                    {
                        id = t.Id, // Sử dụng Id của Tutor
                        email = t.Email,
                        date_of_birth = t.DateOfBirth,
                        fullName = t.FullName,
                        gender = t.Gender,
                        phone = t.Phone,
                        roles = t.Roles,
                    }).ToListAsync();
                return new ApiResponse<List<ListAccount>>
                {
                    Success = true,
                    Message = "Lấy danh sách gia sư thành công",
                    Data = ListStudent
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListTutorsToConfirm: {ex.Message}");

                return new ApiResponse<List<ListAccount>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách học sinh",
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestPending()
        {
            try
            {
                var ListRequestPending = await _context.Requests
                    .Include(t => t.IdAccountNavigation)
                    .Include(t => t.IdClassNavigation)
                    .Include(t => t.IdSubjectNavigation)
                    .Where(t => t.Status == "Chưa duyệt")
                    .Select(t => new ViewRequestOfStudent
                    {
                        IdRequest = t.Id,
                        Title = t.Title,
                        Price = t.Price,
                        Class = t.IdClassNavigation.ClassName,
                        TimeStart = t.TimeStart.HasValue ? t.TimeStart.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeEnd = t.TimeEnd.HasValue ? t.TimeEnd.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeTable = t.TimeTable,
                        TotalSession = t.TotalSession,
                        Subject = t.IdSubjectNavigation.SubjectName,
                        FullName = t.IdAccountNavigation.FullName,
                        Description = t.Description,
                        Status = t.Status,
                        LearningMethod = t.LearningMethod,
                    }).ToListAsync();
                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = true,
                    Message = "Lấy danh sách yêu cầu chưa duyệt thành công",
                    Data = ListRequestPending
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListRequestPending: {ex.Message}");

                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách yêu cầu",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestApproved()
        {
            try
            {
                var ListRequestPending = await _context.Requests
                    .Include(t => t.IdAccountNavigation)
                    .Include(t => t.IdClassNavigation)
                    .Include(t => t.IdSubjectNavigation)
                    .Where(t => t.Status == "Đã duyệt")
                    .Select(t => new ViewRequestOfStudent
                    {
                        IdRequest = t.Id,
                        Title = t.Title,
                        Price = t.Price,
                        Class = t.IdClassNavigation.ClassName,
                        TimeStart = t.TimeStart.HasValue ? t.TimeStart.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeEnd = t.TimeEnd.HasValue ? t.TimeEnd.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeTable = t.TimeTable,
                        TotalSession = t.TotalSession,
                        Subject = t.IdSubjectNavigation.SubjectName,
                        FullName = t.IdAccountNavigation.FullName,
                        Description = t.Description,
                        Status = t.Status,
                        LearningMethod = t.LearningMethod,
                    }).ToListAsync();
                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = true,
                    Message = "Lấy danh sách đã duyệt thành công",
                    Data = ListRequestPending
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListRequestPending: {ex.Message}");

                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách yêu cầu",
                    Data = null
                };
            }
        }
        public async Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestReject()
        {
            try
            {
                var ListRequestPending = await _context.Requests
                    .Include(t => t.IdAccountNavigation)
                    .Include(t => t.IdClassNavigation)
                    .Include(t => t.IdSubjectNavigation)
                    .Where(t => t.Status == "Từ chối")
                    .Select(t => new ViewRequestOfStudent
                    {
                        IdRequest = t.Id,
                        Title = t.Title,
                        Price = t.Price,
                        Class = t.IdClassNavigation.ClassName,
                        TimeStart = t.TimeStart.HasValue ? t.TimeStart.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                       TimeEnd= t.TimeEnd.HasValue ? t.TimeEnd.Value.ToString("HH:mm") : null, // Convert TimeOnly? to string
                        TimeTable = t.TimeTable,
                        TotalSession = t.TotalSession,
                        Subject = t.IdSubjectNavigation.SubjectName,
                        FullName = t.IdAccountNavigation.FullName,
                        Description = t.Description,
                        Status = t.Status,
                        LearningMethod = t.LearningMethod,
                    }).ToListAsync();
                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = true,
                    Message = "Lấy danh sách yêu cầu bị từ chối thành công",
                    Data = ListRequestPending
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu cần thiết
                Console.WriteLine($"Error in GetListRequestPending: {ex.Message}");

                return new ApiResponse<List<ViewRequestOfStudent>>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lấy danh sách yêu cầu",
                    Data = null
                };
            }
        }
        //public async Task<ApiResponse<object>> ViewRent(string Condition)
        //{
        //    var rent = _context.Rents.AsQueryable();

        //    DateTime? date = null;

        //    if (Condition.ToLower() == "1 tuần")
        //    {
        //        date = DateTime.Now.AddDays(-7);
        //    }
        //    if (Condition.ToLower() == "1 tháng")
        //    {
        //        date = DateTime.Now.AddMonths(-1);
        //    }

        //    if (date != null)
        //    {
        //        rent = rent.Where(x => x.CreateDate >= date);
        //    }

        //    var data = rent.Include(x => x.IdRequestNavigation).ThenInclude(x => x.IdSubjectNavigation)
        //                   .Include(x => x.IdRequestNavigation).ThenInclude(x => x.IdAccountNavigation)
        //                   .Join(_context.Accounts, r => r.IdTutor, a => a.Id, (r, a) => new
        //                   {
        //                       User = new
        //                       {
        //                           Name = r.IdRequestNavigation.IdAccountNavigation.FullName,
        //                           Email = r.IdRequestNavigation.IdAccountNavigation.Email,
        //                           DateOfBirth = r.IdRequestNavigation.IdAccountNavigation.DateOfBirth,
        //                           Gender = r.IdRequestNavigation.IdAccountNavigation.Gender,
        //                           Avatar = r.IdRequestNavigation.IdAccountNavigation.Avatar,
        //                           Address = r.IdRequestNavigation.IdAccountNavigation.Address,
        //                           Phone = r.IdRequestNavigation.IdAccountNavigation.Phone
        //                       },
        //                       Subject = r.IdRequestNavigation.IdSubjectNavigation.SubjectName,
        //                       Price = r.Price,
        //                       CreateDate = r.CreateDate,
        //                       Tutor = new
        //                       {
        //                           Name = a.FullName,
        //                           Email = a.Email,
        //                           DateOfBirth = a.DateOfBirth,
        //                           Gender = a.Gender,
        //                           Avatar = a.Avatar,
        //                           Address = a.Address,
        //                           Phone = a.Phone
        //                       },
        //                   }).ToList();

        //    return new ApiResponse<object>
        //    {
        //        Success = true,
        //        Message = "Thành công",
        //        Data = data
        //    };
        //}

        public async Task<ApiResponse<ComplaintResponse>> GetAllComplaint()
        {
            var complaint = _context.Complaints.Include(x => x.IdAccountNavigation)
                                               .Include(x => x.IdTutorNavigation).ThenInclude(x => x.IdAccountNavigation)
                                               .ToList();

            if (!complaint.Any())
            {
                return new ApiResponse<ComplaintResponse>
                {
                    Success = true,
                    Message = "Không có khiếu nại nào"
                };
            }

            var user = new InfoUserModel
            {
                Id = "",
                FullName = "",
                Email = "",
                date_of_birth = "",
                Gender = "",
                Avatar = "",
                Address = "",
                Phone = "",
                Roles = ""
            };

            var description = "";

            var tutor = new InfoUserModel
            {
                Id = "",
                FullName = "",
                Email = "",
                date_of_birth = "",
                Gender = "",
                Avatar = "",
                Address = "",
                Phone = "",
                Roles = ""
            };

            foreach (var c in complaint)
            {
                user.Id += c.IdAccountNavigation.Id + ";";
                user.FullName += c.IdAccountNavigation.FullName + ";";
                user.Email += c.IdAccountNavigation.Email + ";";
                user.date_of_birth += c.IdAccountNavigation.DateOfBirth + ";";
                user.Gender += c.IdAccountNavigation.Gender + ";";
                user.Avatar += c.IdAccountNavigation.Avatar + ";";
                user.Address += c.IdAccountNavigation.Address + ";";
                user.Phone += c.IdAccountNavigation.Phone + ";";
                user.Roles += c.IdAccountNavigation.Roles + ";";

                description += c.Description + ";";

                tutor.Id += c.IdTutorNavigation.IdAccountNavigation.Id + ";";
                tutor.FullName += c.IdTutorNavigation.IdAccountNavigation.FullName + ";";
                tutor.Email += c.IdTutorNavigation.IdAccountNavigation.Email + ";";
                tutor.date_of_birth += c.IdTutorNavigation.IdAccountNavigation.DateOfBirth + ";";
                tutor.Gender += c.IdTutorNavigation.IdAccountNavigation.Gender + ";";
                tutor.Avatar += c.IdTutorNavigation.IdAccountNavigation.Avatar + ";";
                tutor.Address += c.IdTutorNavigation.IdAccountNavigation.Address + ";";
                tutor.Phone += c.IdTutorNavigation.IdAccountNavigation.Phone + ";";
                tutor.Roles += c.IdTutorNavigation.IdAccountNavigation.Roles + ";";
            }

            user.Id = myLib.DeleteLastIndexString(user.Id);
            user.FullName = myLib.DeleteLastIndexString(user.FullName);
            user.Email = myLib.DeleteLastIndexString(user.Email);
            user.date_of_birth = myLib.DeleteLastIndexString(user.date_of_birth);
            user.Gender = myLib.DeleteLastIndexString(user.Gender);
            user.Avatar = myLib.DeleteLastIndexString(user.Avatar);
            user.Address = myLib.DeleteLastIndexString(user.Address);
            user.Phone = myLib.DeleteLastIndexString(user.Phone);
            user.Roles = myLib.DeleteLastIndexString(user.Roles);

            description = myLib.DeleteLastIndexString(description);

            tutor.Id = myLib.DeleteLastIndexString(tutor.Id);
            tutor.FullName = myLib.DeleteLastIndexString(tutor.FullName);
            tutor.Email = myLib.DeleteLastIndexString(tutor.Email);
            tutor.date_of_birth = myLib.DeleteLastIndexString(tutor.date_of_birth);
            tutor.Gender = myLib.DeleteLastIndexString(tutor.Gender);
            tutor.Avatar = myLib.DeleteLastIndexString(tutor.Avatar);
            tutor.Address = myLib.DeleteLastIndexString(tutor.Address);
            tutor.Phone = myLib.DeleteLastIndexString(tutor.Phone);
            tutor.Roles = myLib.DeleteLastIndexString(tutor.Roles);

            var data = new ComplaintResponse
            {
                User = user,

                Description = description,

                Tutor = tutor
            };

            return new ApiResponse<ComplaintResponse>
            {
                Success = true,
                Message = "Thành công",
                Data = data
            };
        }

        public async Task<ApiResponse<TransactionResponse>> GetAllTransaction()
        {
            var transaction = _context.Transactions.Include(x => x.IdAccountNavigation).ToList();

            if (!transaction.Any())
            {
                return new ApiResponse<TransactionResponse>
                {
                    Success = true,
                    Message = "Không có giao dịch nào"
                };
            }

            var id = "";
            var amount = "";
            var createDate = "";
            var status = "";


            var user = new InfoUserModel
            {
                Id = "",
                FullName = "",
                Email = "",
                date_of_birth = "",
                Gender = "",
                Avatar = "",
                Address = "",
                Phone = "",
                Roles = ""
            };

            foreach (var c in transaction)
            {
                id += c.Id + ";";
                amount += c.Amount + ";";
                createDate += c.CreateDate + ";";
                status += c.Status + ";";

                user.Id += c.IdAccountNavigation.Id + ";";
                user.FullName += c.IdAccountNavigation.FullName + ";";
                user.Email += c.IdAccountNavigation.Email + ";";
                user.date_of_birth += c.IdAccountNavigation.DateOfBirth + ";";
                user.Gender += c.IdAccountNavigation.Gender + ";";
                user.Avatar += c.IdAccountNavigation.Avatar + ";";
                user.Address += c.IdAccountNavigation.Address + ";";
                user.Phone += c.IdAccountNavigation.Phone + ";";
                user.Roles += c.IdAccountNavigation.Roles + ";";
            }

            id = myLib.DeleteLastIndexString(id);
            amount = myLib.DeleteLastIndexString(amount);
            createDate = myLib.DeleteLastIndexString(createDate);
            status = myLib.DeleteLastIndexString(status);

            user.Id = myLib.DeleteLastIndexString(user.Id);
            user.FullName = myLib.DeleteLastIndexString(user.FullName);
            user.Email = myLib.DeleteLastIndexString(user.Email);
            user.date_of_birth = myLib.DeleteLastIndexString(user.date_of_birth);
            user.Gender = myLib.DeleteLastIndexString(user.Gender);
            user.Avatar = myLib.DeleteLastIndexString(user.Avatar);
            user.Address = myLib.DeleteLastIndexString(user.Address);
            user.Phone = myLib.DeleteLastIndexString(user.Phone);
            user.Roles = myLib.DeleteLastIndexString(user.Roles);

            var data = new TransactionResponse
            {
                Id = id,
                Amount = amount,
                CreateDate = createDate,
                Status = status,                
                User = user
            };

            return new ApiResponse<TransactionResponse>
            {
                Success = true,
                Message = "Thành công",
                Data = data
            };
        }

        public async Task<ApiResponse<object>> GetRevenueByMonth(int year)
        {
            var rents = await _context.Rents.Where(x => x.CreateDate.Year == year).GroupBy(x => x.CreateDate.Month).Select(x => new
            {                
                Name = $"Tháng {x.Key}" ,
                Data = x.Sum(r => r.Price)
            }).ToListAsync();

            if (!rents.Any())
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = $"Không có việc thuê nào trong năm {year}"
                };
            }

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công",
                Data = rents
            };
        }

        public async Task<ApiResponse<object>> GetRevenueByWeek(int month, int year)
        {
            var rents = _context.Rents.Where(x => x.CreateDate.Month == month && x.CreateDate.Year == year);

            if (!rents.Any())
            {
                return new ApiResponse<object>
                {
                    Success = true,
                    Message = $"Không có việc thuê nào trong {month}/{year}"
                };
            }

            var data = new List<object>();
            float price1 = 0;
            float price2 = 0;
            float price3 = 0;
            float price4 = 0;

            foreach (var x in rents)
            {
                if (x.CreateDate.Day <= 7)
                {
                    price1 += (float) x.Price;
                }
                else if (x.CreateDate.Day > 7 && x.CreateDate.Day <= 14)
                {
                    price2 += (float) x.Price;
                }
                else if (x.CreateDate.Day > 14 && x.CreateDate.Day <= 21)
                {
                    price3 += (float) x.Price;
                }
                else 
                {
                    price4 += (float) x.Price;
                }
            }

            var week1 = new
            {
                Name = "Tuần 1",
                Data = price1
            };

            var week2 = new
            {
                Name = "Tuần 2",
                Data = price2
            };

            var week3 = new
            {
                Name = "Tuần 3",
                Data = price3
            };

            var week4 = new
            {
                Name = "Tuần 4",
                Data = price4
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
    }
}
