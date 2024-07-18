using ODTLearning.DAL.Entities;
using ODTLearning.Models;

namespace ODTLearning.BLL.Repositories
{
    public interface IStudentRepository
    {
        public Task<ApiResponse<bool>> CreateRequestLearning(string IDAccount, RequestLearningModel model);
        public Task<ApiResponse<bool>> UpdateRequestLearning(string requestId, RequestLearningModel model);
        public Task<ApiResponse<bool>> DeleteRequestLearning(string accountId, string requestId);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetPendingRequestsByAccountId(string id);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetApprovedRequestsByAccountId(string id);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetRejectRequestsByAccountId(string accountId);
       
        public Task<ApiResponse<bool>> CreateComplaint(ComplaintModel model);
        public Task<ApiResponse<bool>> CreateReviewRequest(ReviewRequestModel model);
        public Task<ApiResponse<bool>> CreateReviewService(ReviewServiceModel model);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetClassProcess(string id);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetClassCompled(string id);

    }
}
