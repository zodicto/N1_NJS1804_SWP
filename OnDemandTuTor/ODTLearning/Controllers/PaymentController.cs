using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using ODTLearning.BLL.Repositories;
using Microsoft.AspNetCore.Authorization;


namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class paymentController : ControllerBase
    {
        private readonly IVnPayRepository _repo;
        private readonly DbminiCapstoneContext _context;

        public paymentController(IVnPayRepository repo, DbminiCapstoneContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpPost("payment")]
        [Authorize]
        public async Task<ActionResult> Payment(DepositModel model)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.Id == model.Id);

            if (user == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Not found user"
                });
            }

            var vnpayModel = new VnPaymentRequestModel
            {
                IdAccount = model.Id,
                FullName = user.FullName,
                Amount = model.Amount,
                CreatedDate = DateTime.Now
            };

            return Ok(new
            {
                Success = true,
                Message = "Redirect url in data",
                Data = await _repo.CreatePaymentUrl(HttpContext, vnpayModel)
            });

        }
        [HttpGet("paymentCallBack")]
        public async Task<ActionResult> PaymentCallBack()
        {
            var response = await _repo.PaymentExecute(Request.Query);

            var transaction = new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                Amount = response.Amount,
                CreateDate = DateTime.Now,
                IdAccount = response.IdAccount
            };

            if (response == null || response.VnPayResponseCode != "00")
            {
                transaction.Status = "Thất bại";

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();

                return Redirect("http://localhost:3000/paymentFail");
            }

            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == response.IdAccount);
            
            user.AccountBalance = user.AccountBalance + response.Amount;
            transaction.Status = "Thành công";

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return Redirect("http://localhost:3000/paymentSucsess");
        }
    }
}
