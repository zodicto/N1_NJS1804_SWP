namespace ODTLearning.Repositories
{
    public interface IAdminRepository
    {
        public Task<bool> DeleteAccount(string IDAccount);
    }
}
