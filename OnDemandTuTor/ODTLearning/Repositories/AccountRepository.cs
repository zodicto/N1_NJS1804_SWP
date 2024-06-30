using Aqua.EnumerableExtensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ODTLearning.Entities;
using ODTLearning.Helpers;
using ODTLearning.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
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
        EmailLibrary emailLib = new EmailLibrary();


        public async Task<bool> IsEmailExist(string email)
        {
            return await _context.Accounts.AnyAsync(a => a.Email == email);
        }

        public async Task<UserResponse> SignUpOfAccount(SignUpModelOfAccount model)
        {

            var user = new Account
            {
                Id = Guid.NewGuid().ToString(),
                FullName = model.fullname,
                Password = model.password,
                Phone = model.phone,
                AccountBalance = 0,
                Email = model.email,
                DateOfBirth = model.date_of_birth,
                Gender = model.gender,
                Roles = "Học sinh"
            };
            // Thêm Account vào context
            await _context.Accounts.AddAsync(user);
            await _context.SaveChangesAsync();
            return new UserResponse
            {
                id = user.Id,
                fullname = user.FullName,
                email = user.Email,
                date_of_birth = user.DateOfBirth,
                gender = user.Gender,
                roles  = user.Roles,
                avatar  = user.Avatar,
                address = user.Address,
                phone = user.Phone,
                accountbalance= user.AccountBalance
            };

        }

        public async Task<ApiResponse<TutorResponse>> SignUpOftutor(string IdAccount, SignUpModelOfTutor model)
        {
            // Tìm kiếm account trong DB bằng id
            var existingUser = _context.Accounts.FirstOrDefault(a => a.Id == IdAccount);
            if (existingUser != null)
            {
                // Tạo mới đối tượng tutor
                var tutor = new Tutor
                {
                    Id = Guid.NewGuid().ToString(),
                    SpecializedSkills = model.specializedSkills,
                    Experience = model.experience,
                    Status = "Chưa duyệt",
                    IdAccount = existingUser.Id,
                    Introduction = model.introduction,
                   
                };

                // Tạo mới đối tượng educationalqualification
                var educationalQualification = new EducationalQualification
                {
                    Id = Guid.NewGuid().ToString(),
                    IdTutor = tutor.Id,
                    QualificationName = model.qualificationame,
                    Type = model.type,
                };

                // Upload ảnh
                var upload = await imgLib.UploadImage
                    (model.imagequalification);

                if (!upload.Success)
                {
                    return new ApiResponse<TutorResponse>
                    {
                        Success = false,
                        Message = upload.Message
                    };                    
                }

                educationalQualification.Img = model.imagequalification.FileName;

                var subjectModel = await _context.Subjects
                                              .FirstOrDefaultAsync(lm => lm.SubjectName == model.subject);

                if (subjectModel == null)
                {
                    return new ApiResponse<TutorResponse>
                    {
                        Success = false,
                        Message = "Không tìm thấy môn học nào với tên này",
                        Data = null
                    };
                }

                // Tạo mới đối tượng TutorSubject
                var tutorSubject = new TutorSubject
                {
                    Id = Guid.NewGuid().ToString(),
                    IdSubject = subjectModel.Id,
                    IdTutor = tutor.Id,
                };

                // Thêm các đối tượng vào DB
                await _context.Tutors.AddAsync(tutor);
                await _context.EducationalQualifications.AddAsync(educationalQualification);
                await _context.TutorSubjects.AddAsync(tutorSubject);

                try
                {
                    await _context.SaveChangesAsync();
                    // Trả về ID của tutor đã được tạo với tên là idTutor
                    return new ApiResponse<TutorResponse>
                    {
                        Success = true,
                        Message = "Đăng ký gia sư thành công. Bạn vui lòng chờ duyệt",
                        Data = new TutorResponse { Idtutor = tutor.Id }
                    };
                }
                catch (Exception ex)
                {
                    // Ghi lại lỗi nếu có xảy ra
                    Console.WriteLine($"Error while saving changes: {ex.Message}");
                    return new ApiResponse<TutorResponse>
                    {
                        Success = false,
                        Message = "Đã xảy ra lỗi trong quá trình lưu dữ liệu",
                        Data = null
                    };
                }
            }

            // Trường hợp không tìm thấy tài khoản
            return new ApiResponse<TutorResponse>
            {
                Success = false,
                Message = "Không tìm thấy tài khoản với ID này",
                Data = null
            };
        }

        public async Task<ApiResponse<TutorResponse>> SignUpOftutorFB(string IdAccount, SignUpModelOfTutorFB model)
        {
            // Tìm kiếm account trong DB bằng id
            var existingUser = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == IdAccount);
            if (existingUser == null)
            {
                // Trường hợp không tìm thấy tài khoản
                return new ApiResponse<TutorResponse>
                {
                    Success = false,
                    Message = "Không tìm thấy tài khoản với ID này",
                };
            }

            // Tạo mới đối tượng tutor
            var tutor = new Tutor
            {
                Id = Guid.NewGuid().ToString(),
                SpecializedSkills = model.specializedskills,
                Experience = model.experience,
                Status = "Chưa duyệt",
                IdAccount = existingUser.Id,
                Introduction = model.introduction,
            };

            // Tạo mới đối tượng educationalqualification
            var educationalQualification = new EducationalQualification
            {
                Id = Guid.NewGuid().ToString(),
                IdTutor = tutor.Id,
                QualificationName = model.qualificationname,
                Type = model.type,
                Img = model.imagequalification
                
            };

            // Tìm môn học theo tên
            var subjectModel = await _context.Subjects.FirstOrDefaultAsync(lm => lm.SubjectName == model.subject);
            if (subjectModel == null)
            {
                return new ApiResponse<TutorResponse>
                {
                    Success = false,
                    Message = "Không tìm thấy môn học nào với tên này. Vui lòng thử lại!",
                };
            }

            // Tạo mới đối tượng TutorSubject
            var tutorSubject = new TutorSubject
            {
                Id = Guid.NewGuid().ToString(),
                IdSubject = subjectModel.Id,
                IdTutor = tutor.Id,
            };

            // Thêm các đối tượng vào DB

            await _context.Tutors.AddAsync(tutor);
            await _context.EducationalQualifications.AddAsync(educationalQualification);
            await _context.TutorSubjects.AddAsync(tutorSubject);

            try
            {
                await _context.SaveChangesAsync();
                // Trả về ID của tutor đã được tạo với tên là idTutor
                return new ApiResponse<TutorResponse>
                {
                    Success = true,
                    Message = "Đăng ký gia sư thành công. Bạn vui lòng chờ duyệt",
                };
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu có xảy ra
                Console.WriteLine($"Error while saving changes: {ex.Message}");
                return new ApiResponse<TutorResponse>
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi trong quá trình lưu dữ liệu",
                    Data = null
                };
            }
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
                    id = account.Id,
                    fullname= account.FullName,
                    email= account.Email,
                    date_of_birth = account.DateOfBirth,
                    gender = account.Gender,
                    roles = account.Roles,
                    avatar = account.Avatar,
                    address = account.Address,
                    phone = account.Phone,
                    accountbalance= account.AccountBalance
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
                 new Claim(ClaimTypes.Name, user.fullname  + " " + user.date_of_birth?.ToString("yyyy-MM-dd")),
                 new Claim(JwtRegisteredClaimNames.Email, user.email ),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(ClaimTypes.Role, user.roles),
                 new Claim("id", user.id )

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
                IdAccount = user.id,
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


        public async Task<ApiResponse<object>> UpdateAvatar(string id, IFormFile file)

        {
            var response = new ApiResponse<bool>();

            var user = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {

                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng nào với ID này"
                }; 

            }

            if (user.Avatar != null)
            {
                var delete = await imgLib.DeleteImage(user.Avatar);
                if (!delete)
                {

                    return new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Xoá avatar cũ thất bại"
                    }; 

                }
            }

            var upload = await imgLib.UploadImage(file);

            if (!upload.Success)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = upload.Message
                }; 

            }

            user.Avatar = file.FileName;
            await _context.SaveChangesAsync();

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Thành công"
            }; 

        }





        public async Task<string> ChangePassword(string id, ChangePasswordModel model)
        {
            var user = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return "Không tìm thấy người dùng";
            }

            if (model.Password != user.Password)
            {
                return "Không đúng mật khẩu";
            }

            user.Password = model.New_password;
            await _context.SaveChangesAsync();
            return "Thay đổi mật khẩu thành công";
        }

        public async Task<string> ForgotPassword(string Email)
        {
            try
            {
                var user = await _context.Accounts.SingleOrDefaultAsync(x => x.Email == Email);
                
                if (user == null)
                {
                    return "Email không tồn tại trong hệ thống";
                }

                var password = new Random().Next(100000, 999999);

                var result = await emailLib.SendMail("ODTLearning", "Lấy lại mật khẩu", $"Mật khẩu mới là: {password}", Email);

                if (!result)
                {
                    return "Gửi mail không thành công";
                }

                user.Password = password.ToString();
                await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                return "Đã có một số lỗi xảy ra: " + ex.Message;
            }

            return "Gửi mật khẩu mới thành công";
        }
        public async Task<ApiResponse<object>> UpdateProfile(string id, UpdateProfile model)
        {
            var user = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng nào với ID này",
                };
            }

            user.FullName = model.fullName;
            user.DateOfBirth = model.date_of_birth;
            user.Gender = model.gender;
            user.Avatar = model.avatar;
            user.Address = model.address;
            user.Phone = model.phone;

            await _context.SaveChangesAsync();

            var updatedUserResponse = new
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Date_of_birth = user.DateOfBirth,
                Gender = user.Gender,
                Avatar = user.Avatar,
                Address = user.Address,
                Phone = user.Phone,
                Accountbalance = user.AccountBalance,
                Roles = user.Roles
            };

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Cập nhật thông tin người dùng thành công",
                Data = updatedUserResponse
            };
        }




        public async Task<ApiResponse<object>> GetProfile(string id)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id);

            if (account == null)
            {
                return new ApiResponse<object>
                {
                    Success = false,
                    Message = "Không tìm thấy người dùng nào với ID này",
                };
            }

            var userProfile = new
            {
                Id = account.Id,
                Email = account.Email,
                FullName = account.FullName,
                Date_of_birth = account.DateOfBirth,
                Gender = account.Gender,
                Avatar = account.Avatar,
                Address = account.Address,
                Phone = account.Phone,
                Accountbalance = account.AccountBalance,
                Roles = account.Roles
            };

            return new ApiResponse<object>
            {
                Success = true,
                Message = "Lấy thông tin người dùng thành công",
                Data = userProfile
            };
        }



        public async Task<ApiResponse<UserResponse>> SaveGoogleUserAsync(UserResponse user)
        {
            var existingUser = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == user.email);

            if (existingUser == null)
            {
                // Người dùng chưa tồn tại, tạo người dùng mới
                var newUser = new Account
                {
                    Id = user.id,
                    FullName = user.fullname,
                    Email = user.email ,
                    Roles = user.roles,
                    AccountBalance = 0, // Đặt AccountBalance bằng 0
                    Avatar = user.avatar   // Gán URL của ảnh đại diện vào thuộc tính Avatar
                };

                await _context.Accounts.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return new ApiResponse<UserResponse>
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = user
                };
            }
            else
            {
                // Người dùng đã tồn tại, cập nhật thông tin người dùng
                existingUser.FullName = user.fullname;
                existingUser.Roles = user.roles; // Cập nhật roles nếu cần, hoặc có thể bỏ qua nếu không muốn cập nhật
                existingUser.Avatar = user.avatar ; // Cập nhật URL của ảnh đại diện nếu cần
                await _context.SaveChangesAsync();

                return new ApiResponse<UserResponse>
                {
                    Success = true,
                    Message = "User already exists and has been updated",
                    Data = user
                };
            }
        }




    }
}