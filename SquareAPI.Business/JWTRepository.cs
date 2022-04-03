using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SquareAPI.Business.Constants;
using SquareAPI.Business.Models;
using SquareAPI.Data.Entities;

namespace SquareAPI.Business
{
    /// <summary>
    /// Interface to implement JWT methods.
    /// </summary>
    public interface IJWTRepository 
    {
        Task<string> Authenticate(Users users);
    }

/// <summary>
/// Class to generate and validate JWT.
/// </summary>
    public class JWTRepository : IJWTRepository
    {
        /// <summary>
        /// Instance of Configuration to read from configuration file.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Instance of UserService to read/write data from db.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Constructor to intantiate and initialize readonly variable.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userService"></param>
        public JWTRepository(IConfiguration configuration, IUserService userService)
        {
           _configuration = configuration;
            _userService = userService;
        }

        /// <summary>
        /// Authenticate user and generate token.
        /// </summary>
        /// <param name="currentUser">Instance of Users holding user data.</param>
        /// <returns>Generated Token.</returns>
        public async Task<string> Authenticate(Users currentUser)
        {
            var userData = await _userService.GetUser(currentUser.Name);
            if (userData is null)
            {
                return null;
            } else {          
                var password = currentUser.Password.AESDecrypt(_configuration[ApplicationConstant.SymmetricKeyConfig]);
                currentUser.Password = password.GenerateHash(_configuration[ApplicationConstant.JWTKeyConfig]);

                if (!userData.Password.Equals(currentUser.Password)) {
                    return null;
                }
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration[ApplicationConstant.JWTKeyConfig]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ApplicationConstant.UserIdClaim, userData.UserId.Value.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}