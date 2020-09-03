using System.IO;

namespace Kaneko.IdentityCenter.Service
{
    internal class Consts
    {
        public static ClaimTypes ClaimTypes { get; internal set; } = new ClaimTypes();
        public static string DefaultUserPassword { get; internal set; } = "123456";
    }

    internal class ClaimTypes
    {
        public string ClientId  = "ClientId";

        public string RoleAccess  = "RoleAccess";

        public string UserAccess  = "UserAccess";
        public string UserData  = "UserData";
    }
}