using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
    [Index(nameof(CreatedAtUtc))]
    public class Change
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string DiscordId { get; set; }
        public string DiscordNick { get; set; }
        public string? SummaryPrevious { get; set; }
        public string? SummaryNext { get; set; }
        public DateTime CreatedAtUtc { get; set; }

    }
}
