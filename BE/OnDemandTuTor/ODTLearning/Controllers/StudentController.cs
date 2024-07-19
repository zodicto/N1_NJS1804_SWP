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

        
        [HttpGet("GetAllStudent")]
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

        [HttpGet("GetAmountStudent")]
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
