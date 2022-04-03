namespace SquareAPI.Web.Models
{
    /// <summary>
    /// Authentication response class to generate token.
    /// </summary>
    public class AuthResponse : Response
    {
        /// <summary>
        /// JWT Bearer token after authentication.
        /// </summary>
        /// <value>string</value>
        public string AccessToken { get; set; }
    }
}