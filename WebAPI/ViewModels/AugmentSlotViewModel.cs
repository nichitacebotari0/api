using WebAPI.Models;

namespace WebAPI.ViewModels
{
    public class AugmentSlotViewModel
    {
        public int Id { get; set; }
        public int AugmentCategoryId { get; set; }
        public int AugmentArrangementId { get; set; }
        public int SortOrder { get; set; }

    }
}
