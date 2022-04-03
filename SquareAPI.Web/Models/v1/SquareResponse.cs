using SquareAPI.Business.Models;

namespace SquareAPI.Web.Models.v1
{
    /// <summary>
    /// Standard response class for insert/delete API's response.
    /// </summary>
    public class SquareResponse : Response
    {
        /// <summary>
        /// Field to return SquarePoint data.
        /// </summary>
        /// <value>SquarePoint</value>
        public new SquarePoint Data { get; set; }
    }
}