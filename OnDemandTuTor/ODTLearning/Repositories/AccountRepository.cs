using Aqua.EnumerableExtensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ODTLearning.Entities;
using ODTLearning.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ODTLearning.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DbminiCapstoneContext _context;
        private readonly IConfiguration _configuration;

        public AccountRepository(DbminiCapstoneContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public SignUpValidationOfTutorModel signupvalidationtutor(SignUpModelOfTutor model)
        {
            string img = "", specializedskills = "", field = "", type = "", imagedegree = "";
            int i = 0;

            if (string.IsNullOrEmpty(model.SpecializedSkills))
            {
                specializedskills = "please do not specializedskills empty!!";
                i++;
            }
            if (string.IsNullOrEmpty(model.Field))
            {
                field = "please do not field empty!!";
                i++;
            }
            if (string.IsNullOrEmpty(model.Type))
            {
                type = "please do not type empty!!";
                i++;
            }
            if (string.IsNullOrEmpty(model.ImageDegree))
            {
                imagedegree = "please do not image degree empty!!";
                i++;
            }


            if (i != 0)
            {
                return new SignUpValidationOfTutorModel
                {

                    SpecializedSkills = specializedskills,
                    Field = field,
                    Type = type,
                    ImageDegree = img,
                };
            }

            return null;
        }

        public SignUpValidationOfAccountModel signupvalidationofaccount(SignUpModelOfAccount model) //tri(sửa của tânân) : xử lý cho việc check validate
        {
            string username = "", password = "", passwordconfirm = "", firstname = "", lastname = "", gmail = "";
            int i = 0;
            if (string.IsNullOrEmpty(model.Username))
            {
                username = "please do not username empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                password = "please do not password empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.PasswordConfirm))
            {
                passwordconfirm = "please do not passwordconfirm empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                firstname = "please do not firstname empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                lastname = "please do not lastname empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.Gmail))
            {
                gmail = "please do not gmail empty!!";
                i++;
            }

            if (i != 0)
            {
                return new SignUpValidationOfAccountModel
                {
                    Username = username,
                    Password = password,
                    PasswordConfirm = passwordconfirm,
                    FirstName = firstname,
                    LastName = lastname,
                    Gmail = gmail,
                };
            }

            return null;
        }

        public object SignupOfaccount(SignUpModelOfAccount model)// trí(sửa của tân): tạo mới một object và lưu vào db theo tưng thuộc tính 
        {
            if (model.Password != model.PasswordConfirm)
            {
                return null;
            }
            
            var account = new Acount
            {
                IdAccount = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Password = model.Password,
                Gmail = model.Gmail,
                Birthdate = model.Birthdate,
                Gender = model.Gender,
                Role = "student"
            };


            _context.Acounts.Add(account);
            _context.SaveChanges();

            return account;
        }
        public object SignupOftutor(string IdAccount, SignUpModelOfTutor model)
        {
            // tìm kiếm tài khoản trong cơ sở dữ liệu bằng id
            var existinguser = _context.Acounts.FirstOrDefault(a => a.IdAccount == IdAccount);

            if (existinguser != null)
            {
                // cập nhật vai trò của tài khoản thành "tutor"
                existinguser.Role = "tutor";

                // tạo mới đối tượng tutor
                var tutor = new Tutor
                {
                    IdTutor = Guid.NewGuid().ToString(),                 
                    SpecializedSkills = model.SpecializedSkills,
                    Experience = model.Experience,
                    Status = "operating",
                };

                // tạo mới đối tượng educationalqualification
                var educationalqualifications = new EducationalQualification
                {
                    IdEducationalEualifications = Guid.NewGuid().ToString(),
                    IdTutor = tutor.IdTutor,
                    CertificateName = model.QualificationName,
                    Img = model.ImageDegree,
                    Type = model.Type,
                };

                // tạo mới đối tượng tutorfield và field
                var tutorfield = new TutorField
                {
                    IdTutorFileld = Guid.NewGuid().ToString(),
                    IdField = Guid.NewGuid().ToString(),
                    IdTutor = tutor.IdTutor,
                };
                var field = new Field
                {
                    IdField = Guid.NewGuid().ToString(),
                    FieldName = model.Field,
                };

                // thêm các đối tượng vào cơ sở dữ liệu
                _context.Tutors.Add(tutor);
                _context.EducationalQualifications.Add(educationalqualifications);
                _context.TutorFields.Add(tutorfield);
                _context.Fields.Add(field);
                _context.SaveChanges();

                // trả về tài khoản đã được cập nhật vai trò thành "tutor"
                return existinguser;
            }

            // trường hợp không tìm thấy tài khoản
            return null;
        }


        public SignInValidationModel signinvalidation(SignInModel model)
        {
            string username = "", password = "";
            int i = 0;
            if (model.Username == null)
            {
                username = "please do not username empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                password = "please do not password empty!!";
                i++;
            }

            if (i != 0)
            {
                return new SignInValidationModel
                {
                    Username = username,
                    Password = password
                };
            }

            return null;
        }

        public Acount authentication(SignInModel model) => _context.Acounts.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

        public TokenModel generatetoken(Acount user)
        {
            var jwttokenhandler = new JwtSecurityTokenHandler();
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["appsettings:secretkey"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.LastName + "" + user.Birthdate),
                new Claim(JwtRegisteredClaimNames.Email, user.Gmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("id", user.IdAccount )

            };

            // chuyển đổi birthdate từ datetime? sang string nếu không null
            //if (user.Birthdate.hasvalue)
            //{
            //    claims.Add(new Claim(JwtRegisteredClaimNames.Birthdate, user.Birthdate.value.tostring("yyyy-mm-dd")));
            //}

            // thêm gender nếu không null
            if (!string.IsNullOrEmpty(user.Gender))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Gender, user.Gender));
            }

            var tokendescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = credentials
            };

            var token = jwttokenhandler.CreateToken(tokendescriptor);
            var accesstoken = jwttokenhandler.WriteToken(token);
            var refreshtoken = generaterefreshtoken();

            var refreshtokenentity = new RefreshToken
            {
                Id = Guid.NewGuid().ToString(),
                IdAccount = user.IdAccount,
                JwtId = token.Id,
                Token = refreshtoken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow,
            };

            _context.AddAsync(refreshtokenentity);
            _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accesstoken,
                RefreshToken = refreshtoken
            };
        }

        public string generaterefreshtoken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        public List<Acount> getallusers()
        {
            var list = _context.Acounts.ToList();
            return list;
        }


    }
}
