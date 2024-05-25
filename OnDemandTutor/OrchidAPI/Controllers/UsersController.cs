using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrchidBusinessObjects;
using OrchidServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrchidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IOrchidServies iOrchidService;

        public UsersController(IOrchidServies orchidService)
        {
            iOrchidService = orchidService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await iOrchidService.GetAllUsers().ToListAsync();
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user =  iOrchidService.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                 iOrchidService.UpdateUser(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            iOrchidService.AddStudent(user);
            try
            {
                await iOrchidService.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user =  iOrchidService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            await iOrchidService.DeleteUser(id);

            return NoContent();
        }

        private bool UserExists(string id)
        {
            // Cần thay đổi kiểm tra UserExists tại đây dựa vào cách triển khai của IOrchidServies
            return iOrchidService.GetStudent(id) != null;
        }
    }
}
