using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Infrastructure.Mapping
{
    public static class AugmentSlotMap
    {
        public static AugmentSlot MapToDTO(this AugmentSlotViewModel viewModel)
        {
            return new AugmentSlot
            {
                Id = viewModel.Id,
                AugmentCategoryId = viewModel.AugmentCategoryId,
                AugmentArrangementId = viewModel.AugmentArrangementId,
                SortOrder = viewModel.SortOrder,
            };

        }

        public static AugmentSlotViewModel MapToViewModel(this AugmentSlot model)
        {
            return new AugmentSlotViewModel
            {
                Id = model.Id,
                AugmentCategoryId = model.AugmentCategoryId,
                AugmentArrangementId = model.AugmentArrangementId,
                SortOrder = model.SortOrder,
            };
        }
    }
}
