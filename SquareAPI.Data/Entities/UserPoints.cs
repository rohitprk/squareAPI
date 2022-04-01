using System.Text.Json.Serialization;

namespace SquareAPI.Data.Entities
{
    /// <summary>
    /// UserPoint entity class to hold UserPoints DB data.
    /// </summary>
    public class UserPoint
    {
        /// <summary>
        /// Selected userId.
        /// </summary>
        /// <value>int</value>
        [JsonIgnore]
        public int UserId { get; set; }

        /// <summary>
        /// X co-ordinate of plane.
        /// </summary>
        /// <value>int</value>
        public int X { get; set; }

        /// <summary>
        /// Y co-ordinate of plane.
        /// </summary>
        /// <value>int</value>
        public int Y { get; set; }

        /// <summary>
        /// Unique Key of X and Y co-ordinate.
        /// </summary>
        /// <value>int</value>
        [JsonIgnore]
        public string Key => $"{X},{Y}";

        /// <summary>
        /// Overridden equal method to compare.
        /// </summary>
        /// <param name="obj">Second object to compare</param>
        /// <returns>True if the objects are equal else false</returns>
        public override bool Equals(object obj)
        {
            var rightObj = obj as UserPoint;

            if (object.ReferenceEquals(rightObj, null))
            {
                return false;
            }

            if (UserId == rightObj.UserId && X == rightObj.X && Y == rightObj.Y)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Overridden equal method to compare.
        /// </summary>
        /// <param name="obj">Second object to compare</param>
        /// <returns>Whether the objects are equal or not</returns>
        public override int GetHashCode()
        {
            return UserId.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}