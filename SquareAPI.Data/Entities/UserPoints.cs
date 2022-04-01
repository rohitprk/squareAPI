using System.Text.Json.Serialization;

namespace SquareAPI.Data.Entities
{
    public class UserPoint
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

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