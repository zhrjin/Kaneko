using Microsoft.AspNetCore.Mvc;

namespace Kaneko.Hosts.Controller
{
    [Route("api/[controller]")]
    public class HealthCheckController : KaneKoController
    {
        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Get()
        {
            return Ok("ok");
        }
    }
}
