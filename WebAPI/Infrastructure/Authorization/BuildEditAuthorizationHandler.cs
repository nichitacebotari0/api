using Microsoft.AspNetCore.Authorization;
using WebAPI.Models.Discord;

namespace WebAPI.Infrastructure.Authorization
{
    public class BuildEditAuthorizationHandler : AuthorizationHandler<BuildEditRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BuildEditRequirement requirement)
        {
            var hasAdminClaim = context.User.HasClaim(x => x.Type == DiscordConstants.Claim_isdev && x.Value == "True" ||
             x.Type == DiscordConstants.Claim_ismod && x.Value == "True");
            var hasRegionClaim = context.User.HasClaim(x => x.Type == DiscordConstants.Claim_regionId &&
            BuildEditRequirement.AllowedRegionRoles.Intersect(x.Value.Split(',')).Any());
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (hasAdminClaim || hasRegionClaim || userId == DiscordConstants.SuperId)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
