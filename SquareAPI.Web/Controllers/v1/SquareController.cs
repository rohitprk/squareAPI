using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquareAPI.Business;
using SquareAPI.Business.Constants;
using SquareAPI.Web.Models.v1;

namespace SquareAPI.Web.Controllers.v1
{

    /// <summary>
    /// Controller class for UserPoints API operations.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/v1/square")]
    public class SquareController : ApiController
    {
        /// <summary>
        /// SquareService instance to access business logic.
        /// </summary>
        private readonly ISquareService _squareService;

        /// <summary>
        /// Constructor to initialize readonly variables.
        /// </summary>
        /// <param name="squareService">Injected instance of SquareService.</param>
        public SquareController(ISquareService squareService)
        {
            _squareService = squareService;
        }

        /// <summary>
        /// Get squares from points stored in DB.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="500">internal server error</response>
        [HttpGet]
        [Produces(ApplicationConstant.JsonContentType)]
        [ProducesResponseType(typeof(SquareResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(FailResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Get()
        {
            var response = new SquareResponse();

            var squareData = await _squareService.GetSquare(UserId);

            response.Success = true;
            response.Message = squareData.Count > 0 ? string.Empty : ResponseMessage.NoPointDataForSquare;
            response.Data = squareData;

            return Ok(response);
        }
    }
}