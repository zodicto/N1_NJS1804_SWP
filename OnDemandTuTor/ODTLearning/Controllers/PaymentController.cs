using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;

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

            var transaction = new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                Amount = model.Amount,
                CreateDate = DateTime.Now,
                Status = "Thất bại",
                IdAccount = user.Id,
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var vnpayModel = new VnPaymentRequestModel
            {
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
        public async Task<ActionResult> PaymentCallBack(string id)
        {
            var response = await _repo.PaymentExecute(Request.Query);


            if (response == null || response.VnPayResponseCode != "00")
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = $"Payment failed. Error payment VnPay: {response.VnPayResponseCode}"
                });
            }

            var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id);

            var transaction = _context.Transactions.FirstOrDefault(x => x.IdAccount == id);

            user.AccountBalance = user.AccountBalance + response.Amount;
            transaction.Status = "Thành công";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Payment successfully"
            });
        }
    }
}
