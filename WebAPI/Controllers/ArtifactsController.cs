using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure.Mapping;
using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AugmentEdit")]
    public class ArtifactsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ArtifactsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArtifactViewModel>>> GetArtifact([FromQuery] int patchId)
        {
            var patch = await context.Patch.FindAsync(patchId);
            if (patch == null)
                return BadRequest("No such patch");

            var patchEvents = await context.ArtifactEvent
                .Include(x => x.Patch)
                .Where(x => x.Patch.GameDate <= patch.GameDate)
                .GroupBy(x => x.ArtifactId)
                .Select(g => g.OrderByDescending(x => x.Id).First())
                .ToListAsync();

            var result = patchEvents.Where(x => x.Action == EventAction.SET);

            return Ok(result.Select(ArtifactMap.MapToViewModel));
        }

        [HttpPut]
        public async Task<IActionResult> PutArtifact(ArtifactViewModel artifactViewModel)
        {
            var artifactEvent = artifactViewModel.MapToDTO();
            context.ArtifactEvent.Add(artifactEvent);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ArtifactViewModel>> PostArtifact(ArtifactViewModel artifactViewModel)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            var artifactEvent = artifactViewModel.MapToDTO();
            artifactEvent.ArtifactId = await context.ArtifactEvent.MaxAsync(x => x.ArtifactId) + 1;
            context.ArtifactEvent.Add(artifactEvent);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(artifactEvent.MapToViewModel());
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteArtifact(int id, [FromQuery] int patchId)
        {
            var deleteEvent = new ArtifactEvent()
            {
                Action = EventAction.DELETE,
                ArtifactId = id,
                PatchId = patchId
            };

            context.ArtifactEvent.Add(deleteEvent);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
