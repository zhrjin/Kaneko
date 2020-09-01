using Microsoft.AspNetCore.Mvc;

namespace Kaneko.Hosts.Controller
{
    [Route("api/[controller]")]
    public class CapController : KaneKoController
    {
        [HttpGet("health")]
        [HttpHead("health")]
        public IActionResult Get()
        {
            return Ok("ok");
        }
    }
}
