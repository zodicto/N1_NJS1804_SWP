using ODTLearning.Models;

namespace ODTLearning.BLL.Repositories
{
    public interface IModeratorRepository
    {
        public Task<ApiResponse<bool>> RejectProfileTutor(string id, ReasonReject model);
        public Task<ApiResponse<bool>> ApproveProfileTutor(string id);
        public Task<ApiResponse<List<ListTutorToConfirm>>> GetListTutorsToConfirm();
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetPendingRequests();
        public Task<ApiResponse<bool>> RejectRequest(string requestId, ReasonReject model);
        public Task<ApiResponse<bool>> ApproveRequest(string requestId);
        public Task<ApiResponse<bool>> DeleteRequest(string idRequest);
    }
}

