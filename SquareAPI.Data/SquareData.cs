using Dapper;

namespace SquareAPI.Data
{
    public interface IUserPointsRepository
    {
        public Task<IEnumerable<UserPoints>> GetUserPoints(int userId);
        public Task AddUserPoints(int userId, IEnumerable<UserPoints> userPoints);
        public Task DeleteUserPoints(int userId, IEnumerable<UserPoints> userPoints);
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
        public async Task AddUserPoints(int userId, IEnumerable<UserPoints> userPoints)
        {
            var existingPoints = await GetUserPoints(userId);
            var newUserPoints = userPoints.Except(existingPoints);

            if (newUserPoints.Any())
            {
                try
                {
                    using (var connection = _context.CreateConnection())
                    {
                        string query = "INSERT INTO UserPoints(UserId, x, y) VALUES(@UserId, @x, @y)";
                        await connection.ExecuteAsync(query, newUserPoints);
                    }

                }
                catch (Exception e)
                {
                    // can add sql exception logging here
                    throw;
                }
            }
        }

        /// <summary>
        /// Get user points from DB.
        /// </summary>
        /// <param name="userId">User id to fetch points.</param>
        /// <returns><see cref="IEnumerable<UserPoints>">List of UserPoints</see> object.</returns>
        public async Task<IEnumerable<UserPoints>> GetUserPoints(int userId)
        {
            var query = "SELECT * FROM UserPoints WHERE UserId = @UserId";
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<UserPoints>(query, new { UserId = userId });
                }
            }
            catch (Exception e)
            {
                // can add sql exception logging here
                throw;
            }
        }


        /// <summary>
        /// Delete user points to DB.
        /// </summary>
        /// <param name="userId">User id to delete points.</param>
        /// <param name="userPoints">Co-ordinate data to delete.</param>
        public async Task DeleteUserPoints(int userId, IEnumerable<UserPoints> userPoints)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    string query = "DELETE FROM UserPoints WHERE UserId = @UserId AND x = @x AND y = @y";
                    await connection.ExecuteAsync(query, userPoints);
                }
            }
            catch (Exception e)
            {
                // can add sql exception logging here
                throw;
            }

        }
    }
}