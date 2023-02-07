namespace WebAPI.Models
{
    public class BoonEvent
    {
        public int Id { get; set; }
        public int BoonId { get; set; }
        public EventAction Action { get; set; }
        public int PatchId { get; set; }
        public Patch Patch { get; set; }
        public string? Name { get; set; }
        public string? ImagePath { get; set; }
        public string? Description { get; set; }

        public override string ToString()
        {
            return $"{Id} {Name} dbid:{Id} ;patch:{PatchId}";
        }
    }
}
