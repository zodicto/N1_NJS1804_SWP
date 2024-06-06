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
        public IActionResult CreateRequestLearning(string IDStudent, RequestLearningModel model)
        {

            var request = _repo.CreateRequestLearning(IDStudent, model);

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

        [HttpPut("UpdateRequest")]
        public IActionResult UpdateRequestLearning(string IDRequest, RequestLearningModel model)
        {
            var request = _repo.UpdateRequestLearning(IDRequest, model);

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

        [HttpDelete("DeleteRequest")]
        public IActionResult DeleteRequestLearning(string IDRquest)
        {
            var request = _repo.DeleteRequestLearning(IDRquest);

            if (request != null)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Delete Request Learning successfully",
                    Data = request
                });
            }

            return Ok(new ApiResponse
            {
                Success = false,
                Message = "Delete Request Learning not successfully"
            });
        }
        [HttpGet("PedingRequest")]
        public IActionResult ViewPedingRequestLearning()
        {
            var request = _repo.GetPendingApproveRequests();

            if (request != null)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Delete Request Learning successfully",
                    Data = request
                });
            }

            return Ok(new ApiResponse
            {
                Success = false,
                Message = "Delete Request Learning not successfully"
            });


        }
        [HttpGet("AppovedRequest")]
        public IActionResult ViewApprovedRequestLearning()
        {
            var request = _repo.GetApprovedRequests();

            if (request != null)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Delete Request Learning successfully",
                    Data = request
                });
            }

            return Ok(new ApiResponse
            {
                Success = false,
                Message = "Delete Request Learning not successfully"
            });
        }
    }
}
