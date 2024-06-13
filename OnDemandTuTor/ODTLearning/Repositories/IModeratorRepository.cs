using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IModeratorRepository
    {
        public  Task<object?> GetTutorProfileToConfirm(string id);
        public Task<string> ChangeRequestLearningStatus(string requestId, string status);
        public  Task<bool> ConfirmProfileTutor(string idTutor, string status);
        public  Task<List<ListTutorToConfirm>> GetListTutorsToCofirm();
    }
}
