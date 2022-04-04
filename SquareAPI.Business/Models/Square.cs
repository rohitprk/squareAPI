using SquareAPI.Data.Entities;

namespace SquareAPI.Business.Models
{
    /// <summary>
    /// Model class to hold square data.
    /// </summary>
    public class Square
    {
        /// <summary>
        /// Array to hold 4 set of co-ordinates that form square.
        /// </summary>
        /// <value>Array of UerPoint object.</value>
        public UserPoint[] Points { get; set; }

        /// <summary>
        /// Overridden equal method to compare.
        /// </summary>
        /// <param name="obj">Second object to compare</param>
        /// <returns>true if the objects are equal else false</returns>
        public override bool Equals(object obj)
        {
            var square = obj as Square;

            if (square == null)
                return false;

            return square.Points[0].Equals(Points[0])
                && square.Points[1].Equals(Points[1])
                && square.Points[2].Equals(Points[2])
                && square.Points[3].Equals(Points[3]);
        }

        /// <summary>
        /// Overridden get hash code method to compare objects.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return Points[0].X ^ Points[1].X ^ Points[2].X ^ Points[3].X ^ Points[0].Y ^ Points[1].Y ^ Points[2].Y ^ Points[3].Y;
        }
    }
}