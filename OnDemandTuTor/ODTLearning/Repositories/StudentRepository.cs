using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DbminiCapstoneContext _context;
        public StudentRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        public async Task<object> CreateRequestLearning(String IdStudent, RequestLearningModel model)
        {
            // Tìm sinh viên theo IdStudent
            var student = await _context.Accounts
                                  .Include(s => s.Requests)
                                  .FirstOrDefaultAsync(s => s.Id == IdStudent);

            if (student == null)
            {
                throw new ArgumentException("Student not found");
            }
            else
            {
                // Tạo một đối tượng Request mới từ model
                var requestOfStudent = new Request
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = model.Title,
                    Price = model.Price,
                    Description = model.Description,
                    Status = model.Status,
                    LearningMethod = model.MethodLearning
                };

                // Thêm Request vào context
                await _context.Requests.AddAsync(requestOfStudent);
                await _context.SaveChangesAsync();

                // Tạo một đối tượng Schedule mới nếu có thông tin về lịch trình
                if (model.Date.HasValue && model.TimeStart.HasValue && model.TimeEnd.HasValue)
                {
                    var schedule = new Schedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = model.Date.Value,
                        TimeStart = model.TimeStart.Value,
                        TimeEnd = model.TimeEnd.Value,
                        IdService = null,
                        IdRequest = requestOfStudent.Id,
                    };

                    // Thêm Schedule vào context
                    await _context.Schedules.AddAsync(schedule);
                    await _context.SaveChangesAsync();
                }

                return requestOfStudent;
            }
        }
        public async Task<Request> UpdateRequestLearning(string requestId, RequestLearningModel model)
        {
            // Tìm request theo requestId
            var requestToUpdate = await _context.Requests
                                          .Include(r => r.Schedules)
                                          .FirstOrDefaultAsync(r => r.Id == requestId);

            if (requestToUpdate == null)
            {
                throw new ArgumentException("Request not found");
            }

            // Cập nhật các thuộc tính của request từ model
            requestToUpdate.Title = model.Title ?? requestToUpdate.Title;
            requestToUpdate.Price = model.Price ?? requestToUpdate.Price;
            requestToUpdate.Description = model.Description ?? requestToUpdate.Description;
            requestToUpdate.Status = model.Status ?? requestToUpdate.Status;
            requestToUpdate.IdSubject = model.NameService ?? requestToUpdate.IdSubject;

            // Cập nhật schedule nếu có thông tin về lịch trình
            if (model.Date.HasValue && model.TimeStart.HasValue && model.TimeEnd.HasValue)
            {
                var scheduleToUpdate = requestToUpdate.Schedules.FirstOrDefault();
                if (scheduleToUpdate != null)
                {
                    scheduleToUpdate.Date = model.Date.Value;
                    scheduleToUpdate.TimeStart = model.TimeStart.Value;
                    scheduleToUpdate.TimeEnd = model.TimeEnd.Value;
                    scheduleToUpdate.IdService = null; // Bạn cần lấy Id của service từ đâu đó
                }
                else
                {
                    // Tạo mới schedule nếu chưa tồn tại
                    var newSchedule = new Schedule
                    {
                        Id = Guid.NewGuid().ToString(),
                        Date = model.Date.Value,
                        TimeStart = model.TimeStart.Value,
                        TimeEnd = model.TimeEnd.Value,
                        IdService = null, // Bạn cần lấy Id của service từ đâu đó
                        IdRequest = requestToUpdate.Id,
                    };
                    await _context.Schedules.AddAsync(newSchedule);
                }
            }

            // Lưu các thay đổi vào context
            await _context.SaveChangesAsync();

            return requestToUpdate;
        }
        public async Task<bool> DeleteRequestLearning(string requestId)
        {
            // Tìm request theo requestId
            var requestToDelete = await _context.Requests
                                          .Include(r => r.Schedules)
                                          .FirstOrDefaultAsync(r => r.Id == requestId);

            if (requestToDelete == null)
            {
                throw new ArgumentException("Request not found");
            }

            // Xóa các schedules liên quan
            if (requestToDelete.Schedules != null)
            {
                _context.Schedules.RemoveRange(requestToDelete.Schedules);
            }

            // Xóa request
            _context.Requests.Remove(requestToDelete);
            await _context.SaveChangesAsync();

            return true; // Trả về true nếu việc xóa thành công
        }

        public async Task<List<Request>> GetPendingApproveRequests()
        {
            return _context.Requests
                           .Where(r => r.Status == "pending approve")
                           .Include(r => r.IdAccountNavigation)
                           .Include(r => r.IdLearningModelsNavigation)
                           .ToList();
        }

        // Hàm lấy tất cả các Requests có trạng thái "approved"
        public async Task<List<Request>> GetApprovedRequests()
        {
            return _context.Requests
                           .Where(r => r.Status == "approved")
                           .Include(r => r.IdAccountNavigation)
                           .Include(r => r.IdLearningModelsNavigation)
                           .ToList();
        }
    }
}


