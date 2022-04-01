using SquareAPI.Business.Models;

namespace SquareAPI.Web.Models
{
    /// <summary>
    /// Standard response class for insert/delete API's response.
    /// </summary>
    public class SquareResponse : Response
    {
        public SquarePoint data { get; set; }
    }
}