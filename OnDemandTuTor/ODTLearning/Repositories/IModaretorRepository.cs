namespace ODTLearning.Repositories
{
    public interface IModaretorRepository
    {
        public object GetTutorProfileToConFirm(string id);
        public string ChangeRequestLearningStatus(string requestId, string status);
        bool ConFirmProfileTutor(string idTutor, string status);
    }
}
