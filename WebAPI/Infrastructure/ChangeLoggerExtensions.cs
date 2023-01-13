using System.Security.Claims;
using WebAPI.Models.Discord;
using WebAPI.Services;

namespace WebAPI.Infrastructure
{
    public static class ChangeLoggerExtensions
    {

        public static async Task Log<T>(this IChangeLogger logger, ClaimsPrincipal user, T? next, T? previous = null)
            where T : class
        {
            var type = typeof(T).Name;
            await Log(logger, type, user, next?.ToString(), previous?.ToString());
        }

        public static async Task Log(this IChangeLogger logger, string type, ClaimsPrincipal user, string? next, string? previous)
        {
            var userIdClaim = user?.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            var userNickClaim = user?.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userNick)?.Value;

            await logger.Log(type, userIdClaim, userNickClaim, previous, next);
        }
    }
}
