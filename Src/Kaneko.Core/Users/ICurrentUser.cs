using System.Security.Claims;

namespace Kaneko.Core.Users
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        string Id { get; }

        string UserName { get; }

        string PhoneNumber { get; }

        string Email { get; }

        string[] Roles { get; }

        Claim FindClaim(string claimType);

        Claim[] FindClaims(string claimType);

        Claim[] GetAllClaims();

        bool IsInRole(string roleName);
    }
}
