namespace WebAPI.ViewModels
{
    public class BuildViewModel
    {
        public int Id { get; set; }
        public string? DiscordUserId { get; set; }
        public string Augments { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime ModifiedAtUtc { get; set; }
        public int HeroId { get; set; }
    }
}
