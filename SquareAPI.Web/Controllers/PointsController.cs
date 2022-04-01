using System.Net;
using Microsoft.AspNetCore.Mvc;
using SquareAPI.Business;
using SquareAPI.Data;
using SquareAPI.Data.Entities;
using SquareAPI.Web.Models;

namespace SquareAPI.Web.Controllers
{

    /// <summary>
    /// Controller class for UserPoints API operations.
    /// </summary>
    [ApiController]
    [Route("api/points/[action]")]
    public class PointsController : ControllerBase
    {
        private readonly SquareService _squareService;

        public PointsController(SquareService squareService)
        {
            _squareService = squareService;
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
        public async Task<IActionResult> Add([FromBody] UserInput input)
        {
            var response = new Response()
            {
                Success = false
            };

            if (input.Points.Count > 0)
            {
                await _squareService.AddUserPoints(input.GetUserPoints());
                response.Success = true;
                response.Message = "Record/s inserted Successfully!";
            }
            else
            {
                response.Success = false;
                response.Message = "No point data to add.";
                return BadRequest(response);
            }

            return Ok(response);

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
        public async Task<IActionResult> FileUpload([FromForm] int userId, IFormFile file)
        {
            var response = new Response()
            {
                Success = false
            };

            if (file != null && file.Length > 0 && Path.GetExtension(file.FileName).ToLower() == ".csv")
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var pointsToUpload = _squareService.GetPoints(stream).Where(x => x.UserId == userId);
                    if (pointsToUpload != null && pointsToUpload.Count() > 0)
                    {
                        await _squareService.AddUserPoints(pointsToUpload);
                        response.Success = true;
                        response.Message = "Uploaded file successfully!";
                        return Ok(response);
                    }
                }
            }

            response.Message = "No data or invalid file type! Only CSV format file supported.";
            return BadRequest(response);

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
        public async Task<IActionResult> Delete([FromBody] UserInput input)
        {
            var response = new Response()
            {
                Success = false
            };

            if (input.Points.Count > 0)
            {
                await _squareService.DeleteUserPoints(input.GetUserPoints());
                response.Success = true;
                response.Message = "Record/s deleted Successfully!";
            }
            else
            {
                response.Success = false;
                response.Message = "No points data to delete.";
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}