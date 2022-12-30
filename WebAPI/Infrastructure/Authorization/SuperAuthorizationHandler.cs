using Microsoft.AspNetCore.Authorization;
using WebAPI.Models.Discord;

namespace WebAPI.Infrastructure.Authorization
{
    public class SuperAuthorizationHandler : AuthorizationHandler<SuperAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperAuthorizationRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (AugmentEditRequirement.AllowedUsers.Contains(userId))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
