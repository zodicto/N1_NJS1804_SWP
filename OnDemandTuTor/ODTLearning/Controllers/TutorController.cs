using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using ODTLearning.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tutorController : ControllerBase
    {
        private readonly ITutorRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly DbminiCapstoneContext _context;

        public tutorController(ITutorRepository repo, IConfiguration configuration, DbminiCapstoneContext context)
        {
            _repo = repo;
            _configuration = configuration;
            _context = context;
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

        [HttpGet("viewRequest")]
        //[Authorize(Roles = "Gia sư")]
        public async Task<IActionResult> ViewRequest(string id)
        {
            try
            {
                var response = await _repo.GetApprovedRequests(id);

                if (response.Success)
                {
                    return StatusCode(200,new
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
                    Message = "An error occurred while retrieving the arrpoved requests.",
                    Data = ex.Message
                });
            }
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

        [HttpPost("createService")]
        [Authorize(Roles = "Gia sư")]
        public async Task<IActionResult> CreateServiceLearning(string id, ServiceLearningModel model)
        {
            try
            {
                var response = await _repo.CreateServiceLearning(id,model);

                if (response.Success)
                {
                    return StatusCode(200, new
                    {
                        Success = true,
                        response.Message,
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
                    Message = "An error occurred while retrieving the arrpoved requests.",
                    Data = ex.Message
                });
            }
        }
        [HttpGet("getServices")]
        public async Task<IActionResult> GetAllServicesByAccountId(string id)
        {
            try
            {
                var response = await _repo.GetAllServicesByAccountId(id);

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
                    Message = "An error occurred while retrieving the services.",
                    Data = ex.Message
                });
            }
        }
        [HttpPut("updateService")]
        [Authorize(Roles = "Gia sư")]
        public async Task<IActionResult> UpdateService(string idService, [FromBody] ServiceLearningModel model)
        {
            try
            {
                var response = await _repo.UpdateServiceById(idService, model);

                if (response.Success)
                {
                    return StatusCode(200, new
                    {
                        Success = true,
                        response.Message,
                        Data = response.Data
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
                    Message = "An error occurred while updating the service.",
                    Data = ex.Message
                });
            }
        }
        [HttpDelete("deleteService")]
        [Authorize(Roles = "Gia sư")]
        public async Task<IActionResult> DeleteService(string serviceId)
        {
            try
            {
                var response = await _repo.DeleteServiceById(serviceId);

                if (response.Success)
                {
                    return StatusCode(200, new
                    {
                        Success = true,
                        response.Message
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
                    Message = "An error occurred while deleting the service.",
                    Data = ex.Message
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

        [HttpPut("ReSignUpOftutor")]
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
    }
}