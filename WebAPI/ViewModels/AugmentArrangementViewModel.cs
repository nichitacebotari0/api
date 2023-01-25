namespace WebAPI.ViewModels
{
    public class AugmentArrangementViewModel
    {
        public int Id { get; set; }
        public int PatchId { get; set; }
        public IEnumerable<AugmentSlotViewModel> AugmentSlots { get; set; }
    }
}
