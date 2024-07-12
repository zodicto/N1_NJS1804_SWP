using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DiaSymReader;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class modaretorController : ControllerBase
    {
        private readonly IModeratorRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public modaretorController(IModeratorRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet("viewListTutor")]
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

        [HttpPut("approveProfile")]
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
        public async Task<IActionResult> RejectProfileTutor(string id, ReasonReject reason)
        {
            var result = await _repo.RejectProfileTutor(id,reason);

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

        [HttpGet("viewRequest")]
        public async Task<IActionResult> ViewRequest()
        {
            try
            {
                var response = await _repo.GetPendingRequests();

                if (response.Success)
                {
                    return Ok(new
                    {
                        Success = "Thành công",
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

        [HttpPut("approvedRequest")]
        public async Task<IActionResult> ApprovedRequestStatus(string requestId)
        {
            try
            {
                var response = await _repo.ApproveRequest(requestId);

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
        public async Task<IActionResult> RejectRequestStatus(string requestId)
        {
            try
            {
                var response = await _repo.RejectRequest(requestId);

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

        [HttpDelete("DeleteRequest")]
        public async Task<IActionResult> DeleteRequest(string idRequest)
        {
            var result = await _repo.DeleteRequest(idRequest);

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
    }
}
