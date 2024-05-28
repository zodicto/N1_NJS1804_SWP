using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IAccountRepository
    {
        public SignUpValidationModel SignUpValidation(SignUpModel model);
        public object SignUp(SignUpModel model);
        public SignInValidationModel SignInValidation(SignInModel model);
        public User Authentication(SignInModel model);
        public TokenModel GenerateToken(User user);
        public List<User> GetAllUser();


    }
}
