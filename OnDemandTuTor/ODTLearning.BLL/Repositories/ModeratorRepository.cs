using Microsoft.EntityFrameworkCore;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;




namespace ODTLearning.BLL.Repositories
{
    public class ModeratorRepository : IModeratorRepository
    {

        private readonly DbminiCapstoneContext _context;

        public ModeratorRepository(DbminiCapstoneContext context)
        {
            _context = context;
        }

        


       

        public async Task<ApiResponse<bool>> DeleteRequest(string idRequest)
        {
            var request = await _context.Requests.SingleOrDefaultAsync(x => x.Id == idRequest);

            if (request == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Không tìm thấy yêu cầu"
                };
            }

            if (request.Status.ToLower() == "đang diễn ra")
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Yêu cầu đang diễn ra"
                };
            }

            var requestLearnings = await _context.RequestLearnings.Where(x => x.IdRequest == idRequest).ToListAsync();

            if (requestLearnings.Any())
            {
                _context.RequestLearnings.RemoveRange(requestLearnings);
            }

            var classRequests2 = await _context.ClassRequests.Where(x => x.IdRequest == idRequest).ToListAsync();

            if (classRequests2.Any())
            {
                _context.ClassRequests.RemoveRange(classRequests2);
            }

            _context.Requests.Remove(request);

            var nofi = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Description = $"Yêu cầu tìm gia sư '{request.Title}' của bạn đã bị xóa",
                CreateDate = DateTime.Now,
                Status = "Chưa xem",
                IdAccount = request.IdAccount,
            };

            await _context.Notifications.AddAsync(nofi);

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Xóa yêu cầu thành công"
            };
        }


    }
}
