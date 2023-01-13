using WebAPI.Models;

namespace WebAPI.Services
{
    public class ChangeLogger : IChangeLogger
    {
        private readonly ApplicationDbContext applicationDbContext;
        public ChangeLogger(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task Log(string modelName, string discordId, string discordNick, string? previous, string? next)
        {
            var log = new Change()
            {
                Model = modelName,
                DiscordId = discordId,
                DiscordNick = discordNick,
                CreatedAtUtc = DateTime.UtcNow,
                SummaryPrevious = previous,
                SummaryNext = next
            };
            applicationDbContext.ChangeLog.Add(log);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
