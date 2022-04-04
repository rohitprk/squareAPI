using System.ComponentModel.DataAnnotations;
using SquareAPI.Data.Entities;

namespace SquareAPI.Web.Models.v1
{
    /// <summary>
    /// Class to deserialize user entered points data from api request.
    /// </summary>
    public class UserInput
    {
        /// <summary>
        /// Constructor to initialize property.
        /// </summary>
        public UserInput()
        {
            Points = new List<UserPoint>();
        }

        /// <summary>
        /// Co-ordinate points data.
        /// </summary>
        /// <value>List of UserPoints</value>
        [Required]
        public List<UserPoint> Points { get; set; }


        /// <summary>
        /// Map single UserId to multiple UserPoint.UserId. 
        /// </summary>
        /// <returns>List of UserPoint.</returns>
        internal IEnumerable<UserPoint> GetUserPoints(int userId)
        {
            return Points.Select(x => new UserPoint
            {
                UserId = userId,
                X = x.X,
                Y = x.Y
            });
        }
    }
}