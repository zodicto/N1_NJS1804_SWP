using ODTLearning.Models;
using Microsoft.AspNetCore.Http;

namespace ODTLearning.BLL.Repositories
{
    public interface IVnPayRepository
    {
        Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        Task<VnPaymentResponseModel> PaymentExecute(IQueryCollection collections);
    }
}
