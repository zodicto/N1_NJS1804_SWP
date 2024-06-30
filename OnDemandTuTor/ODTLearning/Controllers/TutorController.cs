using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tutorController : ControllerBase
    {
        private readonly ITutorRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly DbminiCapstoneContext _context;

        public tutorController(ITutorRepository repo, IConfiguration configuration, DbminiCapstoneContext context)
        {
            _repo = repo;
            _configuration = configuration;
            _context = context;
        }


        [HttpGet("GetProfileTutor")]
        public async Task<IActionResult> GetTutorProfile(string id)
        {
            var response = await _repo.GetTutorProfile(id);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message,
                    Data = response.Data
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }

        [HttpPut("UpdateTutorProfile")]
        public async Task<IActionResult> UpdateTutorProfile(string id, [FromBody] TutorProfileToUpdate model)
        {
            var response = await _repo.UpdateTutorProfile(id, model);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }

        [HttpPost("AddSubject")]
        public async Task<IActionResult> AddSubject(string id, string subjectName)
        {
            var response = await _repo.AddSubject(id, subjectName);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }

        [HttpDelete("DeleteSubject")]
        public async Task<IActionResult> DeleteSubject(string id, string subjectName)
        {
            var response = await _repo.DeleteSubject(id, subjectName);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }

        //[HttpPost("searchTutor")]
        //public async Task<IActionResult> GetTutorList(SearchTutorModel model)
        //{
        //    var result = await _repo.SearchTutorList(model);

        //    if (result == null)
        //    {
        //        return NotFound(new
        //        {
        //            Success = false,
        //            Message = "Not found"
        //        });
        //    }

        //    return Ok(new
        //    {
        //        Success = true,
        //        Message = "Get list tutor successfully",
        //        Data = result
        //    });
        //}

        [HttpGet("viewRequest")]
        public async Task<IActionResult> ViewRequest()
        {
            try
            {
                var response = await _repo.GetApprovedRequests();

                if (response.Success)
                {
                    return StatusCode(200,new
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
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the arrpoved requests.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("join-request")]
        public async Task<IActionResult> JoinRequest(string requestId, string id)
        {
            try
            {
                var response = await _repo.JoinRequest(requestId, id);

                if (response.Success)
                {
                    return StatusCode(200,
                        response.Message
                        );
                }

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred while processing the request.",
                    Data = ex.Message
                });
            }
        }
    }
}