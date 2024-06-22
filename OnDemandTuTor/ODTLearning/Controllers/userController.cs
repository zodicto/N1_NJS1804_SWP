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
using Microsoft.EntityFrameworkCore;
using ODTLearning.Entities;
using System;

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
        public IActionResult SignUp(SignUpModelOfAccount model)
        {
            try
            {
                if (_repo.IsEmailExist(model.Email))
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

                var user = _repo.SignUpOfAccount(model);
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
                var token = _repo.GenerateToken(user);
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
                _logger.LogError(ex, "An internal server error occurred");
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

        [HttpPost("registerAsTutor")]
        public IActionResult SignUpOfTutor(string IDAccount, [FromForm] SignUpModelOfTutor model)
        {
            try
            {
                var user = _repo.SignUpOftutor(IDAccount, model);

                if (user != null)
                {
                    return StatusCode(200, new
                    {
                        Success = true,
                        user.Message,
                        user.Data,
                    });
                }

                return BadRequest(new
                {
                    Success = false,
                    Message = "Sign up failed, user creation returned null"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while signing up as a tutor.");
                return StatusCode(500, new
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
                var response = _repo.SignInValidationOfAccount(model);
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

                var token = _repo.GenerateToken(response.Data);
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
                _logger.LogError(ex, "An internal server error occurred");
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

        [HttpPost("refreshToken")]
        public IActionResult RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]));

            var tokenValidateParam = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = securityKey,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = false
            };

            try
            {
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.Access_token, tokenValidateParam, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)
                    {
                        return Ok(new
                        {
                            Success = false,
                            Message = "Invalid token"
                        });
                    }
                }

                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);

                if (expireDate > DateTime.UtcNow)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });
                }

                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.Refresh_token);

                //if (storedToken == null)
                //{
                //    return BadRequest(new
                //    {
                //        Success = false,
                //        Message = "Refresh token does not exist"
                //    });
                //}

                //if (storedToken.IsUsed)
                //{
                //    return BadRequest(new
                //    {
                //        Success = false,
                //        Message = "Refresh token has been used"
                //    });
                //}

                //if (storedToken.IsRevoked)
                //{
                //    return BadRequest(new
                //    {
                //        Success = false,
                //        Message = "Refresh token has been revoked"
                //    });
                //}

                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                storedToken.IsUsed = true;
                storedToken.IsRevoked = true;
                _context.Update(storedToken);
                _context.SaveChanges();

                var userAccount = _context.Accounts.FirstOrDefault(x => x.Id == storedToken.IdAccount);

                if (userAccount == null)
                {
                    return NotFound();
                }

                var userResponse = new UserResponse
                {
                    Id = userAccount.Id,
                    FullName = userAccount.FullName,
                    Email = userAccount.Email,
                    Date_of_birth = userAccount.DateOfBirth,
                    Gender = userAccount.Gender,
                    Roles = userAccount.Roles,
                    Avatar = userAccount.Avatar,
                    Address = userAccount.Address,
                    Phone = userAccount.Phone,
                    AccountBalance = userAccount.AccountBalance
                };

                var token = _repo.GenerateToken(userResponse);

                return Ok(new
                {
                    Success = true,
                    Message = "Renew token",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
        }

        [HttpGet("getAllUser")]
        public IActionResult GetAllUser()
        {
            var list = _repo.GetAllUsers();

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
        public IActionResult GoogleCallback()
        {
            var authenticateResult = HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme).Result;

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
        public IActionResult Logout([FromBody] LogoutModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Refresh_token))
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Refresh token is required"
                });
            }

            var refreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.Refresh_token);

            if (refreshToken == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Invalid refresh token"
                });
            }

            refreshToken.IsUsed = true;
            refreshToken.IsRevoked = true;
            _context.RefreshTokens.Update(refreshToken);
            _context.SaveChanges();

            return Ok(new
            {
                Success = true,
                Message = "Logout successful"
            });
        }

        [HttpPut("UpdateAvatar")]
        public IActionResult UpdateAvatar(string id, IFormFile file)
        {
            var result = _repo.UpdateAvatar(id, file);

            if (result)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Update avatar successful"
                });
            }

            return BadRequest(new
            {
                Success = false,
                Message = "Update avatar fail"
            });
        }

        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(string id, ChangePasswordModel model)
        {
            var result = _repo.ChangePassword(id, model);

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
        public IActionResult ForgotPassword(string Email)
        {
            var result = _repo.ForgotPassword(Email);

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
        public IActionResult UpdateStudentProfile(string id, [FromBody] UpdateProfile model)
        {
            try
            {
                var response = _repo.UpdateProfile(id, model);

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
                    response.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An internal server error occurred");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An internal server error occurred. Please try again later."
                });
            }
        }

        [HttpGet("getProfile")]
        public IActionResult GetProfile(string id)
        {
            try
            {
                var response = _repo.GetProfile(id);

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
                _logger.LogError(ex, "An internal server error occurred");
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An internal server error occurred. Please try again later."
                });
            }
        }
    }
}
