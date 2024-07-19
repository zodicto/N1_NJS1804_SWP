using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.BLL.Models;
using ODTLearning.BLL.Repositories;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                var response = await _repo.GetAllTutorJoinRequest(idRequest);

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

      
        [HttpDelete("deleteRequestByStudent")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> DeleteRequestByStudent(string id, string idRequest)
        {
            var request = await _repo.DeleteRequestByStudent(id, idRequest);

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

        [HttpDelete("deleteRequestByModerator")]
        [Authorize(Roles = UserRoleAuthorize.Moderator)]
        public async Task<IActionResult> DeleteRequestByModerator(string idRequest)
        {
            var result = await _repo.DeleteRequestByModerator(idRequest);

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

        [HttpGet("viewAllRequestPending")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewAllRequestPending()
        {
            try
            {
                var response = await _repo.GetListRequestPending();

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

        [HttpGet("viewAllRequestApproved")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewAllRequestApproved()
        {
            try
            {
                var response = await _repo.GetListRequestApproved();

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

        [HttpGet("viewAllRequestReject")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewAllRequetReject()
        {
            try
            {
                var response = await _repo.GetListRequestReject();

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
    }
}
