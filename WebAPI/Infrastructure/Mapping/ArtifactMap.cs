using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Infrastructure.Mapping
{
    public static class ArtifactMap
    {
        public static ArtifactEvent MapToDTO(this ArtifactViewModel viewModel)
        {
            return new ArtifactEvent()
            {
                ArtifactId = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                ImagePath = viewModel.ImagePath,
                ArtifactTypeId = viewModel.ArtifactTypeId,
                PatchId = viewModel.PatchId,
            };
        }

        public static ArtifactViewModel MapToViewModel(this ArtifactEvent model)
        {
            return new ArtifactViewModel()
            {
                Id = model.ArtifactId,
                Name = model.Name,
                Description = model.Description,
                ImagePath = model.ImagePath,
                ArtifactTypeId = model.ArtifactTypeId,
                PatchId = model.PatchId,
            };
        }
    }
}
