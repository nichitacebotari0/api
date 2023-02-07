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
    public class AugmentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AugmentsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AugmentViewModel>>> GetAugment([FromQuery] int heroId, [FromQuery] int patchId)
        {
            var patch = await context.Patch.FindAsync(patchId);
            if (patch == null)
                return BadRequest("No such patch");

            var patchEvents = await context.AugmentEvent
                .Include(x => x.Patch)
                .Where(x => x.HeroId == heroId && x.Patch.GameDate <= patch.GameDate)
                .GroupBy(x => x.AugmentId)
                .Select(g => g.OrderByDescending(x => x.Id).First())
                .ToListAsync();

            var result = patchEvents.Where(x => x.Action == EventAction.SET);

            return Ok(result.Select(AugmentEventMap.MapToViewModel));
        }

        [HttpPut]
        public async Task<ActionResult<AugmentViewModel>> PutAugment(AugmentViewModel augmentViewModel)
        {
            var augment = augmentViewModel.MapToDTO();
            context.AugmentEvent.Add(augment);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AugmentViewModel>> PostAugment(AugmentViewModel augmentViewModel)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            var augmentEvent = augmentViewModel.MapToDTO();
            augmentEvent.AugmentId = await context.AugmentEvent.MaxAsync(x => x.AugmentId) + 1;
            context.AugmentEvent.Add(augmentEvent);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(augmentEvent.MapToViewModel());
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteAugment(int id, [FromQuery] int heroId, [FromQuery] int patchId)
        {
            var deleteEvent = new AugmentEvent()
            {
                Action = EventAction.DELETE,
                AugmentId = id,
                PatchId = patchId,
                HeroId = heroId
            };

            context.AugmentEvent.Add(deleteEvent);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
