using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ODTLearning.Entities;

//using ODTLearning.Entities;

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
        public SignUpValidationOfTutorModel signupvalidationtutor(SignUpModelOfTutor model);

        public SignUpValidationOfAccountModel signupvalidationofaccount(SignUpModelOfAccount model);

        public object SignupOfaccount(SignUpModelOfAccount model);
        public object SignupOftutor(string IdAccount, SignUpModelOfTutor model);


        public SignInValidationModel signinvalidation(SignInModel model);
        public Acount authentication(SignInModel model);

        public TokenModel generatetoken(Acount user);

        public string generaterefreshtoken();

        public List<Acount> getallusers();




    }
}
