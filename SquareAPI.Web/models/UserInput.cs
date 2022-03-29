using System.ComponentModel.DataAnnotations;
using SquareAPI.Business;
using SquareAPI.Data;

namespace SquareAPI.Web
{
    /// <summary>
    /// Class to deserialize user enterd points data from api request.
    /// </summary>
    public class UserInput
    {
        public UserInput()
        {
            Points = new List<Point>();
        }

        [Required]
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        public List<Point> Points { get; set; }

        internal IEnumerable<UserPoints> GetUserPoints()
        {
            return Points.Select(x => new UserPoints
            {
                UserId = UserId,
                X = x.X,
                Y = x.Y
            });
        }
    }
}