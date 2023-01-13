using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models
{
    [Index(nameof(BuildId), nameof(DiscordUserId), IsUnique = true, Name = "User_Build_Unique")]
    public class BuildVote
    {
        public int Id { get; set; }
        public string DiscordUserId { get; set; }
        public int VoteValue { get; set; }
        public int BuildId { get; set; }
        public Build Build { get; set; }

        public override string ToString()
        {
            return $"{Id} ({VoteValue}) ;discordUser:{DiscordUserId} ;Build:{BuildId} {Build?.Title}";
        }
    }
}
