using ODTLearning.Models;

namespace ODTLearning.BLL.Repositories
{
    public interface IModeratorRepository
    {
      
        public Task<ApiResponse<bool>> DeleteRequest(string idRequest);
    }
}

