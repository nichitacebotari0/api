namespace WebAPI.Models
{
    public class Patch
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public DateTime GameDate { get; set; }
        public DateTime? WebsiteTimeUtc { get; set; } = null;

        public override string ToString()
        {
            return $"{Id} {Version} official date:{GameDate.ToString("yyyy-MM-dd HH:mm")}  activation date:{GameDate.ToString("yyyy-MM-dd HH:mm")}";
        }
    }
}
