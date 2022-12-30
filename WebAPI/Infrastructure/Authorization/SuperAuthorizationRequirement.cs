using Microsoft.AspNetCore.Authorization;
using WebAPI.Models.Discord;

namespace WebAPI.Infrastructure.Authorization
{
    public class SuperAuthorizationRequirement : IAuthorizationRequirement
    {
        public static readonly IEnumerable<string> AllowedUsers = new[] { DiscordConstants.SuperId };
    }
}
