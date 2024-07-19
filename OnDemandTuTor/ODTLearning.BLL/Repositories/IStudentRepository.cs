using ODTLearning.DAL.Entities;
using ODTLearning.Models;

namespace ODTLearning.BLL.Repositories
{
    public interface IStudentRepository
    {
        public Task<ApiResponse<List<ListAllStudent>>> GetListStudent();
        public Task<ApiResponse<int>> GetAmountStudent();
    }
}
