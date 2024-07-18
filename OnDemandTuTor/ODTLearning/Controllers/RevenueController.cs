using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.DAL.Entities;
using ODTLearning.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;
using ODTLearning.BLL.Models;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IRevenueRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public RevenueController(IRevenueRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }                                             

        [HttpGet("ViewRevenueByYear")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewRevenueByYear(int year)
        {
            var response = await _repo.GetRevenueByYear(year);

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }

        [HttpGet("ViewRevenueThisMonth")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewRevenueThisMonth()
        {
            var response = await _repo.GetRevenueThisMonth();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }
        
        [HttpGet("ViewRevenueToday")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewRevenueToday()
        {
            var response = await _repo.GetRevenueToday();

            return Ok(new
            {
                Success = response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }
    }
}