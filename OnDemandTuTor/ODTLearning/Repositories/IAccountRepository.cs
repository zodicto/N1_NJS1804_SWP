using Microsoft.AspNetCore.Mvc;
using ODTLearning.Entities;

//using ODTLearning.Entities;
using ODTLearning.Models;

namespace ODTLearning.Repositories
{
    public interface IAccountRepository
    {
        public SignUpValidationTutorModel SignUpValidationTutor(SignUpModelOfTutor model);
        public SignUpValidationStudentModel SignUpValidationStudent(SignUpModelOfStudent model);

        public object SignUpOfStudent(SignUpModelOfStudent model);
        public object SignUpOfTutor(SignUpModelOfTutor model);

        public SignInValidationModel SignInValidation(SignInModel model);
        public Account Authentication(SignInModel model);
        public TokenModel GenerateToken(Account user);
        public List<Account> GetAllUser();


    }
}
