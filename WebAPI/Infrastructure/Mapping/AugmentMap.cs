using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Infrastructure.Mapping
{
    public static class AugmentMap
    {
        public static Augment MapToDTO(this AugmentViewModel viewModel)
        {
            return new Augment()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                ImagePath = viewModel.ImagePath,
                AugmentCategoryId = viewModel.AugmentCategoryId,
                AbilityTypeId = viewModel.AbilityTypeId,
                HeroId = viewModel.HeroId
            };
        }

        public static AugmentViewModel MapToViewModel(this Augment model)
        {
            return new AugmentViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                ImagePath = model.ImagePath,
                AugmentCategoryId = model.AugmentCategoryId,
                AbilityTypeId = model.AbilityTypeId,
                HeroId = model.HeroId
            };
        }
    }
}
