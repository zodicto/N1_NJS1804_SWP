using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IAccountRepository
    {
        UserResponse SignUpOfAccount(SignUpModelOfAccount model);
        ApiResponse<TutorResponse> SignUpOftutor(string IdAccount, SignUpModelOfTutor model);
        bool IsEmailExist(string email);
        ApiResponse<UserResponse> SignInValidationOfAccount(SignInModel model);
        TokenModel GenerateToken(UserResponse user);
        string GenerateRefreshtoken();
        List<Account> GetAllUsers();
        bool UpdateAvatar(string id, IFormFile file);
        string ChangePassword(string id, ChangePasswordModel model);
        string ForgotPassword(string Email);
        ApiResponse<bool> UpdateProfile(string id, UpdateProfile model);
        ApiResponse<object> GetProfile(string id);
    }
}
