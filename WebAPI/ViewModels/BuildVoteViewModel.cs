namespace WebAPI.ViewModels
{
    public class BuildVoteViewModel
    {
        public int Id { get; set; }
        public int BuildId { get; set; }
        public bool IsUpvote { get; set; }
    }
}
