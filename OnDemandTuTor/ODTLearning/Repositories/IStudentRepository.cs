using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IStudentRepository
    {
        public object CreateRequestLearning(String IdStudent, RequestLearningModel model);
        public Request UpdateRequestLearning(string requestId, RequestLearningModel model);
        public bool DeleteRequestLearning(string requestId);
        public List<Request> GetPendingApproveRequests();
        public List<Request> GetApprovedRequests();
    }
}
