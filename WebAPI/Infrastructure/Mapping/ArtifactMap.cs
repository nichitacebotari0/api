using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Infrastructure.Mapping
{
    public static class ArtifactMap
    {
        public static Artifact MapToDTO(this ArtifactViewModel viewModel)
        {
            return new Artifact()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description,
                ImagePath = viewModel.ImagePath,
                ArtifactTypeId = viewModel.ArtifactTypeId,
            };
        }

        public static ArtifactViewModel MapToViewModel(this Artifact model)
        {
            return new ArtifactViewModel()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                ImagePath = model.ImagePath,
                ArtifactTypeId = model.ArtifactTypeId,
            };
        }
    }
}
