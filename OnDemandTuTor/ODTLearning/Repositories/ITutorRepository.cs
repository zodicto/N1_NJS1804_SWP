namespace ODTLearning.Repositories
{
    public interface ITutorRepository
    {
        public object GetTutorProfileToConFirm(string id);
        public bool ConFirmProfileTutor(String idTutor, String status);
    }
}
