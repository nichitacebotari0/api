using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Infrastructure.Mapping
{
    public static class AugmentEventMap
    {
        public static AugmentEvent MapToDTO(this AugmentViewModel viewModel)
        {
            return new AugmentEvent()
            {
                AugmentId = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                ImagePath = viewModel.ImagePath,
                AugmentCategoryId = viewModel.AugmentCategoryId,
                AbilityTypeId = viewModel.AbilityTypeId,
                HeroId = viewModel.HeroId,
                PatchId = viewModel.PatchId,
            };
        }

        public static AugmentViewModel MapToViewModel(this AugmentEvent model)
        {
            return new AugmentViewModel()
            {
                Id = model.AugmentId,
                Name = model.Name,
                Description = model.Description,
                ImagePath = model.ImagePath,
                AugmentCategoryId = model.AugmentCategoryId,
                AbilityTypeId = model.AbilityTypeId,
                HeroId = model.HeroId,
                PatchId = model.PatchId,
            };
        }
    }
}
