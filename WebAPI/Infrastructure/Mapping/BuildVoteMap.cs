using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Infrastructure.Mapping
{
    public static class BuildVoteMap
    {
        public static BuildVote MapToDTO(this BuildVoteViewModel viewModel)
        {
            return new BuildVote()
            {
                Id = viewModel.Id,
                BuildId = viewModel.BuildId,
                VoteValue = viewModel.IsUpvote ? 1 : -1
            };
        }

        public static BuildVoteViewModel MapToViewModel(this BuildVote model)
        {
            return new BuildVoteViewModel()
            {
                Id = model.Id,
                BuildId = model.BuildId,
                IsUpvote = model.VoteValue > 0
            };
        }
    }
}
