using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IModeratorRepository
    {
        public Task<ApiResponse<bool>> RejectProfileTutor(string id);
        public Task<ApiResponse<bool>> ApproveProfileTutor(string id);
        public Task<ApiResponse<List<ListTutorToConfirmFB>>> GetListTutorsToConfirm();
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetPendingRequests();
        public Task<ApiResponse<bool>> RejectRequest(string requestId);
        public Task<ApiResponse<bool>> ApproveRequest(string requestId);
    }
}

