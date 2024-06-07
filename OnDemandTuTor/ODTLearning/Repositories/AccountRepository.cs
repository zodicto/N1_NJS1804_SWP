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

        public object SignUpOfAccount(SignUpModelOfAccount model)// trí(sửa của tân): tạo mới một object và lưu vào db theo tưng thuộc tính 
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

            var student = new Student
            {
                IdStudent = Guid.NewGuid().ToString(),
                IdAccount = account.IdAccount,
                IdAccountNavigation = account
            };

            // Thêm Account và Student vào context
            _context.Acounts.Add(account);
            _context.Students.Add(student);
            _context.SaveChanges();

            return account;
        }
        public SignInValidationModel SignInValidation(SignInModel model)
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
        public SignUpValidationOfTutorModel SignUpValidationOfTutor(SignUpModelOfTutor model)
        {
            string Img = "", SpecializedSkills = "", Field = "", Type = "", ImageDegree = "";
            int i = 0;

            if (string.IsNullOrEmpty(model.SpecializedSkills))
            {
                SpecializedSkills = "please do not specializedskills empty!!";
                i++;
            }
            if (string.IsNullOrEmpty(model.Field))
            {
                Field = "please do not field empty!!";
                i++;
            }
            if (string.IsNullOrEmpty(model.Type))
            {
                Type = "please do not type empty!!";
                i++;
            }
            if (string.IsNullOrEmpty(model.ImageDegree))
            {
                ImageDegree = "please do not image degree empty!!";
                i++;
            }

            if (i != 0)
            {
                return new SignUpValidationOfTutorModel
                {

                    SpecializedSkills = SpecializedSkills,
                    Field = Field,
                    Type = Type,
                    ImageDegree = Img,
                };
            }

            return null;
        }
        public SignUpValidationOfAccountModel SignUpValidationOfAccount(SignUpModelOfAccount model)
        {
            string username = "", password = "", confirm_password = "", firstname = "", lastname = "", gmail = "";
            int i = 0;

            if (string.IsNullOrEmpty(model.Username))
            {
                username = "Please do not leave username empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                password = "Please do not leave password empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.PasswordConfirm))
            {
                confirm_password = "Please do not leave password confirmation empty!!";
                i++;
            }
            else if (model.Password != model.PasswordConfirm)
            {
                confirm_password = "Password and password confirmation are not the same!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                firstname = "Please do not leave first name empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                lastname = "Please do not leave last name empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.Gmail))
            {
                gmail = "Please do not leave email empty!!";
                i++;
            }

            if (i != 0)
            {
                return new SignUpValidationOfAccountModel
                {
                    Username = username,
                    Password = password,
                    PasswordConfirm = confirm_password,
                    FirstName = firstname,
                    LastName = lastname,
                    Gmail = gmail,
                };
            }

            return null;
        }
        public object SignUpOftutor(string IdAccount, SignUpModelOfTutor model)
        {
            // Tìm kiếm account trong DB bằng id
            var existingUser = _context.Acounts.FirstOrDefault(a => a.IdAccount == IdAccount);

            if (existingUser != null)
            {
                // Cập nhật vai trò của tài khoản thành "tutor"
                existingUser.Role = "tutor";
                _context.Acounts.Update(existingUser);

                // Tạo mới đối tượng tutor
                var tutor = new Tutor
                {
                    IdTutor = Guid.NewGuid().ToString(),
                    SpecializedSkills = model.SpecializedSkills,
                    Experience = model.Experience,
                    Status = "Operating",
                    IdAccount = existingUser.IdAccount
                };

                // Tạo mới đối tượng educationalqualification
                var educationalQualifications = new EducationalQualification
                {
                    IdEducationalEualifications = Guid.NewGuid().ToString(),
                    IdTutor = tutor.IdTutor,
                    CertificateName = model.QualificationName,
                    Img = model.ImageDegree,
                    Type = model.Type,
                };

                // Kiểm tra xem field có tồn tại không, nếu không thì tạo mới
                var field = _context.Fields.FirstOrDefault(f => f.FieldName == model.Field);
                if (field == null)
                {
                    field = new Field
                    {
                        IdField = Guid.NewGuid().ToString(),
                        FieldName = model.Field,
                    };
                    _context.Fields.Add(field);
                }

                // Tạo mới đối tượng tutorfield
                var tutorField = new TutorField
                {
                    IdTutorFileld = Guid.NewGuid().ToString(),
                    IdField = field.IdField,
                    IdTutor = tutor.IdTutor,
                };

                // Thêm các đối tượng vào DB
                _context.Tutors.Add(tutor);
                _context.EducationalQualifications.Add(educationalQualifications);
                _context.TutorFields.Add(tutorField);

                try
                {
                    _context.SaveChanges();
                    // Trả về tài khoản đã được cập nhật vai trò thành "tutor"
                    return existingUser;
                }
                catch (Exception ex)
                {
                    // Ghi lại lỗi nếu có xảy ra
                    Console.WriteLine($"Error while saving changes: {ex.Message}");
                }
            }

            // Trường hợp không tìm thấy tài khoản
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
