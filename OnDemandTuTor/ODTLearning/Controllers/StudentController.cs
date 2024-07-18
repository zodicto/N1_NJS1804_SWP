using Aqua.EnumerableExtensions;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using ODTLearning.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;
using ODTLearning.BLL.Models;

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

        [HttpGet("viewAllStudent")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewListStudent()
        {
            try
            {
                var response = await _repo.GetListStudent();

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

        [HttpGet("ViewAmountStudent")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewAmountStudent()
        {
            var response = await _repo.GetAmountStudent();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

    }
}
