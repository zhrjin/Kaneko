using System.Collections.Generic;

namespace Kaneko.OcelotGateway
{
    public class OcelotOptions
    {
        public KanekoIdentityCenter KanekoIdentityCenter { set; get; }

        public List<Route> Routes { set; get; } = new List<Route>();
    }

    public class KanekoIdentityCenter
    {
        public string Authority { set; get; }

        public double CacheDurationMinutes { set; get; } = 10;

        public bool EnableCaching { set; get; } = true;

        public string IdentityScheme { set; get; } = "Bearer";
    }

    public class Route
    {
        public string ServiceName { set; get; }

        public string DownstreamPathTemplate { set; get; }

        public AuthenticationOptions AuthenticationOptions { set; get; }
    }

    public class AuthenticationOptions
    {
        public string AuthenticationProviderKey { set; get; }
    }
}
