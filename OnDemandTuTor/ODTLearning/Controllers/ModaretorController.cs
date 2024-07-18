using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DiaSymReader;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using ODTLearning.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class modaretorController : ControllerBase
    {
        private readonly IModeratorRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public modaretorController(IModeratorRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }

       
        [HttpDelete("DeleteRequest")]
        [Authorize(Roles = "Kiểm duyệt viên")]
        public async Task<IActionResult> DeleteRequest(string idRequest)
        {
            var result = await _repo.DeleteRequest(idRequest);

            if (!result.Success)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = result.Message
                });
            }

            return Ok(new
            {
                Success = true,
                Message = result.Message
            });
        }
    }
}
