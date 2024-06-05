using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface ITutorListRepository
    {
        public List<Acount> SearchTutorList(string name, string field);
    }
}
