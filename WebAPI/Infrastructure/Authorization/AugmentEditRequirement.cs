using Microsoft.AspNetCore.Authorization;
using WebAPI.Models.Discord;

namespace WebAPI.Infrastructure.Authorization
{
    public class AugmentEditRequirement : IAuthorizationRequirement
    {
        public static readonly IEnumerable<string> AllowedClaims = new[] { DiscordConstants.Claim_isdev, DiscordConstants.Claim_ismod };
        public static readonly IEnumerable<string> AllowedUsers = new[] { DiscordConstants.SuperId };
    }
}
