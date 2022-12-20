using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure.Mapping;
using WebAPI.Models;
using WebAPI.Models.Discord;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "UserBuildEdit")]
    public class BuildController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public BuildController(ApplicationDbContext context)
        {
            dbContext = context;
        }

        [HttpGet("{heroId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BuildViewModel>>> Get(int heroId, [FromQuery] uint? previousVote, [FromQuery] uint? previousId)
        {
            var buildPage = dbContext.Build
                .Where(x => x.HeroId == heroId);
            if (previousVote.HasValue && previousId.HasValue)
                buildPage = buildPage.Where(x =>
                    (x.Upvotes - x.Downvotes) < previousVote ||
                    x.Upvotes - x.Downvotes == previousVote && x.Id > previousId);

            var builds = await buildPage
                .OrderByDescending(x => x.Upvotes - x.Downvotes)
                .ThenBy(x => x.Id)
                .Take(20)
                .ToListAsync();


            var buildViewModels = builds.Select(BuildMap.MapToViewModel);
            return Ok(buildViewModels);
        }

        [HttpGet("votes")]
        public async Task<ActionResult<IEnumerable<BuildViewModel>>> Get([FromQuery] string stringBuildIds)
        {
            var buildIds = stringBuildIds.Split(',').Select(x => int.Parse(x)).ToArray();
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (userIdClaim == null)
            {
                return BadRequest("no user id present");
            }
            var votes = await dbContext.BuildVote.FirstAsync(x => x.DiscordUserId == userIdClaim && buildIds.Contains(x.BuildId));

            return Ok(BuildVoteMap.MapToViewModel(votes));
        }

        [HttpGet("{buildId}/vote")]
        public async Task<ActionResult<BuildViewModel>> Get(int buildId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (userIdClaim == null)
            {
                return BadRequest("no user id present");
            }
            var vote = await dbContext.BuildVote.FirstAsync(x => x.DiscordUserId == userIdClaim && x.BuildId == buildId);

            return Ok(BuildVoteMap.MapToViewModel(vote));
        }

        [HttpPut("vote/{id}")]
        public async Task<IActionResult> ChangeVote(int id, BuildVoteViewModel buildVoteViewModel)
        {
            if (id != buildVoteViewModel.Id)
            {
                return BadRequest();
            }

            var vote = await dbContext.BuildVote
                .Include(x => x.BuildId)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (vote.DiscordUserId != userIdClaim)
            {
                return Forbid();
            }

            if (buildVoteViewModel.IsUpvote == vote.VoteValue > 0)
                return NoContent();

            vote.VoteValue = buildVoteViewModel.IsUpvote ? 1 : -1;
            dbContext.Entry(vote).Property(x => x.VoteValue).IsModified = true;

            if (buildVoteViewModel.IsUpvote)
            {
                vote.Build.Upvotes += 1;
                vote.Build.Downvotes -= 1;
            }
            else
            {
                vote.Build.Upvotes -= 1;
                vote.Build.Downvotes += 1;
            }
            var buildEntry = dbContext.Entry(vote.Build);
            buildEntry.Property(x => x.Upvotes).IsModified = true;
            buildEntry.Property(x => x.Downvotes).IsModified = true;

            await dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("vote")]
        public async Task<IActionResult> Vote(BuildVoteViewModel buildVoteViewModel)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (userIdClaim == null)
            {
                return BadRequest("no user id present");
            }

            var build = await dbContext.Build.FindAsync(buildVoteViewModel.BuildId);
            if (buildVoteViewModel.IsUpvote)
            {
                build.Upvotes += 1;
            }
            else
            {
                build.Downvotes += 1;
            }

            var vote = BuildVoteMap.MapToDTO(buildVoteViewModel);
            vote.DiscordUserId = userIdClaim;
            dbContext.BuildVote.Add(vote);

            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
