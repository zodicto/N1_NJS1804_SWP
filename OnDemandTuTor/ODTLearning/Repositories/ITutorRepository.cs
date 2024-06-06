using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface ITutorRepository 
    {


        public bool UpdateTutorProfile(string idTutor, TutorProfileToUpdate model);
    }
}
