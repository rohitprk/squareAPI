using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SquareAPI.Business.Constants;

namespace SquareAPI.Web.Controllers.v1
{
    /// <summary>
    /// Base class for all API controllers.
    /// </summary>
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// Current userId from claim;
        /// </summary>
        private int? _userId = null;

        /// <summary>
        /// Get user Id from claim principals.
        /// </summary>
        /// <value>int</value>
        public int UserId
        {
            get
            {
                if (!_userId.HasValue)
                {
                    var userIdentity = User.Identity as ClaimsIdentity;
                    string userId = userIdentity.FindFirst(ApplicationConstant.UserIdClaim)?.Value;
                    _userId = int.Parse(userId);
                }

                return _userId.Value;
            }
        }
    }
}

