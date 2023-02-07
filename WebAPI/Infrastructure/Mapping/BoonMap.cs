using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Infrastructure.Mapping
{
    public static class BoonMap
    {
        public static BoonEvent MapToDTO(this BoonViewModel viewModel)
        {
            return new BoonEvent()
            {
                BoonId = viewModel.Id,
                PatchId = viewModel.PatchId,
                Name = viewModel.Name,
                ImagePath = viewModel.ImagePath,
                Description = viewModel.Description,
            };
        }

        public static BoonViewModel MapToViewModel(this BoonEvent model)
        {
            return new BoonViewModel()
            {
                Id = model.BoonId,
                PatchId = model.PatchId,
                Name = model.Name,
                ImagePath = model.ImagePath,
                Description = model.Description,
            };
        }
    }
}
