using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.BLL.Models;
using ODTLearning.BLL.Repositories;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {

        private readonly ClassRepository _repo;

        public ClassController(ClassRepository repo)
        {
           
            _repo = repo;
         
        }

        [HttpGet("ViewClassRequest")]
        [Authorize(Roles = UserRoleAuthorize.Student + "," + UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> ViewClassRequest(string id)
        {
            var response = await _repo.GetClassRequest(id);

            if (!response.Success)
            {
                return BadRequest(new
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

        [HttpGet("ViewClassService")]
        [Authorize(Roles = UserRoleAuthorize.Student + "," + UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> GetClassService(string id)
        {
            var response = await _repo.GetClassService(id);

            if (!response.Success)
            {
                return BadRequest(new
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
        [HttpPut("completeClassRequest")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> CompleteClassRequest(string idClassRequest)
        {
            try
            {
                var response = await _repo.CompleteClassRequest(idClassRequest);

                if (response.Success)
                {
                    return StatusCode(200, new
                    {
                        Success = true,
                        response.Message
                    });
                }

                return NotFound(new
                {
                    Success = false,
                    response.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred while complete class.",
                    Data = ex.Message
                });
            }
        }

        [HttpPut("completeClassService")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> CompleteClassService(string idBooking)
        {
            try
            {
                var response = await _repo.CompleteClassService(idBooking);

                if (response.Success)
                {
                    return StatusCode(200, new
                    {
                        Success = true,
                        response.Message
                    });
                }

                return NotFound(new
                {
                    Success = false,
                    response.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred while complete class.",
                    Data = ex.Message
                });
            }
        }


    }
}
