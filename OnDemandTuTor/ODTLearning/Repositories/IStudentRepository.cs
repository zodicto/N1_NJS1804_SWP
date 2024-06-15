using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IStudentRepository
    {
        public Task<object> CreateRequestLearning(String IdStudent, RequestLearningModel model);
        public Task<Request> UpdateRequestLearning(string requestId, RequestLearningModel model);
        public Task<bool> DeleteRequestLearning(string requestId);
        public Task<List<Request>> GetPendingApproveRequests();
        public Task<List<Request>> GetApprovedRequests();
        public Task<object> GetStudentProfile(string id);
        public Task<bool> UpdateStudentProfile(string id, StudentProfileToUpdateModel model);
    }
}
