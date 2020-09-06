namespace Kaneko.Core.IdentityServer
{
    public interface ICurrentUser
    {
        string UserId { get; }

        string UserName { get; }

        string PhoneNumber { get; }

        string Email { get; }

        string ClientId { get; }

        string[] Roles { get; }

        bool IsInRole(string roleName);
    }

    public class CurrentUser : ICurrentUser
    {

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string ClientId { get; set; }

        public string[] Roles { get; set; }

        public bool IsInRole(string roleName)
        {
            throw new System.NotImplementedException();
        }
    }
}
