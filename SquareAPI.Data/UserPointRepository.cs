using Dapper;
using SquareAPI.Data.Entities;

namespace SquareAPI.Data
{
    public interface IUserPointsRepository
    {
        public Task<IEnumerable<UserPoint>> GetUserPoints(int userId);
        public Task AddUserPoints(IEnumerable<UserPoint> userPoints);
        public Task DeleteUserPoints(IEnumerable<UserPoint> userPoints);
    }

    /// <summary>
    /// Repository class for DB operation on UserPoints table.
    /// </summary>
    public class UserPointsRepository : IUserPointsRepository
    {
        private readonly SquareAPIContext _context;

        public UserPointsRepository(SquareAPIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add user points to DB.
        /// </summary>
        /// <param name="userId">User id to save points.</param>
        /// <param name="userPoints">Co-ordinate data to save.</param>
        public async Task AddUserPoints(IEnumerable<UserPoint> userPoints)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = "INSERT INTO UserPoints(UserId, x, y) VALUES(@UserId, @x, @y)";
                await connection.ExecuteAsync(query, userPoints);
            }
        }

        /// <summary>
        /// Get user points from DB.
        /// </summary>
        /// <param name="userId">User id to fetch points.</param>
        /// <returns><see cref="IEnumerable<UserPoints>">List of UserPoints</see> object.</returns>
        public async Task<IEnumerable<UserPoint>> GetUserPoints(int userId)
        {
            var query = "SELECT UserId, X, Y FROM UserPoints WHERE UserId = @UserId";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<UserPoint>(query, new { UserId = userId });
            }
        }


        /// <summary>
        /// Delete user points to DB.
        /// </summary>
        /// <param name="userId">User id to delete points.</param>
        /// <param name="userPoints">Co-ordinate data to delete.</param>
        public async Task DeleteUserPoints(IEnumerable<UserPoint> userPoints)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = "DELETE FROM UserPoints WHERE UserId = @UserId AND x = @x AND y = @y";
                await connection.ExecuteAsync(query, userPoints);
            }

        }
    }
}