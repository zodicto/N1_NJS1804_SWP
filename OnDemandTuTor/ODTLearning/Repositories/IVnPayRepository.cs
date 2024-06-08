using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IVnPayRepository
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
