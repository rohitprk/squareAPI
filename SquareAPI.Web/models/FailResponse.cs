using System.Runtime.Serialization;
using SquareAPI.Business;

namespace SquareAPI.Web.Models
{
    /// <summary>
    /// Standard fail response class for swagger example.
    /// </summary>
    public class FailResponse
    {
        /// <summary>
        /// Success value always false.
        /// </summary>
        /// <example>false</example>
        public bool Success { get; set; }

        /// <summary>
        /// Message to describe operation.
        /// </summary>
        /// <example>Something relevant!</example>
        public string Message { get; set; }
    }
}