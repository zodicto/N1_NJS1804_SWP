using Microsoft.EntityFrameworkCore;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface ITutorRepository 
    {
        object GetTutorProfileToConFirm(string id);
        bool ConFirmProfileTutor(string idTutor, string status);
        bool UpdateTutorProfile(string idTutor, TutorProfileMVModel model);
    }
}
