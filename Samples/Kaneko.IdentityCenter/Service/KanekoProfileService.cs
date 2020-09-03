using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Kaneko.IdentityCenter.Data;
using Kaneko.IdentityCenter.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kaneko.IdentityCenter.Service
{
    internal class KanekoProfileService : IProfileService
    {
        private readonly AspNetAccountDbContext _context;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;


        public KanekoProfileService(AspNetAccountDbContext context,
            UserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
            _context = context;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string userId = context.Subject.GetSubjectId();
            ApplicationUser currentUser = _context.Users.Find(userId);
            List<Claim> claims = new List<Claim>();
            var principal = await _userClaimsPrincipalFactory.CreateAsync(currentUser);
            claims.AddRange(principal.Claims);
            claims.Add(new Claim(Consts.ClaimTypes.ClientId, context.Client.ClientId));
            var roles = GetRoles(currentUser.Id);
        
            UserData userData = new UserData
            {
                UserId = currentUser.Id,
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Roles = roles,
                UserAccesses = principal.Claims.Where(u => u.Type == Consts.ClaimTypes.UserAccess).Select(u => u.Value).ToList(),
                ClientId = context.Client.ClientId
            };

            claims.Add(new Claim(Consts.ClaimTypes.UserData, Newtonsoft.Json.JsonConvert.SerializeObject(userData)));
            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string id = context.Subject.GetSubjectId();
            var user = await _context.Users.FindAsync(id);
            if (user != null && !user.IsDeleted)
            {
                context.IsActive = true;
                return;
            }
            context.IsActive = false;
        }


        private List<RoleInfo> GetRoles(string userId)
        {
            return null;
            //return _context.UserRoles.Include(u => u.r.Role)
            //.Where(u => u.UserId == userId && !u.Role.IsDeleted)
            //.Select(u => new RoleInfo
            //{
            //    Id = u.RoleId,
            //    RoleName = u.Role.Name
            //}).ToList();
        }

        private class UserData
        {
            public string UserId { get; internal set; }
            public string UserName { get; internal set; }
            public object RealName { get; internal set; }
            public string Email { get; internal set; }
            public object Photo { get; internal set; }
            public List<RoleInfo> Roles { get; internal set; }
            public List<string> RoleAccesses { get; internal set; }
            public List<string> UserAccesses { get; internal set; }
            public string ClientId { get; internal set; }
        }
    }

}
