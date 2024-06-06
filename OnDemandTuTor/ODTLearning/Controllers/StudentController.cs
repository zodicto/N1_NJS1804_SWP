using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly DbminiCapstoneContext context;

        public StudentController(IStudentRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            this.context = context;
        }

        [HttpPost("CreateRequest")]
        public IActionResult CreateRequestLearning(string IDStudent , RequestLearningModel model)
        {

            var request = _repo.CreateRequestLearning(IDStudent,model);

            if (request != null)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Create Request Learning successfully",
                    Data = request
                });
            }

            return Ok(new ApiResponse
            {
                Success = false,
                Message = "Create Request Learning not Create Request Learning"
            });
        }

    }
}
