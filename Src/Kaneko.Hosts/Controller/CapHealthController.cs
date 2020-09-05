using Microsoft.AspNetCore.Mvc;

namespace Kaneko.Hosts.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CapController : ControllerBase
    {
        [HttpGet("health")]
        [HttpHead("health")]
        public IActionResult Get()
        {
            return Ok("ok");
        }
    }
}
