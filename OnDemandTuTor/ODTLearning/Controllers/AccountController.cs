using Microsoft.AspNetCore.Authentication.Google;
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
using ODTLearning.Entities;


namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly DbminiCapstoneContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountRepository repo, IConfiguration configuration, DbminiCapstoneContext context, ILogger<AccountController> logger)
        {
            _repo = repo;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        [HttpPost("Register")]
        public IActionResult RegisterOfAccount(SignUpModelOfAccount model)
        {
            try
            {
                var validation = _repo.SignUpValidationOfAccount(model);

                if (validation != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Sign up fail",
                        Data = validation
                    });
                }

                var user = _repo.SignUpOfAccount(model);

                if (user != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Sign up successfully",
                        Data = user
                    });
                }

                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred during the sign up process"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while signing up");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "An internal server error occurred"
                });
            }
        }

        [HttpPost("RegisterAsTuTor")]
        public IActionResult SignUpOfTutor(string IDAccount, SignUpModelOfTutor model)
        {
            var validation = _repo.SignUpValidationOfTutor(model);

            if (validation != null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Sign up failed",
                    Data = validation
                });
            }

            var user = _repo.SignUpOftutor(IDAccount, model);

            if (user != null)
            {
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Sign up successfully",
                    Data = user
                });
            }

            return Ok(new ApiResponse
            {
                Success = false,
                Message = "Sign up failed, user creation returned null"
            });
        }


        [HttpPost("LogIn")]
        public IActionResult SignIn(SignInModel model)
        {
            var validation = _repo.SignInValidation(model);

            if (validation != null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Sign in fail",
                    Data = validation
                });
            }
            var user = _repo.authentication(model);

            if (user != null)
            {
                var token = _repo.generatetoken(user);

                if (token != null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Sign in successfully",
                        Data = token
                    });
                }
            }

            return Ok(new ApiResponse
            {
                Success = false,
                Message = "Invalid email or password"
            });

        }

        [HttpPost("RenewToken")]
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
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

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
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshToken exist in DB
                var storedToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);

                if (storedToken == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: Check refreshToken is used/revoked?
                if ((bool)storedToken.IsUsed)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }

                if ((bool)storedToken.IsRevoked)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccesToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return Ok(new ApiResponse
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

        [HttpGet("GetAllUser")]
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

        [HttpPost("Logout")]
        public IActionResult Logout([FromBody] TokenModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.RefreshToken))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Refresh token is required"
                });
            }

            // Tìm Refresh Token trong cơ sở dữ liệu
            var refreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);

            if (refreshToken == null)
            {
                return Ok(new ApiResponse
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
