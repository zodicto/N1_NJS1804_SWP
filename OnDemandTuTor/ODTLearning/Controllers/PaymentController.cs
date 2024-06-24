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
            var vnpayModel = new VnPaymentRequestModel
            {
                FullName = model.FullName,
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

            user.AccountBalance = user.AccountBalance + response.Amount;
            await _context.SaveChangesAsync();

            return Ok(new 
            {
                Success = true,
                Message = "Payment successfully"
            });
        }
    }
}
