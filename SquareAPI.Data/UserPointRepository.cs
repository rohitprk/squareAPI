using Dapper;
using SquareAPI.Data.Entities;

namespace SquareAPI.Data
{
    /// <summary>
    /// User Point repository interface declaring all operation on UserPoints table. 
    /// </summary>
    public interface IUserPointsRepository
    {
        Task<IEnumerable<UserPoint>> GetUserPoints(int userId);
        Task AddUserPoints(IEnumerable<UserPoint> userPoints);
        Task DeleteUserPoints(IEnumerable<UserPoint> userPoints);
        Task UpdateUserSquarePoints(int userId, string squarePointJson);
        Task<string> GetUserSquarePointJson(int userId);
    }

    /// <summary>
    /// Repository class for DB operation on UserPoints table.
    /// </summary>
    public class UserPointsRepository : IUserPointsRepository
    {
        /// <summary>
        /// SquareAPI database context instance.
        /// </summary>
        private readonly SquareAPIContext _context;

        /// <summary>
        /// Constructor to initialize readonly variables.
        /// </summary>
        /// <param name="context"></param>
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
                string query = 
                    @"INSERT INTO UserPoints(UserId, x, y) VALUES(@UserId, @x, @y);
                    UPDATE UserSquarePoints SET LastUpdateTime = NULL WHERE UserId = @UserId";
                await connection.ExecuteAsync(query, userPoints);
            }
        }

        /// <summary>
        /// Get user points from DB.
        /// </summary>
        /// <param name="userId">User id to fetch points.</param>
        /// <returns>List of UserPoints object.</returns>
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
                string query = 
                @"DELETE FROM UserPoints WHERE UserId = @UserId AND x = @x AND y = @y;
                UPDATE UserSquarePoints SET LastUpdateTime = NULL WHERE UserId = @UserId";
                await connection.ExecuteAsync(query, userPoints);
            }

        }

        /// <summary>
        /// Get UserSquarePoints data from DB.
        /// </summary>
        /// <param name="userId">Current user's Id.</param>
        /// <returns>UserSquarePoints Json formatted string.</returns>
        public async Task<string> GetUserSquarePointJson (int userId)
        {
            var query = "SELECT SquarePointJson FROM UserSquarePoints WHERE UserId = @UserId AND LastUpdateTime IS NOT NULL";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<string>(query, new { UserId = userId });
                return result.FirstOrDefault();
            }
        }

        /// <summary>
        /// Insert/update UserSquarePoints data in DB.
        /// </summary>
        /// <param name="userId">Current user's Id.</param>
        /// <param name="squarePointJson">UserSquarePoints Json formatted string to update.</param>
        /// <returns></returns>
        public async Task UpdateUserSquarePoints(int userId, string squarePointJson)
        {
            using (var connection = _context.CreateConnection())
            {
                string query = 
                @"IF NOT EXISTS(SELECT 1 FROM UserSquarePoints WHERE UserId = @UserId)
                    INSERT INTO UserSquarePoints(UserId, SquarePointJson, LastUpdateTime) VALUES(@UserId, @SquarePointJson, GETDATE())
                ELSE
                    UPDATE UserSquarePoints
                        SET SquarePointJson = @SquarePointJson,
                            LastUpdateTime = GETDATE()
                    WHERE UserId = @UserId";
                await connection.ExecuteAsync(query, new { UserId = userId, SquarePointJson = squarePointJson});
            }
        }
    }
}