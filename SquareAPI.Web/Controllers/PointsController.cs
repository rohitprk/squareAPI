using System.Net;
using Microsoft.AspNetCore.Mvc;
using SquareAPI.Business;
using SquareAPI.Data;

namespace SquareAPI.Web
{

    /// <summary>
    /// Controller class for UserPoints API operations.
    /// </summary>
    [ApiController]
    [Route("api/v1/points/[action]")]
    public class PointsController : ControllerBase
    {
        private readonly IUserPointsRepository _squareDataRepo;

        public PointsController(IUserPointsRepository squareDataRepo)
        {
            _squareDataRepo = squareDataRepo;
        }

        /// <summary>
        /// Insert points in DB.
        /// </summary>
        /// <param name="input">User points.</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. Points are missing</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.BadRequest)]
        public IActionResult Add([FromBody] UserInput input)
        {
            var response = new Response()
            {
                success = false
            };

            if (input.Points.Count > 0)
            {
                try
                {
                    _squareDataRepo.AddUserPoints(input.UserId, input.GetUserPoints());
                    response.success = true;
                    response.message = "Record/s inserted successfully!";
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                response.success = false;
                response.message = "No point data to add.";
                return BadRequest(response);
            }

            return Ok(response);

        }

        /// <summary>
        /// Delete points from DB.
        /// </summary>
        /// <param name="input">User points.</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. Points are missing</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.BadRequest)]
        public IActionResult Delete([FromBody] UserInput input)
        {
            var response = new Response()
            {
                success = false
            };

            if (input.Points.Count > 0)
            {
                try
                {
                    _squareDataRepo.DeleteUserPoints(input.UserId, input.GetUserPoints());
                    response.success = true;
                    response.message = "Record/s deleted successfully!";
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                response.success = false;
                response.message = "No points data to delete.";
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get squares from points stored in DB.
        /// </summary>
        /// <param name="userId">User Id to get data.</param>
        /// <returns></returns>
        /// <response code="200">success</response>
        /// <response code="500">internal server error</response>
        [HttpGet]
        [Route("{userId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseGet), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Squares(int userId)
        {
            var response = new ResponseGet()
            {
                success = false,
                message = "No points found."
            };

            try
            {
                var userPoints = await _squareDataRepo.GetUserPoints(userId);
                if (userPoints != null && userPoints.Any())
                {
                    var points = userPoints.Select(point => new Point { X = point.X, Y = point.Y });
                    var squareData = new Calculate().GetSquare(userId, points);
                    response.success = true;
                    response.message = squareData.Count > 0 ? string.Empty : "No points form square.";
                    response.data = squareData;
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return Ok(response);
        }
    }
}