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
        public Task<Account> SignUpOfAccount(SignUpModelOfAccount model);

        public Task<object> SignUpOftutor(string IdAccount, SignUpModelOfTutor model);
        public Task<SignUpModelOfAccount> SignUpValidationOfAccount(SignUpModelOfAccount model);

        public Task<Account> SignInValidationOfAccount(SignInModel model);

        public Task<TokenModel> GenerateToken(Account user);
        public Task<string> GenerateRefreshtoken();
        public Task<List<Account>> GetAllUsers();
    }
}
