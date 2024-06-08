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

        [HttpGet("ProfileTutor")]
        public IActionResult GetTutorProfile(string id)
        {
            var result = _repo.GetTutorProfile(id);

            if (result == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Get profile tutor fail"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "get tutor profile successfully",
                Data = result
            });
        }

        [HttpPost("UpdateTutorProfile")]
        public IActionResult UpdateTutorProfile(string idTutor, [FromBody] TutorProfileToUpdate model)
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

        [HttpPost("SearchTutor")]
        public IActionResult GetTutorList(SearchTutorModel model)
        {
            var result = _repo.SearchTutorList(model);

            if (result == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Not found"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Get list tutor successfully",
                Data = result
            });
        }
    }
}
