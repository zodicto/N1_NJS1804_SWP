using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ODTLearning.Entities;
using ODTLearning.Models;
using ODTLearning.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ODTLearning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly DbMiniCapStoneContext _context;

        public AccountController(IAccountRepository repo, IConfiguration configuration, DbMiniCapStoneContext context) 
        {
            _repo = repo;
            _configuration = configuration;
            _context = context;
        }
        [HttpPost("SignUp")]        
        public IActionResult SignUp(SignUpModel model)
        {
            var validation = _repo.SignUpValidation(model);

            if (validation != null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Sign up fail",
                    Data = validation
                });
            }

            var user = _repo.SignUp(model);

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
                Message = "Password and PasswordConfirm are not same"
            });                    
        }

        [HttpPost("SignIn")]
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

            var user = _repo.Authentication(model);

            if (user != null)
            {
                var token = _repo.GenerateToken(user);

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
                Message = "Invalid username or password"
            });
        }

        [HttpGet]
        public IActionResult GetAllUser()
        {
            var list = _repo.GetAllUser();

            if (list != null)
            {
                return Ok(list);
            }
            return BadRequest();
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
                var storedToken = _context.Users.FirstOrDefault(x => x.FirstName == model.RefreshToken); // NOT FINISH

                if (storedToken == null) // NOT FINISH
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: Check refreshToken is used/revoked?
                if (storedToken.FirstName == "a") // NOT FINISH
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been used"
                    });
                }

                if (storedToken.FirstName == "b") // NOT FINISH
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccesToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.FirstName != jti) // NOT FINISH
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                //Update token is used
                storedToken.FirstName = "true"; // NOT FINISH
                storedToken.LastName = "true"; // NOT FINISH
                _context.Update(storedToken);
                _context.SaveChanges();

                //Create new token
                var user = _context.Users.FirstOrDefault(x => x.Id == storedToken.Id); // NOT FINISH

                var token = _repo.GenerateToken(user);

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
            var dateTimeInterval = new DateTime(1970,1,1,0,0,0,0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }
    }
}
