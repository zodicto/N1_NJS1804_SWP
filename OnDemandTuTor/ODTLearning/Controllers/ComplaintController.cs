using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.BLL.Models;
using ODTLearning.BLL.Repositories;
using ODTLearning.Models;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintController : ControllerBase
    {
        private readonly ComplaintRepository _repo;

        public ComplaintController(ComplaintRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("CreateComplaint")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> CreateComplaint(ComplaintModel model)
        {
            var response = await _repo.CreateComplaint(model);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    response.Message,
                    response.Data
                });
            }

            return NotFound(new
            {
                Success = false,
                Message = response.Message
            });
        }
        [HttpGet("ViewAllComplaint")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewAllComplaint()
        {
            var response = await _repo.GetAllComplaint();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        
    }
}
