using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class modaretorController : ControllerBase
    {
        private readonly IModeratorRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public modaretorController(IModeratorRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }
        [HttpGet("viewProfile")]
        public async Task<IActionResult> GetProfileToConFirm(string IdTutor)
        {
            var list = await _repo.GetTutorProfileToConFirm(IdTutor);

            if (list == null)
            {
                return NotFound();
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "get list tutor successfully",
                Data = list
            });
        }
        [HttpPut("confirmProfile")]
        public async Task<IActionResult> ChangeStatusTutor(string id, string status)
        {
            var list = await _repo.ConFirmProfileTutor(id, status);

            if (list == null)
            {
                return NotFound();
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "get list tutor successfully",
                Data = list
            });
        }
        [HttpPut("confirmRequest")]
        public async Task<IActionResult> ChangeStatusRequest(string id, string status)
        {
            var list = await _repo.ChangeRequestLearningStatus(id, status);

            if (list == null)
            {
                return NotFound();
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "get list tutor successfully",
                Data = list
            });
        }
    }
}
