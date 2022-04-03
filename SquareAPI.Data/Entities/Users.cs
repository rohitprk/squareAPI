using System.Text.Json.Serialization;

namespace SquareAPI.Data.Entities
{
    /// <summary>
    /// Model class to hold user data.
    /// </summary>
    public class Users
    {
        /// <summary>
        /// User Id for current user.
        /// </summary>
        /// <value>Nullable.Int</value>
        [JsonIgnore]
        public int? UserId { get; set; }

        /// <summary>
        /// Current user name.
        /// </summary>
        /// <value>string</value>
        public string Name { get; set; }

        /// <summary>
        /// Current user password
        /// </summary>
        /// <value>string</value>
        public string Password { get; set; }
    }

}