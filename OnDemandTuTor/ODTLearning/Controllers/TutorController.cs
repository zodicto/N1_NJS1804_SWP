using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using ODTLearning.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;
using ODTLearning.BLL.Models;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorController : ControllerBase
    {
        private readonly TutorRepository _repo;

        public TutorController(TutorRepository repo)
        {
            _repo = repo;

        }
        [HttpPost("registerAsTutor")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> SignUpOfTutorFB(string id, [FromBody] SignUpModelOfTutor model)
        {
            try
            {
                var user = await _repo.SignUpOfTutor(id, model);

                if (user != null)
                {
                    return StatusCode(200, new
                    {
                        Success = true,
                        user.Message,
                    });
                }

                return BadRequest(new
                {
                    Success = false,
                    user.Message,
                });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Lỗi server .Vui lòng thử lại sau."
                });
            }
        }
        [HttpGet("getSignUpTutor")]
        [Authorize]
        public async Task<IActionResult> ViewSignUpTutor(string id)
        {
            var response = await _repo.GetSignUpTutor(id);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message,
                    Data = response.Data
                });
            }

            return NotFound(new
            {
                Success = false,
                Message = response.Message
            });
        }
        [HttpGet("getAllSignUpTutor")]
        [Authorize(Roles = UserRoleAuthorize.Moderator)]
        public async Task<IActionResult> ViewListTutorToConfirm()
        {
            try
            {
                var response = await _repo.GetListTutorsToConfirm();

                if (!response.Success)
                {
                    return NotFound(new
                    {
                        response.Success,
                        response.Message
                    });
                }

                return Ok(new
                {
                    response.Success,
                    response.Message,
                    response.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ViewListTutorToConfirm: {ex.Message}");

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu"
                });
            }
        }

        [HttpPut("reSignUpOfTutor")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> ReSignUpOftutor(string id, SignUpModelOfTutor model)
        {
            try
            {
                var response = await _repo.ReSignUpOftutor(id, model);

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
                    Message = "An error occurred while get register tutor.",
                    Data = ex.Message
                });
            }
        }

        [HttpPut("approveProfile")]
        [Authorize(Roles = UserRoleAuthorize.Moderator)]
        public async Task<IActionResult> ApproveProfileTutor(string id)
        {
            var result = await _repo.ApproveProfileTutor(id);

            if (!result.Success)
            {
                return NotFound(new
                {
                    Success = false,
                    result.Message
                });
            }

            return Ok(new
            {
                Success = true,
                result.Message
            });
        }

        [HttpPut("rejectProfile")]
        [Authorize(Roles = UserRoleAuthorize.Moderator)]
        public async Task<IActionResult> RejectProfileTutor(string id, ReasonReject reason)
        {
            var result = await _repo.RejectProfileTutor(id, reason);

            if (!result.Success)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = result.Message
                });
            }

            return Ok(new
            {
                Success = true,
                Message = result.Message
            });
        }

        [HttpDelete("deleteSignUpTutor")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> DeleteSignUpTutor(string id)
        {
            var response = await _repo.DeleteSignUpTutor(id);

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

        [HttpGet("GetRegisterTutor")]
        [Authorize]
        public async Task<IActionResult> GetRegisterTutor(string id)
        {
            try
            {
                var response = await _repo.GetRegisterTutor(id);

                if (response.Success)
                {
                    return StatusCode(200, new
                    {
                        Success = true,
                        response.Message,
                        response.Data
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
                    Message = "An error occurred while get register tutor.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet("ViewAmountTutor")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewAmountTutor()
        {
            var response = await _repo.GetAmountTutor();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("viewAllTutor")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewListTutor()
        {
            try
            {
                var response = await _repo.GetListTutor();

                if (!response.Success)
                {
                    return BadRequest(new
                    {
                        response.Success,
                        response.Message
                    });
                }

                return Ok(new
                {
                    response.Success,
                    response.Message,
                    response.Data
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ViewListTutorToConfirm: {ex.Message}");

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình xử lý yêu cầu"
                });
            }
        }

    }
}