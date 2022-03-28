using Microsoft.AspNetCore.Mvc;
using SquareAPI.Business;

namespace SquareAPI.Web
{

    [ApiController]
    [Route("api/v1/point/[action]")]
    public class PointController: ControllerBase
    {
        [HttpPost]
        public IActionResult Add([FromBody] UserInput points)
        {
            var returnData = new {success = true, message=""};
            return Ok(returnData);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] UserInput points)
        {
            var returnData = new {success = true, message=""};
            return Ok(returnData);
        }

        [HttpGet]
        public IActionResult Squares()
        {
            var returnData = new {success = true, message=""};
            return Ok(returnData);
        }
    }
}