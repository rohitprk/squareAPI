using Microsoft.Extensions.Configuration;
using SquareAPI.Business.Constants;
using SquareAPI.Data;
using SquareAPI.Data.Entities;

namespace SquareAPI.Business
{
    /// <summary>
    /// Interface to implement UserService methods.
    /// </summary>
    public interface IUserService {

        Task<Users> GetUser(string userName);
        Task<bool> Register(Users usersdata);
    }

    public class UserService : IUserService
    {
        /// <summary>
        /// UserRepository instance to read and write data to Users table.
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Symmetric key for AES algorithm.
        /// </summary>
        private readonly string _symmetricKey;

        /// <summary>
        /// JWT HMACSHA256 Algorithm key.
        /// </summary>
        private readonly string _jwtKey;

        /// <summary>
        /// Square Service class constructor to initialize readonly.
        /// </summary>
        /// <param name="squareDataRepo">UserPoints DB repository injected object.</param>
        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _symmetricKey = configuration[ApplicationConstant.SymmetricKeyConfig];
            _jwtKey = configuration[ApplicationConstant.JWTKeyConfig];
        }

        /// <summary>
        /// Get user data from db to validate user.
        /// </summary>
        /// <param name="usersdata">Instance of Users holding user data.</param>
        /// <returns>Instance of Users class.</returns>
        public async Task<Users> GetUser(string userName)
        {
            var user = await _userRepository.GetUser(userName);
            return user;
        }

        /// <summary>
        /// Register new user and add user data in DB.
        /// </summary>
        /// <param name="currentUser">Instance of Users holding user data.</param>
        /// <returns>True if there is no user with same username and password.</returns>
        public async Task<bool> Register(Users currentUser)
        {
            var user = await _userRepository.GetUser(currentUser.Name);
            if (user != null)
            {
                return false;
            }
            
            var password = currentUser.Password.AESDecrypt(_symmetricKey);
            currentUser.Password = password.GenerateHash(_jwtKey);
            await _userRepository.RegisterUser(currentUser);
            return true;
        }
    }
}