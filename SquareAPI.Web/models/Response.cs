using System.Runtime.Serialization;
using SquareAPI.Business;

namespace SquareAPI.Web.Models
{
    /// <summary>
    /// Standard response class for insert/delete API's response.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Success true if execution successful.
        /// </summary>
        /// <example>true</example>
        public bool Success { get; set; }

        /// <summary>
        /// Message to describe operation.
        /// </summary>
        /// <example>Something relevant!</example>
        public string Message { get; set; }
    }
}