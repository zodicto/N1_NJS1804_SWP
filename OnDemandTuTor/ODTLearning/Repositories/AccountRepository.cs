using Aqua.EnumerableExtensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ODTLearning.Entities;
using ODTLearning.Helpers;
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

        ImageLibrary imgLib = new ImageLibrary();

        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        public async Task<UserResponse> SignUpOfAccount(SignUpModelOfAccount model)
        {

            var user = new Account
            {
                Id = Guid.NewGuid().ToString(),
                FullName = model.FullName,
                Password = model.Password,
                Phone = model.Phone,
                AccountBalance = 0,
                Email = model.Email,
                DateOfBirth = model.date_of_birth,
                Gender = model.Gender,
                Roles = "Student"
            };
            // Thêm Account vào context
            await _context.Accounts.AddAsync(user);
            await _context.SaveChangesAsync();
            return new UserResponse
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Date_of_birth = user.DateOfBirth,
                Gender = user.Gender,
                Roles = user.Roles,
                Avatar = user.Avatar,
                Address = user.Address,
                Phone = user.Phone,
                AccountBalance = user.AccountBalance
            };
            
        }

        public async Task<object> SignUpOftutor(string IdAccount, SignUpModelOfTutor model)
        {
            // Tìm kiếm account trong DB bằng id
            var existingUser = _context.Accounts.FirstOrDefault(a => a.Id == IdAccount);
            if (existingUser != null)
            {
                // Cập nhật vai trò của tài khoản thành "tutor"
                existingUser.Roles = "Tutor";
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
                    Type = model.Type,
                };
                //upload anh
                var upload = await imgLib.UploadImage(model.ImageQualification);
                if (upload)
                {
                    educationalQualification.Img = model.ImageQualification.FileName;
                }
                // Kiểm tra xem subject có tồn tại không, nếu không thì tạo mới
                var subject = _context.Subjects.FirstOrDefault(s => s.SubjectName == model.Subject);
                if (subject == null)
                {
                    subject = new Subject
                    {
                        Id = Guid.NewGuid().ToString(),
                        SubjectName = model.Subject,
                    };
                    await _context.Subjects.AddAsync(subject);
                }

                // Tạo mới đối tượng TutorSubject
                var tutorSubject = new TutorSubject
                {
                    Id = Guid.NewGuid().ToString(),
                    IdSubject = subject.Id,
                    IdTutor = tutor.Id,
                };

                // Thêm các đối tượng vào DB
                await _context.Tutors.AddAsync(tutor);
                await _context.EducationalQualifications.AddAsync(educationalQualification);
                await _context.TutorSubjects.AddAsync(tutorSubject);

                try
                {
                    await _context.SaveChangesAsync();
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


        public async Task<ApiResponse<UserResponse>> SignInValidationOfAccount(SignInModel model)
        {
            // Kiểm tra tài khoản theo email
            var account = await _context.Accounts
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            // Nếu tài khoản không tồn tại hoặc password sai, trả về thông báo lỗi
            if (account == null || account.Password != model.Password)
            {
                return new ApiResponse<UserResponse>
                {
                    Success = false,
                    Message = "Email hoặc password không đúng",
                };
            }

            // Nếu tài khoản tồn tại và password đúng, trả về thông tin người dùng
            return new ApiResponse<UserResponse>
            {
                Success = true,
                Message = "Đăng nhập thành công",
                Data = new UserResponse
                {
                    Id = account.Id,
                    FullName = account.FullName,
                    Email = account.Email,
                    Date_of_birth = account.DateOfBirth,
                    Gender = account.Gender,
                    Roles = account.Roles,
                    Avatar = account.Avatar,
                    Address = account.Address,
                    Phone = account.Phone,
                    AccountBalance = account.AccountBalance
                }
            };
        }



        public async Task<TokenModel> GenerateToken(UserResponse user)
        {
            var jwttokenhandler = new JwtSecurityTokenHandler();
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["appsettings:secretkey"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.Name, user.FullName + " " + user.Date_of_birth?.ToString("yyyy-MM-dd")),
                 new Claim(JwtRegisteredClaimNames.Email, user.Email),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(ClaimTypes.Role, user.Roles),
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
            var refreshtoken = await GenerateRefreshtoken();

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

            await _context.AddAsync(refreshtokenentity);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                Access_token = accesstoken,
                Refresh_token = refreshtoken
            };
        }

        public async Task<string> GenerateRefreshtoken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        public async Task<List<Account>> GetAllUsers()
        {
            var list = await _context.Accounts.ToListAsync();
            return list;
        }

        public async Task<bool> UpdateAvatar(string id, IFormFile file)
        {
            var user = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return false;
            }

            if (user.Avatar != null)
            {
                var delete = await imgLib.DeleteImage(user.Avatar);

                if (!delete)
                {
                    return false;
                }
            }

            var upload = await imgLib.UploadImage(file);

            if (!upload)
            {
                return false;
            }

            user.Avatar = file.FileName;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
