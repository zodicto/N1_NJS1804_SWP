using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IStudentRepository
    {
        public Task<ApiResponse<bool>> CreateRequestLearning(string IDAccount, RequestLearningModel model);
        public Task<Request> UpdateRequestLearning(string requestId, RequestLearningModel model);
        public Task<bool> DeleteRequestLearning(string requestId);
        public Task<List<Request>> GetPendingApproveRequests();
        public Task<List<Request>> GetApprovedRequests();
    }
}
