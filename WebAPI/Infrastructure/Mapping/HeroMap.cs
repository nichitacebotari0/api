using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Infrastructure.Mapping
{
    public static class HeroMap
    {
        public static Hero MapToDTO(this HeroViewModel viewModel)
        {
            return new Hero()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                ImagePath = viewModel.ImagePath,
                HeroClassId = viewModel.HeroClassId
            };
        }

        public static HeroViewModel MapToViewModel(this Hero model)
        {
            return new HeroViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                ImagePath = model.ImagePath,
                HeroClassId = model.HeroClassId
            };
        }
    }
}
