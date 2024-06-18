using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class studentController : ControllerBase
    {
        private readonly IStudentRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public studentController(IStudentRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpPost("createRequest")]
        public async Task<IActionResult> CreateRequestLearning(string IDAccount, RequestLearningModel model)
        {
            try
            {
                var response = await _repo.CreateRequestLearning(IDAccount, model);

                if (response.Success)
                {
                    return StatusCode(200,new
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
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while creating the request learning.",
                    Details = ex.Message // Optional: Include exception details in the response
                });
            }
        }



        [HttpPut("updateRequest")]
        public async Task<IActionResult> UpdateRequestLearning(string IDRequest, RequestLearningModel model)
        {
            try
            {
                var response = await _repo.UpdateRequestLearning(IDRequest , model);

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
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while creating the request learning.",
                    Details = ex.Message // Optional: Include exception details in the response
                });
            }
        }

        [HttpDelete("deleteRequest")]
        public async Task<IActionResult> DeleteRequestLearning(string IDRquest)
        {
            var request = await _repo.DeleteRequestLearning(IDRquest);

            if (request != null)
            {
                return Ok(new 
                {
                    Success = true,
                    Message = "Delete Request Learning successfully",
                    Data = request
                });
            }

            return BadRequest(new 
            {
                Success = false,
                Message = "Delete Request Learning not successfully"
            });
        }
        [HttpGet("pedingRequest")]
        public async Task<IActionResult> ViewPedingRequestLearning(string IDAccount)
        {
            try
            {
                var response = await _repo.GetPendingRequestsByAccountId(IDAccount);

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
                    Details = ex.Message // Optional: Include exception details in the response
                });
            }
        }
        [HttpGet("appovedRequest")]
        public async Task<IActionResult> ViewApprovedRequestLearning(string IDAccount)
        {
            try
            {
                var response = await _repo.GetApprovedRequestsByAccountId(IDAccount);

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
                    Details = ex.Message // Optional: Include exception details in the response
                });
            }
        }

        [HttpGet("rejectRequest")]
        public async Task<IActionResult> ViewRejectRequestLearning(string IDAccount)
        {
            try
            {
                var response = await _repo.GetRejectRequestsByAccountId(IDAccount);

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
                    Details = ex.Message // Optional: Include exception details in the response
                });
            }
        }

        [HttpGet("getStudentProfile")]
        public async Task<IActionResult> GetStudentProfile(string id)
        {
            var result = await _repo.GetStudentProfile(id);

            if (result == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Get student profile fail"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "get student profile successfully",
                Data = result
            });
        }

        [HttpPut("updateStudentProfile")]
        public async Task<IActionResult> UpdateStudentProfile(string id, StudentProfileToUpdateModel model)
        {
            var result = await _repo.UpdateStudentProfile(id, model);

            if (!result)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Update student profile failed"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Update student profile successfully"
            });
        }

        [HttpGet("viewAllTutorsJoinRequest")]
        public async Task<IActionResult> ViewAllTutorJoinRequest(string requestId)
        {
            try
            {
                var response = await _repo.ViewAllTutorJoinRequest(requestId);

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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while processing the request.",
                    Details = ex.Message // Optional: Include exception details in the response
                });
            }
        }

    }
}
