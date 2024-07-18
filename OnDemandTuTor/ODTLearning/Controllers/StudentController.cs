using Aqua.EnumerableExtensions;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using ODTLearning.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repo;

        public StudentController(IStudentRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
        }

        [HttpGet("classActive")]
        [Authorize(Roles = "Học sinh")]
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
        [Authorize(Roles = "Học sinh")]
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
        [Authorize(Roles = "Học sinh")]
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

        [HttpPost("CreateReviewRequest")]
        [Authorize(Roles = "Học sinh")]
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
        [Authorize(Roles = "Học sinh")]
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

        






       
    }
}
