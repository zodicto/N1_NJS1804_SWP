using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class studentController : ControllerBase
    {
        private readonly IStudentRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public studentController(IStudentRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpPost("createRequest")]
        public async Task<IActionResult> CreateRequestLearning(string IDStudent, RequestLearningModel model)
        {

            var request = await _repo.CreateRequestLearning(IDStudent, model);

            if (request != null)
            {
                return Ok(new 
                {
                    Success = true,
                    Message = "Create Request Learning successfully",
                    Data = request
                });
            }

            return BadRequest(new 
            {
                Success = false,
                Message = "Create Request Learning not Create Request Learning"
            });
        }

        [HttpPut("updateRequest")]
        public async Task<IActionResult> UpdateRequestLearning(string IDRequest, RequestLearningModel model)
        {
            var request = await _repo.UpdateRequestLearning(IDRequest, model);

            if (request != null)
            {
                return Ok(new 
                {
                    Success = true,
                    Message = "Create Request Learning successfully",
                    Data = request
                });
            }

            return BadRequest(new 
            {
                Success = false,
                Message = "Create Request Learning not Create Request Learning"
            });
        }

        [HttpDelete("deleteRequest")]
        public async Task<IActionResult> DeleteRequestLearning(string IDRquest)
        {
            var request = await _repo.DeleteRequestLearning(IDRquest);

            if (request != null)
            {
                return Ok(new 
                {
                    Success = true,
                    Message = "Delete Request Learning successfully",
                    Data = request
                });
            }

            return BadRequest(new 
            {
                Success = false,
                Message = "Delete Request Learning not successfully"
            });
        }
        [HttpGet("pedingRequest")]
        public async Task<IActionResult> ViewPedingRequestLearning()
        {
            var request = await _repo.GetPendingApproveRequests();

            if (request != null)
            {
                return Ok(new 
                {
                    Success = true,
                    Message = "Delete Request Learning successfully",
                    Data = request
                });
            }

            return BadRequest(new 
            {
                Success = false,
                Message = "Delete Request Learning not successfully"
            });


        }
        [HttpGet("appovedRequest")]
        public async Task<IActionResult> ViewApprovedRequestLearning()
        {
            var request = await _repo.GetApprovedRequests();

            if (request != null)
            {
                return Ok(new 
                {
                    Success = true,
                    Message = "Delete Request Learning successfully",
                    Data = request
                });
            }

            return BadRequest(new 
            {
                Success = false,
                Message = "Delete Request Learning not successfully"
            });
        }
    }
}
