using System.Globalization;
using System.Text.Json;
using CsvHelper;
using SquareAPI.Business.Models;
using SquareAPI.Data;
using SquareAPI.Data.Entities;

namespace SquareAPI.Business
{
    public interface ISquareService {
        Task<bool> AddUserPoints(IEnumerable<UserPoint> userPoints);
        Task DeleteUserPoints(IEnumerable<UserPoint> userPoints);
        Task<IEnumerable<UserPoint>> GetUserPoints(int userId);
        Task UpdateUserSquarePoints(int userId, SquarePoint squarePoint);
        Task<SquarePoint> GetUserSquarePoints(int userId);
        Task UpdateSquarePoints(int userId);
        SquarePoint CalculateSquare(IEnumerable<UserPoint> userPoints);
        IEnumerable<UserPoint> GetPointsFromStream(StreamReader stream);
    }

    /// <summary>
    /// Service to handle DB operations and calculations.
    /// </summary>
    public class SquareService : ISquareService
    {
        /// <summary>
        /// UserPoints DB repository.
        /// </summary>
        private readonly IUserPointsRepository _squareDataRepo;

        /// <summary>
        /// Square Service class constructor to initialize readonly.
        /// </summary>
        /// <param name="squareDataRepo">UserPoints DB repository injected object.</param>
        public SquareService(IUserPointsRepository squareDataRepo)
        {
            _squareDataRepo = squareDataRepo;
        }

