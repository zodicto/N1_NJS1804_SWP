using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không có gia sư nào đang chờ duyệt"
                });
            }

            return Ok(new ApiResponse
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
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không tìm thấy hồ sơ gia sư"
                });
            }

            return Ok(new ApiResponse
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
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Không thể thay đổi trạng thái của gia sư"
                });
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = status.ToLower() == "approved" ? "Trạng thái của gia sư đã được phê duyệt" : "Trạng thái của gia sư đã bị từ chối"
            });
        }

        [HttpPut("confirmRequest")]
        public async Task<IActionResult> ChangeStatusRequest(string id, string status)
        {
            var list = await _repo.ChangeRequestLearningStatus(id, status);

            if (list == null)
            {
                return NotFound();
            }

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "get list tutor successfully",
                Data = list
            });
        }
    }
}
