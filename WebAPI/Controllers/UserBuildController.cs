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
    public class UserBuildController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public UserBuildController(ApplicationDbContext context,
            IChangeLogger changeLogger)
        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        // GET: api/UserBuilds
        [HttpGet()]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BuildViewModel>>> GetBuild([FromQuery] string userId)
        {
            var builds = await context.Build.Where(x => x.DiscordUserId == userId).ToListAsync();
            return Ok(builds.Select(BuildMap.MapToViewModel));
        }

        // PUT: api/UserBuilds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuild(int id, BuildViewModel buildViewModel)
        {
            if (id != buildViewModel.Id)
            {
                return BadRequest();
            }

            var build = await context.Build.FindAsync(id);
            if (build == null)
            {
                return NotFound();
            }

            var previousLog = build.ToString();
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (build.DiscordUserId != userIdClaim)
            {
                return Forbid("This is not your build so you cannot edit it");
            }

            build.Title = buildViewModel.Title;
            build.Augments = buildViewModel.Augments;
            build.Description = buildViewModel.Description;
            build.ModifiedAtUtc = DateTime.UtcNow;

            try
            {
                await context.SaveChangesAsync();
                await changeLogger.Log(nameof(Build), User, build.ToString(), previousLog);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuildExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserBuilds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BuildViewModel>> PostBuild(BuildViewModel buildViewModel)
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (userIdClaim == null)
            {
                return BadRequest("no user id present");
            }

            var build = buildViewModel.MapToDTO();
            build.CreatedAtUtc = DateTime.UtcNow;
            build.DiscordUserId = userIdClaim;
            context.Build.Add(build);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, build);

            return CreatedAtAction("GetBuild", new { id = build.Id }, build.MapToViewModel());
        }

        // DELETE: api/UserBuilds/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteBuild(int id)
        {
            var build = await context.Build.FindAsync(id);
            if (build == null)
            {
                return NotFound();
            }

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (build.DiscordUserId != userIdClaim)
            {
                return Forbid();
            }

            context.Build.Remove(build);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, build);

            return NoContent();
        }

        private bool BuildExists(int id)
        {
            return context.Build.Any(e => e.Id == id);
        }
    }
}
