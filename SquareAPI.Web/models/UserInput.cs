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
        public UserInput()
        {
            Points = new List<UserPoint>();
        }

        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        public List<UserPoint> Points { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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