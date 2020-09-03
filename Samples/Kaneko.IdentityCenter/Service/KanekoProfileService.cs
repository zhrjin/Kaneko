using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Kaneko.IdentityCenter.Data;
using Kaneko.IdentityCenter.Entities;
using Kaneko.IdentityCenter.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
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
        
            UserData userData = new UserData
            {
                UserId = currentUser.Id,
                UserName = currentUser.UserName,
                Email = currentUser.Email,
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
    }

}
