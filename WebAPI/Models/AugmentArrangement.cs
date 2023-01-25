namespace WebAPI.Models
{
    public class AugmentArrangement
    {
        public int Id { get; set; }
        public int PatchId { get; set; }
        public Patch Patch { get; set; }
        public ICollection<AugmentSlot> AugmentSlots { get; set; }

        public override string ToString()
        {
            return @$"{Id} ;patch:{Id} {Patch?.Version} 
;augmentSlots:{string.Join(" ,", AugmentSlots?.Select(x => $"") ?? new[] { "" })}";
        }
    }
}
