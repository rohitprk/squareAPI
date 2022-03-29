using System.Diagnostics.CodeAnalysis;

namespace SquareAPI.Data
{
    public class UserPoints
    {
        public int UserId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        /// <summary>
        /// Overriden equal method to compare.
        /// </summary>
        /// <param name="obj">Second object to compare</param>
        /// <returns>True if the objects are equal else false</returns>
        public override bool Equals(object obj)
        {
            var rightObj = obj as UserPoints;

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
        /// Overriden equal method to compare.
        /// </summary>
        /// <param name="obj">Second object to compare</param>
        /// <returns>Whether the objects are equal or not</returns>
        public override int GetHashCode()
        {
            return UserId.GetHashCode() ^ X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}