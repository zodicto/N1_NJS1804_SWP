using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IAdminRepository
    {
        public Task<bool> DeleteAccount(string IDAccount);
        public Task<ApiResponse<object>> ViewRent(string Condition);
        public Task<ApiResponse<List<ListAccount>>> GetListStudent();
        public  Task<ApiResponse<List<ListAccount>>> GetListTutor();
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestPending();
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestApproved();
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestReject();
    }
}
