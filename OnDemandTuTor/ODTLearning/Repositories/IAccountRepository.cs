using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;

//using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IAccountRepository
    {
        public SignUpValidationModel SignUpValidation(SignUpModel model);
        public object SignUp(SignUpModel model);
        public SignInValidationModel SignInValidation(SignInModel model);
        public Account Authentication(SignInModel model);
        public TokenModel GenerateToken(Account user);
        public List<Account> GetAllUser();


    }
}
