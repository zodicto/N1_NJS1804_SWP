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
        public async Task<IActionResult> UpdateRequestLearning(string IdRequest, RequestLearningModel model)
        {
            try
            {
                var response = await _repo.UpdateRequestLearning(IdRequest , model);

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
        public async Task<IActionResult> DeleteRequestLearning(string id,string idRequest)
        {
            var request = await _repo.DeleteRequestLearning(id,idRequest);

            if (request != null)
            {
                return Ok(new 
                {
                    Success = true,
                   request.Message,
                });
            }

            return BadRequest(new 
            {
                Success = false,
                request.Message
            });
        }
        [HttpGet("pedingRequest")]
        public async Task<IActionResult> ViewPedingRequestLearning(string id)
        {
            try
            {
                var response = await _repo.GetPendingRequestsByAccountId(id);

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
        public async Task<IActionResult> ViewRejectRequestLearning(string id)
        {
            try
            {
                var response = await _repo.GetRejectRequestsByAccountId(id);

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
        public async Task<IActionResult> ViewAllTutorJoinRequest(string idRequest)
        {
            try
            {
                var response = await _repo.ViewAllTutorJoinRequest(idRequest);

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
        public async Task<IActionResult> SelectTutor(string idRequest, string idaccounttutor)
        {
            var response = await _repo.SelectTutor(idRequest, idaccounttutor);

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
        [HttpGet("classActive")]
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

        [HttpGet("classCompled")]
        public async Task<IActionResult> ViewClassCompled(string id)
        {
            try
            {
                var response = await _repo.GetClassCompled(id);

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
        [HttpPost("CreateComplaint")]
        public async Task<IActionResult> CreateComplaint(ComplaintModel model)
        {
            var response = await _repo.CreateComplaint(model);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    response.Message,
                    response.Data
                });
            }

            return NotFound(new
            {
                Success = false,
                Message = response.Message
            });
        }

        [HttpPost("CreateReview")]
        public async Task<IActionResult> CreateReview(ReviewModel model)
        {
            var response = await _repo.CreateReview(model);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return NotFound(new
            {
                Success = false,
                Message = response.Message
            });
        }

        [HttpGet("ViewSignUpTutor")]
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
        [HttpPost("BookingServiceLearning")]
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
                        Message = response.Message
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


    }
}
