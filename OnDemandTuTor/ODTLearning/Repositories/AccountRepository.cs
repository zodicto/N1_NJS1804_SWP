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
        public SignUpModelOfAccount? SignUpValidationOfAccount(SignUpModelOfAccount model) => _context.Accounts.Any(a => a.Gmail == model.Email) ? null : model;
        public Account SignUpOfAccount(SignUpModelOfAccount model)
        {

            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                FullName = model.Fullname,
                Password = model.Password,
<<<<<<< HEAD
                PhoneNumber = model.Phone,
                AccountBalance = null,
=======
                PhoneNumber = model.phone,
                AccountBalance = 0,
>>>>>>> 5bf4fe10fc7aa035a5a3914b65e7f2704ccce55a
                Gmail = model.Email,
                Birthdate = model.date_of_birth,
                Gender = model.Gender,
                Role = "student"
            };

            // Thêm Account vào context
            _context.Accounts.Add(account);
            _context.SaveChanges();
            return account;
        }

        public SignInModel? SignInValidation(SignInModel model)
        {
            if (_context.Accounts.Any(a => a.Gmail != model.Email) || _context.Accounts.Any(a => a.Password != model.Password))
            {
                return null; // Email không trùng hoặt pass không trùng
            }
            return model; // Email không tồn tại
        }

        public object SignUpOftutor(string IdAccount, SignUpModelOfTutor model)
        {
            // Tìm kiếm account trong DB bằng id
            var existingUser = _context.Accounts.FirstOrDefault(a => a.Id == IdAccount);
            if (existingUser != null)
            {
                // Cập nhật vai trò của tài khoản thành "tutor"
                existingUser.Role = "Tutor";
                _context.Accounts.Update(existingUser);

                // Tạo mới đối tượng tutor
                var tutor = new Tutor
                {
                    Id = Guid.NewGuid().ToString(),
                    SpecializedSkills = model.SpecializedSkills,
                    Experience = model.Experience,
                    Status = "Operating",
                    IdAccount = existingUser.Id
                };

                // Tạo mới đối tượng educationalqualification
                var educationalQualification = new EducationalQualification
                {
                    Id = Guid.NewGuid().ToString(),
                    IdTutor = tutor.Id,
                    QualificationName = model.QualificationName,
                    Img = model.ImageQualification,
                    Type = model.Type,
                };

                // Kiểm tra xem subject có tồn tại không, nếu không thì tạo mới
                var subject = _context.Subjects.FirstOrDefault(s => s.SubjectName == model.Subject);
                if (subject == null)
                {
                    subject = new Subject
                    {
                        Id = Guid.NewGuid().ToString(),
                        SubjectName = model.Subject,
                    };
                    _context.Subjects.Add(subject);
                }

                // Tạo mới đối tượng TutorSubject
                var tutorSubject = new TutorSubject
                {
                    Id = Guid.NewGuid().ToString(),
                    IdSubject = subject.Id,
                    IdTutor = tutor.Id,
                };

                // Thêm các đối tượng vào DB
                _context.Tutors.Add(tutor);
                _context.EducationalQualifications.Add(educationalQualification);
                _context.TutorSubjects.Add(tutorSubject);

                try
                {
                    _context.SaveChanges();
                    // Trả về đối tượng tutor đã được tạo
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


        public Account? SignInValidationOfAccount(SignInModel model) => _context.Accounts.FirstOrDefault(u => u.Gmail == model.Email && u.Password == model.Password);

        public TokenModel generatetoken(Account user)
        {
            var jwttokenhandler = new JwtSecurityTokenHandler();
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["appsettings:secretkey"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, user.FullName + " " + user.Birthdate?.ToString("yyyy-MM-dd")),
                 new Claim(JwtRegisteredClaimNames.Email, user.Gmail),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(ClaimTypes.Role, user.Role),
                 new Claim("id", user.Id )

            };


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
                IdAccount = user.Id,
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
                Access_Token = accesstoken,
                Refresh_Token = refreshtoken
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

        public List<Account> getallusers()
        {
            var list = _context.Accounts.ToList();
            return list;
        }
    }
}
