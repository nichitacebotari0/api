namespace WebAPI.Models.Discord
{
    public class DiscordGuildMember
    {
        public DiscordUser user { get; set; }
        public string nick { get; set; }
        public string avatar { get; set; }
        public string[] roles { get; set; }
        public string joined_at { get; set; }
        public string premium_since { get; set; }
        public bool deaf { get; set; }
        public bool mute { get; set; }
        public bool pending { get; set; }
        public string permissions { get; set; }
        public string communication_disabled_until { get; set; }
    }
}
