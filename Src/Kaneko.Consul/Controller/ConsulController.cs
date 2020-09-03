using Microsoft.AspNetCore.Mvc;

namespace Kaneko.Consul.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsulController : ControllerBase
    {
        [HttpGet("health")]
        [HttpHead("health")]
        public IActionResult Get()
        {
            return Ok("ok");
        }
    }
}
