﻿using Microsoft.AspNetCore.Authorization;
using WebAPI.Models.Discord;

namespace WebAPI.Infrastructure.Authorization
{
    public class AugmentEditAuthorizationHandler : AuthorizationHandler<AugmentEditRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AugmentEditRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            var hasClaim = context.User.HasClaim(x => AugmentEditRequirement.AllowedClaims.Contains(x.Type) && x.Value == "True");
            if (AugmentEditRequirement.AllowedUsers.Contains(userId) || hasClaim)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }

}
