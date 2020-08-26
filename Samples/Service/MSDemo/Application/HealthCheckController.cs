using Microsoft.AspNetCore.Mvc;

namespace MSDemo.Application
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
