using Kaneko.Core.IdentityServer;
using Microsoft.AspNetCore.Mvc.Filters;
using Orleans.Runtime;

namespace Kaneko.Hosts.Attributes
{
    public class KanekoActionFilterAttribute : ActionFilterAttribute
    {
        public KanekoActionFilterAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string userData = null;
            try { userData = context.HttpContext.Request.Headers[IdentityServerConsts.ClaimTypes.UserData].ToString(); } catch { }
            RequestContext.Set(IdentityServerConsts.ClaimTypes.UserData, userData);
        }
    }
}
