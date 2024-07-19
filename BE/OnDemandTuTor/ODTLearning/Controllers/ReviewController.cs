using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.BLL.Models;
using ODTLearning.BLL.Repositories;
using ODTLearning.Models;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewRepository _repo;

        public ReviewController(ReviewRepository repo) 
        {
            _repo = repo;
        }

        [HttpPost("CreateReviewRequest")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> CreateReviewRequest(ReviewRequestModel model)
        {
            var response = await _repo.CreateReviewRequest(model);

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

        [HttpPost("CreateReviewService")]
        [Authorize(Roles = UserRoleAuthorize.Student)]
        public async Task<IActionResult> CreateReviewService(ReviewServiceModel model)
        {
            var response = await _repo.CreateReviewService(model);

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

        [HttpGet("ViewAllReview")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewAllReview()
        {
            var response = await _repo.GetAllReview();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

    }
}
