using System.Runtime.Serialization;
using SquareAPI.Business;

namespace SquareAPI.Web
{
    /// <summary>
    /// Standard response class for insert/delete API's response.
    /// </summary>
    public class Response
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    /// <summary>
    /// Standard response class for get API's response.
    /// </summary>
    public class ResponseGet : Response
    {
        public SquarePoint data { get; set; }
    }
}