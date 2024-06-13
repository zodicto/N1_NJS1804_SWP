using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IVnPayRepository
    {
        Task<string> CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        Task<VnPaymentResponseModel> PaymentExecute(IQueryCollection collections);
    }
}
