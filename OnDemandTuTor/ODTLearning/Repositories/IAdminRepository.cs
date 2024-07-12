using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IAdminRepository
    {
        public Task<ApiResponse<bool>> DeleteAccount(string id);
        //public Task<ApiResponse<object>> ViewRent(string Condition);
        public Task<ApiResponse<List<ListAllStudent>>> GetListStudent();
        //public Task<ApiResponse<List<object>>> GetListTutor();
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestPending();
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestApproved();
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetListRequestReject();
        public Task<ApiResponse<object>> GetAllComplaint();
        public Task<ApiResponse<object>> GetAllTransaction();
        public Task<ApiResponse<object>> GetAllReview();
        public Task<ApiResponse<object>> GetRevenueByMonth(int year);
        public Task<ApiResponse<object>> GetRevenueByWeek(int month, int year);
        public Task<ApiResponse<object>> GetRevenueByYear(int year);
        public Task<ApiResponse<object>> GetRevenueThisMonth();
        public Task<ApiResponse<int>> GetAmountStudent();
        public Task<ApiResponse<int>> GetAmountTutor();
    }
}