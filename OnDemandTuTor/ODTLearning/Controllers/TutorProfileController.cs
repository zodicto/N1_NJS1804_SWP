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
    public class TutorProfileController : ControllerBase
    {
        private readonly TutorProfileRepository _repo;
        public TutorProfileController(TutorProfileRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("addSubject")]
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> AddSubject(string id, string subjectName)
        {
            var response = await _repo.AddSubject(id, subjectName);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }

        [HttpPost("addQualification")]
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> AddQualification(string id, AddQualificationModel model)
        {
            var response = await _repo.AddQualification(id, model);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }

        [HttpGet("getProfileTutor")]
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> GetTutorProfile(string id)
        {
            var response = await _repo.GetTutorProfile(id);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message,
                    Data = response.Data
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }

        [HttpPut("updateTutorProfile")]
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> UpdateTutorProfile(string id, [FromBody] TutorProfileToUpdate model)
        {
            var response = await _repo.UpdateTutorProfile(id, model);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }


        [HttpDelete("DeleteSubject")]
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> DeleteSubject(string id, string subjectName)
        {
            var response = await _repo.DeleteSubject(id, subjectName);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }

        [HttpDelete("DeleteQualification")]
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> DeleteQualification(string id, string idQualification)
        {
            var response = await _repo.DeleteQualification(id, idQualification);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }
    }
}
