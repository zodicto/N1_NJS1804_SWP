using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public class StudentRepository :IStudentRepository
    {
        private readonly DbminiCapstoneContext _context;

        public StudentRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        public object CreateRequestLearning(String IdStudent,RequestLearningModel model)
        {
            // Tìm sinh viên theo IdStudent
            var student = _context.Students
                                  .Include(s => s.Requests)
                                  .FirstOrDefault(s => s.IdStudent == IdStudent);

            if (student == null)
            {
                throw new ArgumentException("Student not found");
            }
            else
            {
                // Tạo một đối tượng Request mới từ model
                var requestOfStudent = new Request
                {
                    IdPost = Guid.NewGuid().ToString(),
                    Titile = model.Titile,
                    Price = model.Price,
                    Description = model.Description,
                    Status = model.Status,
                    IdTypeOfService = model.NameService,
                    IdStudent = IdStudent,
                };

                // Thêm Request vào context
                _context.Requests.Add(requestOfStudent);
                _context.SaveChanges();

                // Tạo một đối tượng Schedule mới nếu có thông tin về lịch trình
                if (model.Date.HasValue && model.TimeStart.HasValue && model.TimeEnd.HasValue)
                {
                    var schedule = new Schedule
                    {
                        IdSchedule = Guid.NewGuid().ToString(),
                        Date = model.Date.Value,
                        TimeStart = model.TimeStart.Value,
                        TimeEnd = model.TimeEnd.Value,
                        IdService = null,
                        IdPost = requestOfStudent.IdPost,
                    };

                    // Thêm Schedule vào context
                    _context.Schedules.Add(schedule);
                    _context.SaveChanges();
                }

                return requestOfStudent;
            }
        }
        public Request UpdateRequestLearning(string requestId, RequestLearningModel model)
        {
            // Tìm request theo requestId
            var requestToUpdate = _context.Requests
                                          .Include(r => r.Schedules)
                                          .FirstOrDefault(r => r.IdPost == requestId);

            if (requestToUpdate == null)
            {
                throw new ArgumentException("Request not found");
            }

            // Cập nhật các thuộc tính của request từ model
            requestToUpdate.Titile = model.Titile ?? requestToUpdate.Titile;
            requestToUpdate.Price = model.Price ?? requestToUpdate.Price;
            requestToUpdate.Description = model.Description ?? requestToUpdate.Description;
            requestToUpdate.Status = model.Status ?? requestToUpdate.Status;
            requestToUpdate.IdTypeOfService = model.NameService ?? requestToUpdate.IdTypeOfService;

            // Cập nhật schedule nếu có thông tin về lịch trình
            if (model.Date.HasValue && model.TimeStart.HasValue && model.TimeEnd.HasValue)
            {
                var scheduleToUpdate = requestToUpdate.Schedules.FirstOrDefault();
                if (scheduleToUpdate != null)
                {
                    scheduleToUpdate.Date = model.Date.Value;
                    scheduleToUpdate.TimeStart = model.TimeStart.Value;
                    scheduleToUpdate.TimeEnd = model.TimeEnd.Value;
                    scheduleToUpdate.IdService = "IdServicePlaceholder"; // Bạn cần lấy Id của service từ đâu đó
                }
                else
                {
                    // Tạo mới schedule nếu chưa tồn tại
                    var newSchedule = new Schedule
                    {
                        IdSchedule = Guid.NewGuid().ToString(),
                        Date = model.Date.Value,
                        TimeStart = model.TimeStart.Value,
                        TimeEnd = model.TimeEnd.Value,
                        IdService = "IdServicePlaceholder", // Bạn cần lấy Id của service từ đâu đó
                        IdPost = requestToUpdate.IdPost,
                    };
                    _context.Schedules.Add(newSchedule);
                }
            }

            // Lưu các thay đổi vào context
            _context.SaveChanges();

            return requestToUpdate;
        }
        public void DeleteRequestLearning(string requestId)
        {
            // Tìm request theo requestId
            var requestToDelete = _context.Requests
                                          .Include(r => r.Schedules)
                                          .FirstOrDefault(r => r.IdPost == requestId);

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
            _context.SaveChanges();
        }
        public List<Request> GetPendingApproveRequests()
        {
            return _context.Requests
                           .Where(r => r.Status == "pending approve")
                           .Include(r => r.IdStudentNavigation)
                           .Include(r => r.IdTypeOfServiceNavigation)
                           .ToList();
        }

        // Hàm lấy tất cả các Requests có trạng thái "approved"
        public List<Request> GetApprovedRequests()
        {
            return _context.Requests
                           .Where(r => r.Status == "approved")
                           .Include(r => r.IdStudentNavigation)
                           .Include(r => r.IdTypeOfServiceNavigation)
                           .ToList();
        }
    }
}


