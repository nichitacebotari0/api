using Microsoft.AspNetCore.Authorization;
using WebAPI.Models.Discord;

namespace WebAPI.Infrastructure.Authorization
{
    public class BuildEditRequirement : IAuthorizationRequirement
    {
        public static readonly IEnumerable<string> AllowedRegionRoles = DiscordConstants.FangsRegionRoles;
    }
}
