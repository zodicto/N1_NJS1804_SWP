using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public AdminController(IAdminRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet("ViewRent")]
        public async Task<IActionResult> ViewRent(string condition)
        {
            var response = await _repo.ViewRent(condition);

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
    }
}
