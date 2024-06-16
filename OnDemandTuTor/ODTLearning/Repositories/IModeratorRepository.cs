using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IModeratorRepository
    {
        public  Task<object?> GetTutorProfileToConfirm(string id);
        public  Task<ApiResponse<bool>> ConfirmRequest(string requestId, string status);
        public  Task<bool> ConfirmProfileTutor(string idTutor, string status);
        public  Task<List<ListTutorToConfirm>> GetListTutorsToCofirm();
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetPendingRequests();
    }
}

