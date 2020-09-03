using Microsoft.AspNetCore.Identity;

namespace Kaneko.IdentityCenter.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsDeleted { set; get; }
    }
}
