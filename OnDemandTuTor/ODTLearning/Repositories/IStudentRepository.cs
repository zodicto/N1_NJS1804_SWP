using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IStudentRepository
    {
        public Task<ApiResponse<bool>> CreateRequestLearning(string IDAccount, RequestLearningModel model);
        public Task<ApiResponse<bool>> UpdateRequestLearning(string requestId, RequestLearningModel model);
        public Task<ApiResponse<bool>> DeleteRequestLearning(string requestId);
        public Task<ApiResponse<List<RequestLearningModel>>> GetPendingRequestsByAccountId(string accountId);
        public Task<ApiResponse<List<RequestLearningModel>>> GetApprovedRequestsByAccountId(string accountId);
        public Task<ApiResponse<List<RequestLearningModel>>> GetRejectRequestsByAccountId(string accountId);
        //public Task<object> GetStudentProfile(string id);
        public Task<ApiResponse<List<TutorListModel>>> ViewAllTutorJoinRequest(string requestId);
        public Task<ApiResponse<SelectTutorModel>> SelectTutor(string idRequest, string idAccountTutor);
        public Task<ApiResponse<ComplaintResponse>> CreateComplaint(ComplaintModel model);
    }
}
