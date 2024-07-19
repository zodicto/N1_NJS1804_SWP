using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.BLL.Repositories;
using ODTLearning.DAL.Entities;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationRepository _repo;

        public NotificationController(NotificationRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("ViewAllNotification")]
        [Authorize]
        public async Task<IActionResult> GetAllNotification(string id)
        {
            var response = await _repo.GetAllNotification(id);

            if (!response.Success)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = response.Message
                });
            }

            return Ok(new
            {
                Success = true,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpPut("UpdateStatusNotification")]
        [Authorize]
        public async Task<IActionResult> UpdateStatusNotification(string id)
        {
            var response = await _repo.UpdateStatusNotification(id);

            if (!response.Success)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = response.Message
                });
            }

            return Ok(new
            {
                Success = true,
                Message = response.Message
            });
        }
    }
}
