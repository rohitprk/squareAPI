using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SquareAPI.Business;
using SquareAPI.Business.Constants;
using SquareAPI.Data.Entities;
using SquareAPI.Web.Models.v1;

namespace SquareAPI.Web.Controllers.v1
{

    /// <summary>
    /// Controller class for User management.
    /// </summary>
    [ApiController]
    [Route("api/v1/user/[action]")]
    public class UserController : ApiController
    {
        /// <summary>
        /// User Service instance to access business layer methods.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// JWT repository instance for JWT Authentication. 
        /// </summary>
        private readonly IJWTRepository _jwtRepository;

        /// <summary>
        /// Constructor to initialize readonly variables.
        /// </summary>
        /// <param name="service"> User Service injected instance.</param>
        /// <param name="jwtRepository">JWT repository injected instance.</param>
        public UserController(IUserService service, IJWTRepository jwtRepository)
        {
            _userService = service;
            _jwtRepository = jwtRepository;
        }

        /// <summary>
        /// Authenticate User and Generate JWT Token.
        /// </summary>
        /// <param name="usersdata">User credentials.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Produces(ApplicationConstant.JsonContentType)]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Authenticate(Users usersdata)
        {
            var response = new AuthResponse
            {
                Success = true,
            };

            var token = await _jwtRepository.Authenticate(usersdata);

            if (token == null)
            {
                response.Success = false;
                response.Message = ResponseMessage.InvalidCredentials;
                return Unauthorized(response);
            }

            response.AccessToken = token;
            return Ok(response);
        }

        /// <summary>
        /// Register new users.
        /// </summary>
        /// <param name="usersdata">User credentials.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Produces(ApplicationConstant.JsonContentType)]
        [ProducesResponseType(typeof(Response), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Register(Users user)
        {
            var response = new Response
            {
                Success = true,
            };

            bool validRegister = await _userService.Register(user);

            if (!validRegister)
            {
                response.Message = ResponseMessage.UserAlreadyExists;
                return BadRequest(response);
            }

            response.Message = ResponseMessage.UserRegisterSuccess;
            return Ok(response);

        }
    }
}