using Microsoft.AspNetCore.Mvc;

namespace Kaneko.AuthCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Get()
        {
            return Ok("ok");
        }
    }
}
