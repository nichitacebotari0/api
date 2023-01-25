using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Infrastructure.Mapping
{
    public static class AugmentArrangementMap
    {
        public static AugmentArrangement MapToDTO(this AugmentArrangementViewModel viewModel)
        {
            return new AugmentArrangement
            {
                Id = viewModel.Id,
                PatchId = viewModel.PatchId,
                AugmentSlots = viewModel.AugmentSlots.Select(AugmentSlotMap.MapToDTO).ToArray(),
            };

        }

        public static AugmentArrangementViewModel MapToViewModel(this AugmentArrangement model)
        {
            return new AugmentArrangementViewModel
            {
                Id = model.Id,
                PatchId = model.PatchId,
                AugmentSlots = model.AugmentSlots.Select(AugmentSlotMap.MapToViewModel)
            };
        }
    }
}
