using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Payment(DepositModel model)
        {
            var vnpayModel = new VnPaymentRequestModel
            {
                OrderId = new Random().Next(1000, 100000),
                FullName = model.LastName + " " + model.FirstName,
                Description = "asdjef",
                Amount = model.Amount,
                CreatedDate = DateTime.Now
            };

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Redirect url in data",
                Data = _repo.CreatePaymentUrl(HttpContext, vnpayModel)
            });
        }
        [HttpGet("paymentCallBack")]
        public ActionResult PaymentCallBack(string id)
        {
            var response = _repo.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = $"Payment failed. Error payment VnPay: {response.VnPayResponseCode}"
                });
            }

            var user = _context.Accounts.FirstOrDefault(x => x.Id == id);
            user.AccountBalance += response.Amount;
            _context.Accounts.Update(user);
            _context.SaveChanges();

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Payment successfully"
            });
        }
    }
}
