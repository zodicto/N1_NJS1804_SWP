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
        public async Task<IActionResult> CreateRequestLearning(string IDAccount, RequestLearningModel model)
        {
            try
            {
                var response = await _repo.CreateRequestLearning(IDAccount, model);

                if (response.Success)
                {
                    return StatusCode(200,new
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
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An error occurred while creating the request learning.",
                    Details = ex.Message // Optional: Include exception details in the response
                });
            }
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
                    Message = "Get Pending Request Learning successfully",
                    Data = request
                });
            }

            return BadRequest(new 
            {
                Success = false,
                Message = "Errol"
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
                    Message = "Delete approve request Learning successfully",
                    Data = request
                });
            }

            return BadRequest(new 
            {
                Success = false,
                Message = "Errol"
            });
        }

        [HttpGet("getStudentProfile")]
        public async Task<IActionResult> GetStudentProfile(string id)
        {
            var result = await _repo.GetStudentProfile(id);

            if (result == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Get student profile fail"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "get student profile successfully",
                Data = result
            });
        }

        [HttpPut("updateStudentProfile")]
        public async Task<IActionResult> UpdateStudentProfile(string id, StudentProfileToUpdateModel model)
        {
            var result = await _repo.UpdateStudentProfile(id, model);

            if (!result)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Update student profile failed"
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Update student profile successfully"
            });
        }
    }
}
