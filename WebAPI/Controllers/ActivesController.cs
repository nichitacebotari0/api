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
    public class ActivesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ActivesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/Actives
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<BoonViewModel>>> GetActive([FromQuery] int patchId)
        {
            var patch = await context.Patch.FindAsync(patchId);
            if (patch == null)
                return BadRequest("No such patch");

            var patchEvents = await context.BoonEvent
                .Include(x => x.Patch)
                .Where(x => x.Patch.GameDate <= patch.GameDate)
                .GroupBy(x => x.BoonId)
                .Select(g => g.OrderByDescending(x => x.Id).First())
                .ToListAsync();

            var result = patchEvents.Where(x => x.Action == EventAction.SET);

            return Ok(result.Select(BoonMap.MapToViewModel));
        }

        // PUT: api/Actives/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutActive(BoonViewModel boonViewModel)
        {
            var boonEvent = boonViewModel.MapToDTO();
            context.BoonEvent.Add(boonEvent);
            await context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Actives
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BoonViewModel>> PostActive(BoonViewModel boonViewModel)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            var boonEvent = boonViewModel.MapToDTO();
            boonEvent.BoonId = await context.BoonEvent.MaxAsync(x => x.BoonId) + 1;
            context.BoonEvent.Add(boonEvent);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(boonEvent.MapToViewModel());
        }

        // DELETE: api/Actives/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteActive(int id, [FromQuery] int patchId)
        {
            var deleteEvent = new BoonEvent()
            {
                Action = EventAction.DELETE,
                BoonId = id,
                PatchId = patchId
            };

            context.BoonEvent.Add(deleteEvent);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
