using ODTLearning.Models;

namespace ODTLearning.BLL.Repositories
{
    public interface ITutorRepository 
    {
        public Task<ApiResponse<object>> GetTutorProfile(string id);
        public Task<ApiResponse<bool>> UpdateTutorProfile(string id, TutorProfileToUpdate model);
        public Task<ApiResponse<bool>> AddSubject(string id, string subjectName);
        public Task<ApiResponse<bool>> AddQualification(string id, AddQualificationModel model);
        public Task<ApiResponse<bool>> DeleteSubject(string id, string subjectName);
        public Task<ApiResponse<bool>> DeleteQualification(string id, string idQualification);
        public Task<ApiResponse<List<ViewRequestOfStudent>>> GetApprovedRequests(string id);
        public Task<ApiResponse<bool>> JoinRequest(string requestId, string tutorId);
        public Task<ApiResponse<List<RequestLearningResponse>>> GetClassProcess(string accountId);
        public Task<ApiResponse<bool>> CreateServiceLearning(string id, ServiceLearningModel model);
        public  Task<ApiResponse<List<object>>> GetAllServicesByAccountId(string id);
        public Task<ApiResponse<bool>> DeleteServiceById(string serviceId);
        public Task<ApiResponse<object>> UpdateServiceById(string serviceId, ServiceLearningModel model);
        public Task<ApiResponse<object>> GetReview(string id);
        public Task<ApiResponse<object>> GetRegisterTutor(string id);
        public Task<ApiResponse<bool>> ReSignUpOftutor(string id, SignUpModelOfTutor model);
    }
}
