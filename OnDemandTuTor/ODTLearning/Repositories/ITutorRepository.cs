using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface ITutorRepository 
    {
        public Task<TutorProfileModel> GetTutorProfile(string id);
        public Task<bool> UpdateTutorProfile(string idTutor, TutorProfileToUpdate model);
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetApprovedRequests();
        public Task<List<TutorListModel>> SearchTutorList(SearchTutorModel model);
        public Task<ApiResponse<bool>> JoinRequest(string requestId, string tutorId, JoinRequestModel joinRequestModel);
    }
}
