using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IAdminRepository
    {
        public Task<bool> DeleteAccount(string IDAccount);
        public Task<ApiResponse<object>> ViewRent(string Condition);
    }
}
