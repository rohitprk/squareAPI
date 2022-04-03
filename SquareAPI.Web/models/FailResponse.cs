using System.Runtime.Serialization;
using System.Text.Json;
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

        /// <summary>
        /// Overridden ToString method to return custom string. 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}