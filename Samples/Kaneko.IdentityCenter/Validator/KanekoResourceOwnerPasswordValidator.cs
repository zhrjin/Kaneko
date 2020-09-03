﻿using IdentityModel;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Kaneko.IdentityCenter.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kaneko.IdentityCenter.Validator
{
    public class KanekoResourceOwnerPasswordValidator<TUser> : ResourceOwnerPasswordValidator<TUser>
        where TUser : ApplicationUser
    {
        protected UserManager<TUser> UserManager { get; }

        protected SignInManager<TUser> SignInManager { get; }

        protected ILogger<ResourceOwnerPasswordValidator<TUser>> Logger { get; }

        public KanekoResourceOwnerPasswordValidator(
            UserManager<TUser> userManager,
            SignInManager<TUser> signInManager,
            ILogger<ResourceOwnerPasswordValidator<TUser>> logger)
            : base(
                  userManager,
                  signInManager,
                  logger)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Logger = logger;
        }

        public override async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await UserManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                var result = await SignInManager.CheckPasswordSignInAsync(user, context.Password, true);
                if (result.Succeeded)
                {
                    Logger.LogInformation("Credentials validated for username: {username}", context.UserName);

                    var sub = await UserManager.GetUserIdAsync(user);
                    context.Result = new GrantValidationResult(sub, OidcConstants.AuthenticationMethods.Password, GetAdditionalClaimsOrNull(user));
                    return;
                }
                else if (result.IsLockedOut)
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
                }
                else if (result.IsNotAllowed)
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                }
                else
                {
                    Logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                }
            }
            else
            {
                Logger.LogInformation("No user found matching username: {username}", context.UserName);
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }

        protected virtual IEnumerable<Claim> GetAdditionalClaimsOrNull(TUser user)
        {
            return null;
        }
    }
}
