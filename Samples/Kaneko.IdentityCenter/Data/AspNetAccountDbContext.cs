using Kaneko.IdentityCenter.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kaneko.IdentityCenter.Data
{
    public class AspNetAccountDbContext : IdentityDbContext<ApplicationUser>
    {
        public AspNetAccountDbContext(DbContextOptions<AspNetAccountDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }

    public class ApplicationDbContext : IdentityDbContext<SysUser, SysRole, string, SysUserClaim, SysUserRole, SysUserLogin, SysRoleClaim, SysUserToken>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Entity<SysUser>().ToTable("SysUser");
            //builder.Entity<SysUserClaim>().ToTable("SysUserClaim");
            //builder.Entity<SysUserLogin>().ToTable("SysUserLogin");
            //builder.Entity<SysUserToken>().ToTable("SysUserToken");
            //builder.Entity<SysUserRole>().ToTable("SysUserRole");
            //builder.Entity<SysUserRole>()
            //    .HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId);
            //builder.Entity<SysUserRole>()
            //   .HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId);
            //builder.Entity<SysUserRole>().ToTable("SysUserRole");
            //builder.Entity<SysRole>().ToTable("SysRole");
            //builder.Entity<SysRoleClaim>().ToTable("SysRoleClaim");
            //builder.Entity<SysOrgUnit>().ToTable("SysOrgUnit");
            //builder.Entity<SysMenuMaster>().ToTable("SysMenuMaster");
            //builder.Entity<SysMenu>().ToTable("SysMenu");
            //builder.Entity<SysDepartment>().ToTable("SysDepartment");
            //builder.Entity<SysRole>().HasData(DatabaseIniter.GetRoles());
            //builder.Entity<SysDepartment>().HasData(DatabaseIniter.GetDepartments());
            //builder.Entity<SysOrgUnit>().HasData(DatabaseIniter.GetOrgUnits());
        }
    }

}
