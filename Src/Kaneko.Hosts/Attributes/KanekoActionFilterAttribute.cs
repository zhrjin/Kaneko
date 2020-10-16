using Kaneko.Core.IdentityServer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Orleans.Runtime;
using SkyApm.Config;
using System;
using System.Linq;

namespace Kaneko.Hosts.Attributes
{
    public class KanekoActionFilterAttribute : ActionFilterAttribute
    {
        public KanekoActionFilterAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            RequestContext.Set(IdentityServerConsts.ClaimTypes.SkyWalking, GetSkyWalkingHeaderSW8(context.HttpContext.Request));
            string userData = null;
            try { userData = context.HttpContext.Request.Headers[IdentityServerConsts.ClaimTypes.UserData].ToString(); } catch { }
            RequestContext.Set(IdentityServerConsts.ClaimTypes.UserData, userData);
        }

        private string GetSkyWalkingHeaderSW8(HttpRequest httpRequest)
        {
            string sw8 = "";
            try
            {
                if (httpRequest.Headers.Any(m => m.Key == HeaderVersions.SW8))
                {
                    string values = httpRequest.Headers.First(x => x.Key == HeaderVersions.SW8).Value;
                    if (values.IndexOf(",") > -1)
                    {
                        string[] arr = values.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        sw8 = arr[0];
                    }
                    else
                    {
                        sw8 = values;
                    }
                }
            }
            catch { }
            return sw8;
        }
    }
}
