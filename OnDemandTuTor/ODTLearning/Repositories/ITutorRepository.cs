using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface ITutorRepository 
    {
        public TutorProfileModel GetTutorProfile(string id);
        public bool UpdateTutorProfile(string idTutor, TutorProfileToUpdate model);
        public List<TutorListModel> SearchTutorList(SearchTutorModel model);
    }
}
