using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class UserBuildsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserBuildsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserBuilds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildViewModel>>> GetBuild()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (userIdClaim == null)
            {
                return BadRequest("no user id present");
            }
            var builds = await _context.Build.Where(x => x.DiscordUserId == userIdClaim).ToListAsync();

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

            var build = await _context.Build.FindAsync(id);
            if (build == null)
            {
                return NotFound();
            }

            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (build.DiscordUserId != userIdClaim)
            {
                return Forbid();
            }

            build.Title = buildViewModel.Title;
            build.Augments = buildViewModel.Augments;
            build.Description = buildViewModel.Description;
            build.ModifiedAtUtc = DateTime.UtcNow;
            _context.Entry(build).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            _context.Build.Add(build);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuild", new { id = build.Id }, buildViewModel);
        }

        // DELETE: api/UserBuilds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuild(int id)
        {
            var build = await _context.Build.FindAsync(id);
            if (build == null)
            {
                return NotFound();
            }
            
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == DiscordConstants.Claim_userId)?.Value;
            if (build.DiscordUserId != userIdClaim)
            {
                return Forbid();
            }

            _context.Build.Remove(build);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BuildExists(int id)
        {
            return _context.Build.Any(e => e.Id == id);
        }
    }
}
