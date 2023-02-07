using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Infrastructure.Mapping
{
    public static class BuildMap
    {
        public static Build MapToDTO(this BuildViewModel viewModel)
        {
            return new Build()
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Augments = viewModel.Augments,
                Description = viewModel.Description,
                ModifiedAtUtc = viewModel.ModifiedAtUtc,
                HeroId = viewModel.HeroId,
            };
        }

        public static BuildViewModel MapToViewModel(this Build model)
        {
            return new BuildViewModel()
            {
                Id = model.Id,
                DiscordUserId = model.DiscordUserId,
                Title = model.Title,
                Augments = model.Augments,
                Description = model.Description,
                CreatedAtUtc = model.CreatedAtUtc,
                ModifiedAtUtc = model.ModifiedAtUtc,
                Upvotes = model.Upvotes,
                Downvotes = model.Downvotes,
                HeroId = model.HeroId,
                PatchId = model.PatchId,
            };
        }
    }
}
