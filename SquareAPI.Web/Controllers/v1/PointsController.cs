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
    [Route("api/v1/points/[action]")]
    public class PointsController : ApiController
    {
        /// <summary>
        /// SquareService instance to access business logic.
        /// </summary>
        private readonly ISquareService _squareService;

        /// <summary>
        /// Constructor to initialize readonly variables.
        /// </summary>
        /// <param name="squareService">Injected instance of SquareService.</param>
        public PointsController(ISquareService squareService)
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
        [Produces(ApplicationConstant.JsonContentType)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(FailResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Add([FromBody] UserInput input)
        {
            var response = new Response()
            {
                Success = false
            };

            if (input.Points.Count > 0)
            {
                await _squareService.AddUserPoints(input.GetUserPoints(UserId));
                response.Success = true;
                response.Message = ResponseMessage.RecordsInsertSuccess;
            }
            else
            {
                response.Success = false;
                response.Message = ResponseMessage.RecordsInsertFail;
                return BadRequest(response);
            }

            return Ok(response);

        }

        /// <summary>
        /// Upload CSV file to insert points in DB.
        /// </summary>
        /// <param name="file">Uploaded csv file.</param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request. Points are missing</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Produces(ApplicationConstant.JsonContentType)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(FailResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> FileUpload(IFormFile file)
        {
            var response = new Response()
            {
                Success = false
            };

            if (file != null && file.Length > 0 && Path.GetExtension(file.FileName).ToLower() == ApplicationConstant.CsvFileExtention)
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var pointsToUpload = _squareService.GetPoints(stream);
                    if (pointsToUpload != null && pointsToUpload.Count() > 0)
                    {
                        foreach (var points in pointsToUpload)
                        {
                            points.UserId = UserId;
                        }

                        await _squareService.AddUserPoints(pointsToUpload);
                        response.Success = true;
                        response.Message = ResponseMessage.FileUploadSuccess;
                        return Ok(response);
                    }
                }
            }

            response.Message = ResponseMessage.FileInvalid;
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
        [Produces(ApplicationConstant.JsonContentType)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(FailResponse), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Delete([FromBody] UserInput input)
        {
            var response = new Response()
            {
                Success = false
            };

            if (input.Points.Count > 0)
            {
                await _squareService.DeleteUserPoints(input.GetUserPoints(UserId));
                response.Success = true;
                response.Message = ResponseMessage.RecordsDeleteSuccess;
            }
            else
            {
                response.Success = false;
                response.Message = ResponseMessage.RecordsDeleteFail;
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}