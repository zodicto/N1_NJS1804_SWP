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

        public SignUpValidationOfTutorModel SignUpValidationTutor(SignUpModelOfTutor model)
        {
           String img = "", specializedSkills = "", organization = "", field = "", type = "", imageDegree = "";
            int i = 0;
            
            if (string.IsNullOrEmpty(model.SpecializedSkills))
            {
                specializedSkills = "Please do not SpecializedSkills empty!!";
                i++;
            }
            if (string.IsNullOrEmpty(model.Organization))
            {
                organization = "Please do not Organization empty!!";
                i++;
            }
            if (string.IsNullOrEmpty(model.Field))
            {
                field = "Please do not Field empty!!";
                i++;
            }
            if (string.IsNullOrEmpty(model.Type))
            {
                type = "Please do not Type empty!!";
                i++;
            }
            if (string.IsNullOrEmpty(model.ImageDegree))
            {
                imageDegree = "Please do not Image Degree empty!!";
                i++;
            }


            if (i != 0)
            {
                return new SignUpValidationOfTutorModel
                {
                    
                   SpecializedSkills = specializedSkills,   
                   Organization = organization,
                   Field = field,
                   Type = type,
                   ImageDegree = img,
                };
            }

            return null;
        }
       
        public SignUpValidationOfAccountModel SignUpValidationOfAccount(SignUpModelOfAccount model) //Tri(Sửa của Tânân) : Xử lý cho việc check validate
        {
            string username = "", password = "", passwordConfirm = "", firstname = "", lastname = "", gmail = "";
            int i = 0;
            if (string.IsNullOrEmpty(model.Username))
            {
                username = "Please do not Username empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                password = "Please do not Password empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.PasswordConfirm))
            {
                passwordConfirm = "Please do not PasswordConfirm empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                firstname = "Please do not Firstname empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                lastname = "Please do not Lastname empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.Gmail))
            {
                gmail = "Please do not Gmail empty!!";
                i++;
            }

            if (i != 0)
            {
                return new SignUpValidationOfAccountModel
                {
                    Username = username,
                    Password = password,
                    PasswordConfirm = passwordConfirm,
                    FirstName = firstname,
                    LastName = lastname,
                    Gmail = gmail,
                };
            }

            return null;
        }

        public object SignUpOfAccount(SignUpModelOfAccount model)// Trí(Sửa của Tân): Tạo mới một object và lưu vào Db theo tưng thuộc tính 
        {
            if (model.Password != model.PasswordConfirm)
            {
                return null;
            }

            var user = new Account
            {
                IdAccount = Guid.NewGuid().ToString(),
                FisrtName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Password = model.Password,
                Gmail = model.Gmail,
                Birthdate = model.Birthdate,
                Gender = model.Gender,
                Role = "Student"
            };
            

            _context.Accounts.Add(user);
            _context.SaveChanges();

            return user;
        }
        public object SignUpOfTutor(String IDAccount,SignUpModelOfTutor model)
        {
            // Tìm kiếm tài khoản trong cơ sở dữ liệu bằng ID
            var existingUser = _context.Accounts.FirstOrDefault(a => a.IdAccount == IDAccount);

            if (existingUser != null)
            {
                // Cập nhật vai trò của tài khoản thành "Tutor"
                existingUser.Role = "Tutor";

                // Tạo mới đối tượng Tutor
                var tutor = new Tutor
                {
                    IdTutor = Guid.NewGuid().ToString(),
                    IdAccount = existingUser.IdAccount, // Gán ID của tài khoản
                    SpecializedSkills = model.SpecializedSkills,
                    Experience = model.Experience,
                    Status = true
                };

                // Tạo mới đối tượng EducationalQualification
                var educationalQualifications = new EducationalQualification
                {
                    IdEducationalEualifications = Guid.NewGuid().ToString(),
                    IdTutor = tutor.IdTutor,
                    CertificateName = model.QualificationName,
                    Organization = model.Organization,
                    Img = model.ImageDegree,
                    Type = model.Type,
                };

                // Tạo mới đối tượng TutorField và Field
                var tutorField = new TutorField
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

                // Thêm các đối tượng vào cơ sở dữ liệu
                _context.Tutors.Add(tutor);
                _context.EducationalQualifications.Add(educationalQualifications);
                _context.TutorFields.Add(tutorField);
                _context.Fields.Add(field);
                _context.SaveChanges();

                // Trả về tài khoản đã được cập nhật vai trò thành "Tutor"
                return existingUser;
            }

            // Trường hợp không tìm thấy tài khoản
            return null;
        }


        public SignInValidationModel SignInValidation(SignInModel model)
        {
            string username = "", password = "";
            int i = 0;
            if (model.Username == null)
            {
                username = "Please do not Username empty!!";
                i++;
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                password = "Please do not Password empty!!";
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

        public Account Authentication(SignInModel model) => _context.Accounts.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

        public TokenModel GenerateToken(Account user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.LastName + "" + user.FisrtName),
                new Claim(JwtRegisteredClaimNames.Email, user.Gmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Id", user.IdAccount )

            };

            // Chuyển đổi Birthdate từ DateTime? sang string nếu không null
            if (user.Birthdate.HasValue)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Birthdate, user.Birthdate.Value.ToString("yyyy-MM-dd")));
            }

            // Thêm Gender nếu không null
            if (!string.IsNullOrEmpty(user.Gender))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Gender, user.Gender));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                IdAccount = user.IdAccount,
                JwtId = token.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow,
            };

            _context.AddAsync(refreshTokenEntity);
            _context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        public List<Account> GetAllUser()
        {
            var list = _context.Accounts.ToList();
            return list;
        }

      
    }
}
