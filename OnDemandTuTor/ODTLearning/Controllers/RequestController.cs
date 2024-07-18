using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.BLL.Models;
using ODTLearning.BLL.Repositories;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;

namespace ODTLearning.Controllers
{
    public class RequestController : Controller
    {
        private readonly RequestRepository _repo;

        public RequestController(RequestRepository repo)
        {
            _repo = repo;
        }
        [HttpPost("createRequest")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> CreateRequestLearning(string id,[FromBody] RequestLearningModel model)
        {
            try
            {
                var response = await _repo.CreateRequestLearning(id, model);

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
                    Details = ex.Message
                });
            }
        }
        [HttpPost("selectTutor")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
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
        [HttpPost("join-request")]
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
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
        [HttpGet("getAllRequestPending")]
        [Authorize(Roles = UserRoleAuthorize.Moderator)]
        public async Task<IActionResult> ViewRequest()
        {
            try
            {
                var response = await _repo.GetAllPendingRequests();

                if (response.Success)
                {
                    return Ok(new
                    {
                        response.Success,
                        response.Message,
                        response.Data
                    });
                }

                return BadRequest(new
                {
                    Success = "Thất bại",
                    response.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the pending requests.",
                    Data = ex.Message
                });
            }
        }
        [HttpGet("getAllApprovedRequest")]
        [Authorize(Roles = UserRoleAuthorize.Tutor)]
        public async Task<IActionResult> ViewRequest(string id)
        {
            try
            {
                var response = await _repo.GetAllApprovedRequests(id);

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
                    Message = "An error occurred while retrieving the arrpoved requests.",
                    Data = ex.Message
                });
            }
        }
        [HttpGet("getAllTutorsJoinRequest")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> ViewAllTutorJoinRequest(string idRequest)
        {
            try
            {
                var response = await _repo.GetAllApprovedRequests(idRequest);

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
        [HttpGet("getPedingRequest")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
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
        [HttpGet("getAppovedRequest")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
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

        [HttpGet("getRejectRequest")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
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

        [HttpPut("updateRequest")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> UpdateRequestLearning(string idRequest, [FromBody] RequestLearningModel model)
        {
            try
            {
                var response = await _repo.UpdateRequestLearning(idRequest, model);

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
        [HttpPut("approvedRequest")]
        [Authorize(Roles = UserRoleAuthorize.Moderator)]
        public async Task<IActionResult> ApprovedRequestStatus(string idRequest)
        {
            try
            {
                var response = await _repo.ApproveRequest(idRequest);

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
                    Message = "An error occurred while changing the request status.",
                    Data = ex.Message
                });
            }
        }

        [HttpPut("rejectRequest")]
        [Authorize(Roles = UserRoleAuthorize.Moderator)]
        public async Task<IActionResult> RejectRequestStatus(string idRequest, ReasonReject model)
        {
            try
            {
                var response = await _repo.RejectRequest(idRequest, model);

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
                    Message = "An error occurred while changing the request status.",
                    Data = ex.Message
                });
            }
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
        [HttpDelete("deleteRequest")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> DeleteRequestLearning(string id, string idRequest)
        {
            var request = await _repo.DeleteRequestLearning(id, idRequest);

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
        
    }
}
