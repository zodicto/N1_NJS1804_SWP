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
        public Account SignUpOfAccount(SignUpModelOfAccount model);

        public object SignUpOftutor(string IdAccount, SignUpModelOfTutor model);
        public SignUpModelOfAccount? SignUpValidationOfAccount(SignUpModelOfAccount model);

        public SignInModel? SignInValidation(SignInModel model);
        public Account? SignInValidationOfAccount(SignInModel model);

        public TokenModel generatetoken(Account user);
        public string generaterefreshtoken();
        public List<Account> getallusers();
    }
}