        /// <summary>
        /// Add unique user points in DB.
        /// </summary>
        /// <param name="userId">Current user's Id.</param>
        /// <param name="userPoints">List of User co-ordinate points.</param>
        /// <returns>True if there is any new point to add.</returns>
        public async Task<bool> AddUserPoints(IEnumerable<UserPoint> userPoints)
        {
            var userId = userPoints.First().UserId;
            var existingPoints = await _squareDataRepo.GetUserPoints(userId);
            var newUserPoints = userPoints.Except(existingPoints.Distinct());

            if (newUserPoints.Any())
            {
                await _squareDataRepo.AddUserPoints(newUserPoints);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete user points from DB.
        /// </summary>
        /// <param name="userId">Current user's Id.</param>
        /// <param name="userPoints">List of User co-ordinate points to delete.</param>
        /// <returns></returns>
        public async Task DeleteUserPoints(IEnumerable<UserPoint> userPoints)
        {
            var userId = userPoints.First().UserId;
            await _squareDataRepo.DeleteUserPoints(userPoints);
        }

        /// <summary>
        /// Get user points from DB.
        /// </summary>
        /// <param name="userId">Current user's Id.</param>
        /// <returns>List of UserPoint object stored in DB.</returns>
        public async Task<IEnumerable<UserPoint>> GetUserPoints(int userId)
        {
            return await _squareDataRepo.GetUserPoints(userId);
        }

        /// <summary>
        /// Get UserSquarePoints data from DB.
        /// </summary>
        /// <param name="userId">Current user's Id.</param>
        /// <returns>SquarePoint object.</returns>
        public async Task<SquarePoint> GetUserSquarePoints(int userId)
        {
            string squarePointJson = await _squareDataRepo.GetUserSquarePointJson(userId);
            if (!string.IsNullOrEmpty(squarePointJson))
            {
                var squarePoint = JsonSerializer.Deserialize<SquarePoint>(squarePointJson);
                return squarePoint;
            }

            return null;
        }

        /// <summary>
        /// Update UserSquarePoints data in DB.
        /// </summary>
        /// <param name="userId">Current user's Id.</param>
        /// <param name="squarePoint">SquarePoint object to update data in DB.</param>
        /// <returns></returns>
        public async Task UpdateUserSquarePoints(int userId, SquarePoint squarePoint)
        {
            string squarePointJson = JsonSerializer.Serialize(squarePoint);
            await _squareDataRepo.UpdateUserSquarePoints(userId, squarePointJson);
        }        

        /// <summary>
        /// Recalculate and update Square point data in db after add/delete API operations.
        /// </summary>
        /// <param name="userId">Current user's Id.</param>
        /// <returns></returns>
        public async Task UpdateSquarePoints(int userId)
        {
            var userPoints = await _squareDataRepo.GetUserPoints(userId);
            var squarePoint = CalculateSquare(userPoints);
            await UpdateUserSquarePoints(userId, squarePoint);
        }

        /// <summary>
        /// Calculate square formed from User Points. 
        /// </summary>
        /// <param name="userId">User Id to get user points.</param>
        /// <returns>Square point object with square count and square co-ordinates.</returns>
        public SquarePoint CalculateSquare(IEnumerable<UserPoint> userPoints)
        {
            var squarePoint = new SquarePoint();
            IDictionary<string, Square> squares = new Dictionary<string, Square>();
            int squareCount = 0;
            if (userPoints != null && userPoints.Count() > 3)
            {
                // convert to dictionary with unique key
                IDictionary<string, UserPoint> dictUserPoint = userPoints.ToDictionary(x => x.Key);
                for (int i = 0; i < dictUserPoint.Count() - 1; i++)
                {
                    for (int j = i + 1; j < dictUserPoint.Count(); j++)
                    {
                        // to store all points whether x or y
                        int[] allPoints;

                        // calculate the vector perpendicular to the line between current two points
                        var a2 = dictUserPoint.ElementAt(j).Value;
                        var a1 = dictUserPoint.ElementAt(i).Value;
                        var vectorPoint = new UserPoint
                        {
                            X = a2.Y - a1.Y,
                            Y = -(a2.X - a1.X)
                        };

                        // predicated square position can be at below points
                        string predictedKeyA1 = (a1.X + vectorPoint.X) + "," + (a1.Y + vectorPoint.Y);
                        string predictedKeyA2 = (a2.X + vectorPoint.X) + "," + (a2.Y + vectorPoint.Y);
                        string predictedKeyB1 = (a1.X - vectorPoint.X) + "," + (a1.Y - vectorPoint.Y);
                        string predictedKeyB2 = (a2.X - vectorPoint.X) + "," + (a2.Y - vectorPoint.Y);

                        UserPoint p1 = null, p2 = null;

                        if (dictUserPoint.ContainsKey(predictedKeyA1) && dictUserPoint.ContainsKey(predictedKeyA2))
                        {
                            p1 = dictUserPoint[predictedKeyA1];
                            p2 = dictUserPoint[predictedKeyA2];
                        }
                        else if (dictUserPoint.ContainsKey(predictedKeyB1) && dictUserPoint.ContainsKey(predictedKeyB2))
                        {
                            p1 = dictUserPoint[predictedKeyB1];
                            p2 = dictUserPoint[predictedKeyB2];
                        }

                        // if there is key with predicated square co-ordinates on x-y plane then add in list with current co-ordinates
                        if (p1 != null && p2 != null)
                        {
                            // combine all x's and y's to get unique key for each square
                            allPoints = new int[8] { a1.X, a1.Y, a2.X, a2.Y, p1.X, p1.Y, p2.X, p2.Y };
                            var square = new Square
                            {
                                Points = new[] { a1, a2, p1, p2 }
                            };

                            var squareKey = string.Join(",", allPoints.OrderBy(x => x));

                            // if key is not there then add new square.
                            if (!squares.ContainsKey(squareKey))
                            {
                                squareCount++;
                                squares.Add(squareKey, square);
                            }
                        }
                    }
                }

                squarePoint.Count = squareCount;
                squarePoint.Square.AddRange(squares.Values);
            }

            return squarePoint;
        }

        /// <summary>
        /// Read csv points data from stream using CSVHelper.
        /// </summary>
        /// <param name="stream">Stream reader object holding file data.</param>
        /// <returns>List of UserPoints uploaded in file.</returns>
        public IEnumerable<UserPoint> GetPointsFromStream(StreamReader stream)
        {
            using (var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<UserPointMap>();
                var records = csv.GetRecords<UserPoint>().ToList();
                return records;
            }
        }
    }
}