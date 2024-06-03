using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ODTLearning.Entities;

//using ODTLearning.Entities;
using ODTLearning.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ODTLearning.Repositories
{
    public interface IAccountRepository
    {
        public SignUpValidationOfTutorModel SignUpValidationTutor(SignUpModelOfTutor model);

        public SignUpValidationOfAccountModel SignUpValidationOfAccount(SignUpModelOfAccount model);
        public object SignUpOfAccount(SignUpModelOfAccount model);
        public object SignUpOfTutor(String IDAccount ,SignUpModelOfTutor model);

        public SignInValidationModel SignInValidation(SignInModel model);

        public Account Authentication(SignInModel model);

        public TokenModel GenerateToken(Account user);

        public  string GenerateRefreshToken();

        public List<Account> GetAllUsers();


    }
}
