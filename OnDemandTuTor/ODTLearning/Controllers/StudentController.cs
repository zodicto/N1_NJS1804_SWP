using Aqua.EnumerableExtensions;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public StudentController(IStudentRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpPost("createRequest")]
        public async Task<IActionResult> CreateRequestLearning(string id, RequestLearningModel model)
        {
            try
            {
                var response = await _repo.CreateRequestLearning(id, model);

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
                    Details = ex.Message 
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
        public async Task<IActionResult> ViewApprovedRequestLearning(string id)
        {
            try
            {
                var response = await _repo.GetApprovedRequestsByAccountId(id);

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

        [HttpPost("SelectTutor")]
        public async Task<IActionResult> SelectTutor(string idrequest, string idaccounttutor)
        {
            var response = await _repo.SelectTutor(idrequest, idaccounttutor);

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

        [HttpPost("CreateComplaint")]
        public async Task<IActionResult> CreateComplaint(ComplaintModel model)
        {
            var response = await _repo.CreateComplaint(model);

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

    }
}
