using ODTLearning.Models;
using ODTLearning.Entities;

namespace ODTLearning.Repositories
{
    public interface IStudentRepository
    {
        public object CreateRequestLearning(String IdStudent, RequestLearningModel model);
        public Request UpdateRequestLearning(string requestId, RequestLearningModel model);
        public void DeleteRequestLearning(string requestId);
        public List<Request> GetPendingApproveRequests();
        public List<Request> GetApprovedRequests();
    }
}
