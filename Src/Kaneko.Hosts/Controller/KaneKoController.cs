using Kaneko.Hosts.Attributes;
using Microsoft.AspNetCore.Mvc;
namespace Kaneko.Hosts.Controller
{
    [ApiController]
    [KanekoActionFilter]
    public class KaneKoController : ControllerBase
    {
    }
}
