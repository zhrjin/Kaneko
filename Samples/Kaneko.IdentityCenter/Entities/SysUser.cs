using Microsoft.AspNetCore.Identity;

namespace Kaneko.IdentityCenter.Entities
{
    public class SysUser : IdentityUser
    {
        public bool IsDeleted { set; get; }
    }
}
