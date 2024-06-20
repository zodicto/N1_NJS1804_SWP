using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ODTLearning.Entities;
using ODTLearning.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ODTLearning.Repositories
{
    public interface IAccountRepository
    {
        public Task<UserResponse> SignUpOfAccount(SignUpModelOfAccount model);

        public Task<ApiResponse<TutorResponse>> SignUpOftutor(string IdAccount, SignUpModelOfTutor model);
        public  Task<bool> IsEmailExist(string email);
        public  Task<ApiResponse<UserResponse>> SignInValidationOfAccount(SignInModel model);

        public  Task<TokenModel> GenerateToken(UserResponse user);
        public Task<string> GenerateRefreshtoken();
        public Task<List<Account>> GetAllUsers();
        public Task<bool> UpdateAvatar(string id, IFormFile file);
        public Task<string> ChangePassword(string id, ChangePasswordModel model);
        public Task<string> ForgotPassword(string Email);
        public Task<ApiResponse<bool>> UpdateProfile(string id, UpdateProfile model);
        public Task<ApiResponse<object>> GetProfile(string id);
    }
}
