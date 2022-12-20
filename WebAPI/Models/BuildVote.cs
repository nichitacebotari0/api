using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
    [Index(nameof(DiscordUserId), nameof(BuildId), IsUnique = true, Name = "User_Build_Unique")]
    public class BuildVote
    {
        public int Id { get; set; }
        public string DiscordUserId { get; set; }
        public int VoteValue { get; set; }
        public int BuildId { get; set; }
        public Build Build { get; set; }
    }
}
