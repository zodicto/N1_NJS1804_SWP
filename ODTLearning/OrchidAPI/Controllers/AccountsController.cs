using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrchidBusinessObject;
using OrchidService;

namespace OrchidAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService iAccountService;

        public AccountsController()
        {
            iAccountService = new AccountService();
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return  iAccountService.GetAccount().ToList();
        }

        // GET: api/Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(string id)
        {
            var account =  iAccountService.GetAccountById(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(string id, Account account)
        {
            if (id != account.IdAccount)
            {
                return BadRequest();
            }

            iAccountService.UpdateAccount(account);

            return NoContent();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            if (iAccountService.GetAccount() == null)
            {
                return Problem("Enttity is emply");
            }
            iAccountService.AddAccount(account);
          

            return CreatedAtAction("GetAccount", new { id = account.IdAccount }, account);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            
            if (iAccountService.GetAccount() == null)
            {
                return NotFound();
            }

            iAccountService.DeleteAccount(id);
 

            return NoContent();
        }

       
    }
}
