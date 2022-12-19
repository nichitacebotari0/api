using Microsoft.AspNetCore.Authorization;
using WebAPI.Models.Discord;

namespace WebAPI.Infrastructure.Authorization
{
    public class BuildEditAuthorizationHandler : AuthorizationHandler<BuildEditRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BuildEditRequirement requirement)
        {
            var hasAdminClaim = context.User.HasClaim(x => x.Type == DiscordConstants.Claim_isdev && x.Value == "true" ||
             x.Type == DiscordConstants.Claim_ismod && x.Value == "true");
            var hasRegionClaim = context.User.HasClaim(x => x.Type == DiscordConstants.Claim_regionId &&
            BuildEditRequirement.AllowedRegionRoles.Intersect(x.Value.Split(',')).Any());
            if (hasAdminClaim || hasRegionClaim)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
