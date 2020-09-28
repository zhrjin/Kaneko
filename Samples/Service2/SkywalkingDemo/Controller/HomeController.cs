using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SkywalkingDemo.Controller
{
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new List<string> { "value1", "value2" };
        }
    }
}
