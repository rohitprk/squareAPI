using System.Text.Json;

namespace SquareAPI.Web.Models.v1
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