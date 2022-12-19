using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Build
    {
        public int Id { get; set; }
        [Required]
        public string DiscordUserId { get; set; }
        public string Augments { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime ModifiedAtUtc { get; set; }
        public int HeroId { get; set; }
        public Hero Hero { get; set; }
    }
}
