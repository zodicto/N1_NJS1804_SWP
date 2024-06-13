namespace ODTLearning.Repositories
{
    public interface IModeratorRepository
    {
        public Task<object> GetTutorProfileToConFirm(string id);
        public Task<string> ChangeRequestLearningStatus(string requestId, string status);
        Task<bool> ConFirmProfileTutor(string idTutor, string status);
    }
}
