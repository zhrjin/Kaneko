using Kaneko.Hosts.Attributes;
using Microsoft.AspNetCore.Mvc;
namespace Kaneko.Hosts.Controller
{
    [ApiController]
    [ServiceFilter(typeof(KanekoActionFilterAttribute))]
    public class KaneKoController : ControllerBase
    {
    }
}
