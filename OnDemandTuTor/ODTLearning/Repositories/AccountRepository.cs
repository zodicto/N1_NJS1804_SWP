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

        public SignUpValidationModel SignUpValidation(SignUpModel model)
        {
            string username = "", password = "", passwordConfirm = "", firstname = "", lastname = "";
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

            if (i != 0)
            {
                return new SignUpValidationModel
                {
                    Username = username,
                    Password = password,
                    PasswordConfirm = passwordConfirm,
                    FirstName = firstname,
                    LastName = lastname,
                };
            }

            return null;
        }

        public object SignUp(SignUpModel model)
        {
            if (model.Password != model.PasswordConfirm)
            {
                return null;
            }

            var user = new Account
            {
                IdAccount = model.Id,
                FisrtName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Password = model.Password
            };

            _context.Add(user);
            _context.SaveChanges();

            return user;
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

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FisrtName + "" + user.LastName),
                new Claim(JwtRegisteredClaimNames.Email, user.Gmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.FisrtName),
                new Claim(JwtRegisteredClaimNames.Gender, user.FisrtName),
                new Claim("Id", user.IdAccount )
            };

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

        private string GenerateRefreshToken()
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
