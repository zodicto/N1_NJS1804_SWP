using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IStudentRepository
    {
        public Task<ApiResponse<bool>> CreateRequestLearning(string IDAccount, RequestLearningModel model);
        public Task<ApiResponse<bool>> UpdateRequestLearning(string requestId, RequestLearningModel model);
        public Task<ApiResponse<bool>> DeleteRequestLearning(string accountId, string requestId);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetPendingRequestsByAccountId(string id);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetApprovedRequestsByAccountId(string id);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetRejectRequestsByAccountId(string accountId);
        public Task<ApiResponse<List<TutorListModel>>> ViewAllTutorJoinRequest(string requestId);
        public Task<ApiResponse<SelectTutorModel>> SelectTutor(string idRequest, string idAccountTutor);
        public Task<ApiResponse<bool>> CreateComplaint(ComplaintModel model);
        public Task<ApiResponse<bool>> CreateReview(ReviewModel model);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetClassProcess(string id);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetClassCompled(string id);
        public Task<ApiResponse<object>> GetSignUpTutor(string id);
        public Task<ApiResponse<bool>> BookingServiceLearning(string id, string idService, BookingServiceLearingModels model);
    }
}
