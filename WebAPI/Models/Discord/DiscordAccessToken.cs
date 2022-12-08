namespace WebAPI.Models.Discord
{
    public class DiscordAccessToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public uint expires_in { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
    }
}
