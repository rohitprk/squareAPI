using System.Net;
using Microsoft.AspNetCore.Mvc;
using SquareAPI.Business;
using SquareAPI.Web.Models;

namespace SquareAPI.Web.Controllers
{

    /// <summary>
    /// Controller class for UserPoints API operations.
    /// </summary>
    [ApiController]
    [Route("api/square")]
    public class SquareController : ControllerBase
    {
        /// <summary>
        /// SquareService instance to access business logic.
        /// </summary>
        private readonly SquareService _squareService;

        /// <summary>
        /// Constructor to initialize readonly variables.
        /// </summary>
        /// <param name="squareService">Injected instance of SquareService.</param>
        public SquareController(SquareService squareService)
        {
            _squareService = squareService;
        }

        /// <summary>
        /// Get squares from points stored in DB.
        /// </summary>
        /// <param name="userId">User Id to get data.</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="500">internal server error</response>
        [HttpGet]
        [Route("{userId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(SquareResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int userId)
        {
            var response = new SquareResponse()
            {
                Success = false,
                Message = "No points data found for user."
            };

            var squareData = await _squareService.GetSquare(userId);

            response.Success = true;
            response.Message = squareData.Count > 0 ? string.Empty : "No points to form square.";
            response.Data = squareData;

            return Ok(response);
        }
    }
}