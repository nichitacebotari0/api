namespace WebAPI.Models
{
    public class AugmentSlot
    {
        public int Id { get; set; }
        public int AugmentCategoryId { get; set; }
        public AugmentCategory AugmentCategory { get; set; }
        public int AugmentArrangementId { get; set; }
        public AugmentArrangement AugmentArrangement { get; set; }
        public int SortOrder { get; set; }

        public override string ToString()
        {
            return $"{Id} ;category:{AugmentCategoryId} {AugmentCategory?.Name}";
        }
    }
}
