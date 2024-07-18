using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTLearning.BLL.Models;
using ODTLearning.BLL.Repositories;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : Controller
    {
        private readonly ServiceOfTutorRepository _repo;
        public ServiceController(ServiceOfTutorRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("createService")]
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> CreateServiceLearning(string id,[FromBody] ServiceLearningModel model)
        {
            try
            {
                var response = await _repo.CreateServiceLearning(id, model);

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

        [HttpPost("bookingService")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> BookingServiceLearning(string id, string idService, [FromBody] BookingServiceLearingModels model)
        {
            try
            {
                var response = await _repo.BookingServiceLearning(id, idService, model);

                if (response.Success)
                {
                    return Ok(new
                    {
                        Success = true,
                        Message = response.Message,
                        response.Data


                    });
                }

                return BadRequest(new
                {
                    Success = false,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while booking the service.",
                    Details = ex.Message
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
        [HttpGet("getAllService")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> GetAllService()
        {
            try
            {
                var response = await _repo.GetAllServices();

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
        [HttpPut("updateService")]
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
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
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> DeleteService(string idService)
        {
            try
            {
                var response = await _repo.DeleteServiceById(idService);

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

    }
}
