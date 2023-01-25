using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure;
using WebAPI.Infrastructure.Mapping;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Super")]
    public class AugmentArrangementController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public AugmentArrangementController(ApplicationDbContext context, IChangeLogger changeLogger)
        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AugmentArrangementViewModel>>> GetAugmentArrangement()
        {
            var arrangements = await context.AugmentArrangement
                .Include(x => x.AugmentSlots)
                .ToListAsync();

            return Ok(arrangements.Select(AugmentArrangementMap.MapToViewModel));
        }

        [HttpGet("patch/{patchId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AugmentArrangementViewModel>>> GetByPatchId(int patchId)
        {
            var arrangements = await context.AugmentArrangement
                .Include(x => x.AugmentSlots)
                .Where(arrangement => arrangement.PatchId == patchId)
                .ToListAsync();

            return Ok(arrangements.Select(AugmentArrangementMap.MapToViewModel));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AugmentArrangementViewModel>> GetAugmentArrangement(int id)
        {
            var augment = await context.AugmentArrangement
                .Include(x => x.AugmentSlots)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (augment == null)
                return NotFound();

            return augment.MapToViewModel();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAugmentArrangement(int id, AugmentArrangementViewModel arrangementViewModel)
        {
            if (id != arrangementViewModel.Id)
                return BadRequest();

            var existing = await context.AugmentArrangement
                .Include(x => x.AugmentSlots)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null)
                return NotFound();

            var arrangement = arrangementViewModel.MapToDTO();
            var entry = context.Update(arrangement);

            try
            {
                await context.SaveChangesAsync();
                await changeLogger.Log(User, entry.Entity, existing);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<AugmentArrangementViewModel>> PostAugmentArrangement(AugmentArrangementViewModel arrangementViewModel)
        {
            var arrangement = arrangementViewModel.MapToDTO();
            context.AugmentArrangement.Add(arrangement);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, arrangement, null);

            return CreatedAtAction("GetAugmentArrangement", new { id = arrangement.Id }, arrangementViewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAugmentArrangement(int id)
        {
            var arrangement = await context.AugmentArrangement.FindAsync(id);
            if (arrangement == null)
                return NotFound();

            context.AugmentArrangement.Remove(arrangement);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, arrangement);

            return NoContent();
        }
    }
}
