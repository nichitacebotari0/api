namespace WebAPI.ViewModels
{
    public class AugmentViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int? AugmentCategoryId { get; set; }
        public int? AbilityTypeId { get; set; }
        public int HeroId { get; set; }
        public int PatchId { get; set; }
    }
}
