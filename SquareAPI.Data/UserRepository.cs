using Dapper;
using SquareAPI.Data.Entities;

namespace SquareAPI.Data
{
    /// <summary>
    /// User repository interface declaring all operation on Users table. 
    /// </summary>
    public interface IUserRepository
    {
        public Task<Users> GetUser(string userName);
        public Task RegisterUser(Users user);
    }

    /// <summary>
    /// Repository class for DB operation on Users table.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// SquareAPI database context instance.
        /// </summary>
        private readonly SquareAPIContext _context;

        /// <summary>
        /// Constructor to initialize readonly variables.
        /// </summary>
        /// <param name="context"></param>
        public UserRepository(SquareAPIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get specific user data from Users table.
        /// </summary>
        /// <param name="userName">User name to fetch user data.</param>
        /// <returns>Instance of Users class.</returns>
        public async Task<Users> GetUser(string userName)
        {
            var query = "SELECT Id AS UserId, Name, Password FROM Users WHERE LOWER(Name) = @Name";
            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<Users>(query, new { Name = userName.ToLower() });
                return users.SingleOrDefault();
            }
        }

        /// <summary>
        /// Insert new user in Users table.
        /// </summary>
        /// <param name="user">Instance of Users class.</param>
        /// <returns></returns>
        public async Task RegisterUser(Users user)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = "INSERT INTO Users(Name, Password) VALUES(@Name, @Password)";
                await connection.ExecuteAsync(query, user);
            }
        }
    }
}