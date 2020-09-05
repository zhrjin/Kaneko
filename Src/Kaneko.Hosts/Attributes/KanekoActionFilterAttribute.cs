using IdentityModel.Client;
using Kaneko.Core.Users;
using Kaneko.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Orleans.Runtime;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kaneko.Hosts.Attributes
{
    public class KanekoActionFilterAttribute : ActionFilterAttribute
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _tokenClient;

        public KanekoActionFilterAttribute(IDistributedCache distributedCache, IConfiguration configuration)
        {
            this._cache = distributedCache;
            this._configuration = configuration;
            _tokenClient = new HttpClient();
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var authorization = context.HttpContext.Request.Headers["Authorization"].ToString();

            authorization = authorization.Replace("Bearer ", "");

            string userData = TryGetOrAdd(authorization);

            RequestContext.Set(UserConsts.ClaimTypes.UserData, userData);
        }

        private string TryGetOrAdd(string token)
        {
            try
            {
                string key = $"{UserConsts.ClaimTypes.UserData}:{HashHelper.GetHashString(token)}";

                string userData = _cache.GetString(key);

                if (string.IsNullOrEmpty(userData))
                {
                    var response = _tokenClient.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = "http://192.168.0.106:12345/connect/userinfo",
                        Token = token
                    }).ConfigureAwait(false).GetAwaiter().GetResult();

                    if (response.IsError) throw new Exception(response.Error);

                    userData = response.Claims.Where(m => m.Type == UserConsts.ClaimTypes.UserData).FirstOrDefault().ToString();
                    _cache.SetString(key, userData, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) });
                }

                return userData;
            }
            catch
            {
                return null;
            }
        }
    }
}
