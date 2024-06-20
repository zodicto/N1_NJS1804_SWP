using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IModeratorRepository
    {
        public  Task<object?> GetTutorProfileToConfirm(string id);
        public Task<bool> ApproveProfileTutor(string id);
        public Task<bool> RejectProfileTutor(string id);
        public  Task<List<ListTutorToConfirm>> GetListTutorsToCofirm();
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetPendingRequests();
        public Task<ApiResponse<bool>> RejectRequest(string requestId);
        public Task<ApiResponse<bool>> ApproveRequest(string requestId);
    }
}

