using System.ComponentModel.DataAnnotations;
using SquareAPI.Business.Models;
using SquareAPI.Data.Entities;

namespace SquareAPI.Web.Models
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
        /// Current User Id.
        /// </summary>
        /// <value>int</value>
        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

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
        internal IEnumerable<UserPoint> GetUserPoints()
        {
            return Points.Select(x => new UserPoint
            {
                UserId = UserId,
                X = x.X,
                Y = x.Y
            });
        }
    }
}