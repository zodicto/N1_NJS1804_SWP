using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ODTLearning.BLL.Repositories;
using ODTLearning.DAL.Entities;
using ODTLearning.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ODTLearning.BLL.Models;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AccountRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly DbminiCapstoneContext _context;


        public AccountController(AccountRepository repo, IConfiguration configuration, DbminiCapstoneContext context)
        {
            _repo = repo;
            _configuration = configuration;
            _context = context;   
        }

        [HttpPost("register")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModelOfAccount model)
        {
            try
            {
                if (await _repo.IsEmailExist(model.email))
                {
                    return StatusCode(422, new
                    {
                        message = "Lỗi",
                        data = new
                        {
                            email = "Email đã tồn tại"
                        }
                    });
                }

                var user = await _repo.SignUpOfAccount(model);
                if (user == null)
                {
                    return StatusCode(500, new
                    {
                        message = "Lỗi",
                        data = new
                        {
                            error = "Xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại sau!"
                        }
                    });
                }
                var token = await _repo.GenerateToken(user);
                return StatusCode(200, new
                {
                    message = "Đăng ký thành công!",
                    data = new
                    {
                        user,
                        token.Access_token,
                        token.Refresh_token,
                    }

                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi",
                    data = new
                    {
                        error = "Xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại sau!"
                    }
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel model)
        {
            try
            {
                var response = await _repo.SignInValidationOfAccount(model);
                if (!response.Success)
                {
                    return StatusCode(422, new
                    {
                        message = "Lỗi",
                        data = new
                        {
                            password = response.Message
                        }
                    });
                }

                var token = await _repo.GenerateToken(response.Data);
                if (token != null)
                {
                    return StatusCode(200, new
                    {
                        message = "Đăng nhập thành công!",
                        data = new
                        {
                            User = response.Data,
                            token.Refresh_token,
                            token.Access_token,
                        }
                    });
                }

                // Trả về phản hồi nếu token là null
                return StatusCode(500, new
                {
                    message = "Lỗi",
                    data = new
                    {
                        error = "Xảy ra lỗi trong quá trình tạo token. Vui lòng thử lại sau!"
                    }
                });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new
                {
                    message = "Lỗi",
                    data = new
                    {
                        error = "Xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại sau!"
                    }
                });
            }
        }

      

        [HttpGet("getAllUser")]
        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAllUser()
        {
            var list = await _repo.GetAllUsers();

            if (list != null)
            {
                return Ok(list);
            }
            return BadRequest();
        }

        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded)
                return StatusCode(422, new
                {
                    message = "Google authentication failed",
                    data = new { error = "Google authentication failed" }
                });

            var claims = authenticateResult.Principal.Identities
                .FirstOrDefault().Claims.ToList();

            var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var userEmail = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userAvatar = claims.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;

            var user = new UserGG
            {
                id = userId,
                fullName = userName,
                email = userEmail,
                roles = "học sinh",
                avatar = userAvatar,
            };

            var result = await _repo.SaveGoogleUserAsync(user);

            if (!result.Success)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi",
                    data = new { error = result.Message }
                });
            }

            var savedUser = result.Data;

            // Generate token
            var token = await _repo.GenerateToken(savedUser);

            // Send data back to the front-end using postMessage
            var script = $@"
<script>
    window.opener.postMessage(
        {{
            profile: JSON.stringify({{
                id: '{savedUser.id}',
                fullName: '{System.Text.Encodings.Web.JavaScriptEncoder.Default.Encode(savedUser.fullName)}',
                email: '{System.Text.Encodings.Web.JavaScriptEncoder.Default.Encode(savedUser.email)}',
                avatar: '{System.Text.Encodings.Web.JavaScriptEncoder.Default.Encode(savedUser.avatar)}',
                accountBalance: '{savedUser.accountBalance?.ToString() ?? "0"}',
                roles: '{System.Text.Encodings.Web.JavaScriptEncoder.Default.Encode(savedUser.roles)}'
            }}),
            accessToken: '{token.Access_token}',
            refreshToken: '{token.Refresh_token}'
        }},
        'http://localhost:3000'
    );
    window.close();
</script>";
            return Content(script, "text/html");
        }

        [HttpGet("getProfile")]
        public async Task<IActionResult> GetProfile(string id)
        {
            try
            {
                var response = await _repo.GetProfile(id);

                if (!response.Success)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        response.Message
                    });
                }

                return Ok(new
                {
                    Success = true,
                    response.Message,
                    response.Data
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An internal server error occurred. Please try again later."
                });
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Refresh_token))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Refresh token is required"
                });
            }

            // Tìm Refresh Token trong cơ sở dữ liệu
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == model.Refresh_token);

            if (refreshToken == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Invalid refresh token"
                });
            }

            // Đánh dấu token là đã sử dụng và thu hồi
            refreshToken.IsUsed = true;
            refreshToken.IsRevoked = true;
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Logout successful"
            });
        }

        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id, ChangePasswordModel model)
        {
            var result = await _repo.ChangePassword(id, model);

            if (result == "Thay đổi mật khẩu thành công")
            {
                return Ok(new
                {
                    Success = true,
                    Message = result
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = result
            });
        }

        [HttpPut("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            var result = await _repo.ForgotPassword(Email);

            if (result == "Gửi mật khẩu mới thành công")
            {
                return Ok(new
                {
                    Success = true,
                    Message = result
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = result
            });
        }

        [HttpPut("updateProfile")]
        [Authorize]
        public async Task<IActionResult> UpdateStudentProfile(string id, [FromBody] UpdateProfile model)
        {
            try
            {
                var response = await _repo.UpdateProfile(id, model);

                if (!response.Success)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        response.Message
                    });
                }

                return Ok(new
                {
                    Success = true,
                    response.Message,
                    response.Data
                });
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An internal server error occurred. Please try again later."
                });
            }
        }
       

        [HttpDelete("DeleteAccount")]
        [Authorize(Roles = UserRoleAuthorize.Admin)]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var response = await _repo.DeleteAccount(id);

            if (response.Success)
            {
                return Ok(new
                {
                    Success = true,
                    Message = response.Message
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = response.Message
            });
        }
    }
}
