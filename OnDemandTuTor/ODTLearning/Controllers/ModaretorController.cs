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

        [HttpGet("listTutor")]
        public async Task<IActionResult> ViewListTutorToConfirm()
        {
            var tutorList = await _repo.GetListTutorsToCofirm();

            if (tutorList == null || !tutorList.Any())
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Không có gia sư nào đang chờ duyệt"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Lấy danh sách gia sư đang chờ duyệt thành công",
                Data = tutorList
            });
        }
        [HttpGet("viewProfile")]
        public async Task<IActionResult> GetProfileToConfirm(string IdTutor)
        {
            var profile = await _repo.GetTutorProfileToConfirm(IdTutor);

            if (profile == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Không tìm thấy hồ sơ gia sư"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Lấy hồ sơ gia sư thành công",
                Data = profile
            });
        }
        [HttpPut("confirmProfile")]
        public async Task<IActionResult> ChangeStatusTutor(string id, string status)
        {
            var result = await _repo.ConfirmProfileTutor(id, status);

            if (!result)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Không thể thay đổi trạng thái của gia sư"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = status.ToLower() == "approved" ? "Trạng thái của gia sư đã được phê duyệt" : "Trạng thái của gia sư đã bị từ chối"
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
                    Message = "An error occurred while retrieving the pending requests.",
                    Data = ex.Message
                });
            }
        }

        [HttpPut("confirmRequest")]
        public async Task<IActionResult> ChangeRequestStatus(string requestId, string status)
        {
            try
            {
                var response = await _repo.ConfirmRequest(requestId, status);

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
    }
}
