using System.IO;

namespace Kaneko.Core.IdentityServer
{
    public class IdentityServerConsts
    {
        public static ClaimTypes ClaimTypes { get; internal set; } = new ClaimTypes();
        public static string DefaultUserPassword { get; internal set; } = "123456";

        public static string HttpClientName { get; internal set; } = "GetIdentityServerUserInfo";
    }

    public class ClaimTypes
    {
        public string ClientId  = "ClientId";

        public string RoleAccess  = "RoleAccess";

        public string UserAccess  = "UserAccess";
        public string UserData  = "KanekoUserData";
        public string SkyWalking = "KanekoSkyWalking";
        public string SkyWalkingOnActivateSpeedTime = "KanekoSkyWalkingOnActivateSpeedTime";
    }
}