using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly ITutorRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly DbminiCapstoneContext _context;

        public TutorController(ITutorRepository repo, IConfiguration configuration, DbminiCapstoneContext context)
        {
            _repo = repo;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("id")]
        public IActionResult GetTutorToConfirm(string id)
        {
            var list = _repo.GetTutorProfileToConFirm(id);

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
        [HttpPut("idAndStatus")]
        public IActionResult ChangeStatusTutor(string id, string status)
        {
            var list = _repo.ConFirmProfileTutor(id, status);

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
        [HttpPost("UpdateTutorProfile")]
        public IActionResult UpdateTutorProfile(string idTutor, [FromBody] TutorProfileMVModel model)
        {
            var result = _repo.UpdateTutorProfile(idTutor, model);

            if (!result)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Update tutor profile failed"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Update tutor profile successfully"
            });
        }

    }
}
