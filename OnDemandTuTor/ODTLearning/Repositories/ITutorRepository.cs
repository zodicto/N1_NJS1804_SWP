using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface ITutorRepository 
    {
        public Task<ApiResponse<object>> GetTutorProfile(string id);
        public Task<ApiResponse<bool>> UpdateTutorProfile(string id, TutorProfileToUpdate model);
        public Task<ApiResponse<bool>> AddSubject(string id, string subjectName);
        public Task<ApiResponse<bool>> AddQualification(string id, AddQualificationModel model);
        public Task<ApiResponse<bool>> DeleteSubject(string id, string subjectName);
        public Task<ApiResponse<bool>> DeleteQualification(string id, string idQualification);
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetApprovedRequests();
//public Task<List<TutorListModel>> SearchTutorList(SearchTutorModel model);
        public Task<ApiResponse<bool>> JoinRequest(string requestId, string tutorId);
    }
}
