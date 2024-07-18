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
        [Authorize]
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
        [HttpGet("GetProfileTutor")]
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

        [HttpPut("UpdateTutorProfile")]
        [Authorize(Roles = "Gia sư")]
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

        [HttpPost("AddSubject")]
        [Authorize(Roles = "Gia sư")]
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

        [HttpPost("AddQualification")]
        [Authorize(Roles = "Gia sư")]
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

        [HttpDelete("DeleteSubject")]
        [Authorize(Roles = "Gia sư")]
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
        [Authorize(Roles = "Gia sư")]
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

       

        [HttpPost("join-request")]
        [Authorize(Roles = "Gia sư")]
        public async Task<IActionResult> JoinRequest(string requestId, string id)
        {
            try
            {
                var response = await _repo.JoinRequest(requestId, id);

                if (response.Success)
                {
                    return StatusCode(200, new
                    {
                        Success = true,
                        response.Message,
                        response.Data
                    });
                }

                return BadRequest(new
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
                    Message = "An error occurred while processing the request.",
                    Data = ex.Message
                });
            }
        }
        [HttpGet("classActive")]
        [Authorize(Roles = "Gia sư")]
        public async Task<IActionResult> ViewClassActive(string id)
        {
            try
            {
                var response = await _repo.GetClassProcess(id);

                if (response.Success)
                {
                    return StatusCode(200, new
                    {
                        Success = true,
                        response.Message,
                        response.Data
                    });
                }

                return BadRequest(new
                {
                    Success = false,
                    response.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while creating the request learning.",
                    Details = ex.Message
                });
            }
        } 



        [HttpGet("GetReview")]
        public async Task<IActionResult> GetReview(string id)
        {
            try
            {
                var response = await _repo.GetReview(id);

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
                    Message = "An error occurred while get reviews.",
                    Data = ex.Message
                });
            }
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



    }
}