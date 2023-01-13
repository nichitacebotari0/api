using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure;
using WebAPI.Infrastructure.Mapping;
using WebAPI.Models;
using WebAPI.Models.Discord;
using WebAPI.Services;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "UserBuildEdit")]
    public class BuildController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IChangeLogger changeLogger;

        public BuildController(ApplicationDbContext context, IChangeLogger changeLogger)
        {
            dbContext = context;
            this.changeLogger = changeLogger;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<BuildViewModel>> Get(int id)
        {
            var build = await dbContext.Build.FindAsync(id);

            if (build == null)
            {
                return NotFound();
            }

            return Ok(build.MapToViewModel());
        }

        [HttpGet("hero/{heroId}")]
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

        [HttpGet("myvotes")]
        public async Task<ActionResult<IEnumerable<BuildVoteViewModel>>> Get([FromQuery] string builds)
        {
            var buildIds = builds.Split(',').Select(x => int.Parse(x)).ToArray();
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (userIdClaim == null)
            {
                return BadRequest("no user id present");
            }
            var votes = await dbContext.BuildVote.Where(x => x.DiscordUserId == userIdClaim && buildIds.Contains(x.BuildId)).ToListAsync();

            return Ok(votes.Select(BuildVoteMap.MapToViewModel));
        }

        [HttpGet("{buildId}/myvote")]
        public async Task<ActionResult<BuildVoteViewModel>> GetVote(int buildId)
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
            using var transaction = dbContext.Database.BeginTransaction();
            var vote = await dbContext.BuildVote
                .Include(x => x.Build)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (vote == null)
            {
                return NotFound();
            }
            var previousVoteLog = GetVoteLog(vote);

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (vote.DiscordUserId != userIdClaim)
            {
                return Forbid();
            }

            if (buildVoteViewModel.IsUpvote == vote.VoteValue > 0)
                return BadRequest();

            vote.VoteValue = buildVoteViewModel.IsUpvote ? 1 : -1;

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

            await dbContext.SaveChangesAsync();
            await changeLogger.Log(nameof(BuildVote), User, GetVoteLog(vote), previousVoteLog);
            transaction.Commit();

            return NoContent();
        }

        [HttpPost("vote")]
        public async Task<ActionResult<BuildViewModel>> CreateVote(BuildVoteViewModel buildVoteViewModel)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (userIdClaim == null)
            {
                return BadRequest("no user id present");
            }

            using var transaction = dbContext.Database.BeginTransaction();
            var build = await dbContext.Build.FindAsync(buildVoteViewModel.BuildId);
            if (build == null)
            {
                return BadRequest("such a build does not exist");
            }

            var vote = BuildVoteMap.MapToDTO(buildVoteViewModel);
            vote.DiscordUserId = userIdClaim;
            dbContext.BuildVote.Add(vote);

            if (buildVoteViewModel.IsUpvote)
            {
                build.Upvotes += 1;
            }
            else
            {
                build.Downvotes += 1;
            }

            await dbContext.SaveChangesAsync();
            await changeLogger.Log(User, GetVoteLog(vote));
            buildVoteViewModel.Id = vote.Id;
            transaction.Commit();

            return CreatedAtAction("GetVote", new { buildId = build.Id }, buildVoteViewModel);
        }

        [HttpDelete("vote/{id}")]
        public async Task<IActionResult> DeleteVote(int id)
        {
            using var transaction = dbContext.Database.BeginTransaction();
            var vote = await dbContext.BuildVote
                .Include(x => x.Build)
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

            if (vote.VoteValue > 0)
            {
                vote.Build.Upvotes -= 1;
            }
            else
            {
                vote.Build.Downvotes -= 1;
            }

            dbContext.BuildVote.Remove(vote);
            await dbContext.SaveChangesAsync();
            await changeLogger.Log(User, null, GetVoteLog(vote));
            transaction.Commit();

            return NoContent();
        }


        private static string GetVoteLog(BuildVote vote)
        {
            return vote.ToString() + $" buildVotes:+{vote.Build.Upvotes} -{vote.Build.Downvotes}";
        }
    }
}
