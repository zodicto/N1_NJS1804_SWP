﻿using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ODTLearning.Models;
using ODTLearning.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ODTLearning.Entities;
using NuGet.Common;



namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class userController : ControllerBase
    {
        private readonly IAccountRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly DbminiCapstoneContext _context;
        private readonly ILogger<userController> _logger;

        public userController(IAccountRepository repo, IConfiguration configuration, DbminiCapstoneContext context, ILogger<userController> logger)
        {
            _repo = repo;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        [HttpPost("register")]
        public IActionResult RegisterOfAccount(SignUpModelOfAccount model)
        {
            try
            {
                var validation = _repo.SignUpValidationOfAccount(model);
                if (validation == null)
                {
                    return StatusCode(422, new ApiResponse
                    {
                        Success = false,
                        Message = "Email của bạn trùng với một email khác. Vui lòng thử lại!"
                    });
                }
                {
                    var user = _repo.SignUpOfAccount(model);
                    var token = _repo.generatetoken(user); 
                    return StatusCode(200, new ApiResponse
                    {
                        Success = true,
                        Message = "Đăng ký thành công!",
                        Data = new
                        {
                            User = user,
                            Token = token,
                        }
                    });
                }
<<<<<<< HEAD
=======

                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Email already exists"
                });
>>>>>>> 5bf4fe10fc7aa035a5a3914b65e7f2704ccce55a
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An internal server error occurred");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "Xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại sau."
                });
            }
        }



        [HttpPost("registerAsTutor")]
        public IActionResult SignUpOfTutor(string IDAccount, SignUpModelOfTutor model)
        {
            try
            {
                var user = _repo.SignUpOftutor(IDAccount, model);

                if (user != null)
                {
                    return StatusCode(200,new ApiResponse
                    {
                        Success = true,
                        Message = "Đăng ký trở thành gia sư thành công",
                        Data = user
                    });
                }

                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Sign up failed, user creation returned null"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while signing up as a tutor.");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An internal server error occurred. Please try again later."
                });
            }
        }


        [HttpPost("login")]
        public IActionResult SignIn(SignInModel model)
        {
            try
            {
                var user = _repo.SignInValidationOfAccount(model);
                if (user == null)
                {
                    return StatusCode(422, new ApiResponse
                    {
                        Success = false,
                        Message = "Email hoặc Password không đúng. Vui lòng thử lại!"
                    });
                }

                var token = _repo.generatetoken(user);
                if (token != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Đăng nhập thành công!",
                        Data = new
                        {
                            User = user,
                            Token = token,
                        }
                    });
                }

                // Thêm phần này để trả về một phản hồi nếu token là null
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "Xảy ra lỗi trong quá trình tạo token. Vui lòng thử lại sau!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An internal server error occurred");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "Xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại sau!"
                });
            }
        }





        [HttpPost("refreshToken")]
        public IActionResult RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]));

            var tokenValidateParam = new TokenValidationParameters
            {
                //tu cap token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ky vao token
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = securityKey,

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //khong kiem tra token het han
            };

            try
            {
                //check 1: Access Token valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.Access_Token, tokenValidateParam, out var validatedToken);

                //check 2: Check alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                //check 3: Check AccessToken expired?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);

                if (expireDate > DateTime.UtcNow)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshToken exist in DB
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.Refresh_Token);

                if (storedToken == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: Check refreshToken is used/revoked?
                if ((bool)storedToken.IsUsed)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }

                if ((bool)storedToken.IsRevoked)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccesToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                //Update token is used
                storedToken.IsUsed = true;
                storedToken.IsRevoked = true;
                _context.Update(storedToken);
                _context.SaveChanges();

                //Create new token
                var user = _context.Accounts.FirstOrDefault(x => x.Id == storedToken.IdAccount);

                var token = _repo.generatetoken(user);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Renew token",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }

        [HttpGet("getAllUser")]
        //[Authorize(Roles = "Student")]
        public IActionResult GetAllUser()
        {
            var list = _repo.getallusers();

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
                return BadRequest("Google authentication failed");

            var claims = authenticateResult.Principal.Identities
                .FirstOrDefault().Claims.Select(claim => new
                {
                    claim.Type,
                    claim.Value
                });

            return Ok(new
            {
                Message = "Google authentication successful",
                Claims = claims
            });


        }

        [HttpPost("logout")]
        public IActionResult Logout([FromBody] TokenModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Refresh_Token))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Refresh token is required"
                });
            }

            // Tìm Refresh Token trong cơ sở dữ liệu
            var refreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.Refresh_Token);

            if (refreshToken == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid refresh token"
                });
            }

            // Đánh dấu token là đã sử dụng và thu hồi
            refreshToken.IsUsed = true;
            refreshToken.IsRevoked = true;
            _context.RefreshTokens.Update(refreshToken);
            _context.SaveChanges();

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Logout successful"
            });
        }
    }
}
