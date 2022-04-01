namespace SquareAPI.Business.Models
{
    /// <summary>
    /// Model class to hold square point data and count.
    /// </summary>
    public class SquarePoint
    {
        /// <summary>
        /// Number of squares form from co-ordinate points.
        /// </summary>
        /// <value></value>
        public int Count { get; set; } = 0;

        /// <summary>
        /// All the squares form from provided co-ordinates.
        /// </summary>
        /// <typeparam name="Square"><see cref="Square"></see></typeparam>
        /// <returns>list of Square object.</returns>
        public List<Square> Square { get; set; } = new List<Square>();
    }
}