using System.IO;

namespace Kaneko.Core.Users
{
    public class UserConsts
    {
        public static ClaimTypes ClaimTypes { get; internal set; } = new ClaimTypes();
        public static string DefaultUserPassword { get; internal set; } = "123456";
    }

    public class ClaimTypes
    {
        public string ClientId  = "ClientId";

        public string RoleAccess  = "RoleAccess";

        public string UserAccess  = "UserAccess";
        public string UserData  = "UserData";
    }
}