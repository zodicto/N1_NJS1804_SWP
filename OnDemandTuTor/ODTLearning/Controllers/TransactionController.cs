using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTLearning.BLL.Models;
using ODTLearning.BLL.Repositories;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionRepository _repo;

        public TransactionController(TransactionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("ViewAllTransaction")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> ViewAllTransaction()
        {
            var response = await _repo.GetAllTransaction();

            return Ok(new
            {
                response.Success,
                Message = response.Message,
                Data = response.Data
            });
        }
    }
}
