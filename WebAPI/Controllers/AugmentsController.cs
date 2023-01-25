using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure;
using WebAPI.Infrastructure.Mapping;
using WebAPI.Models;
using WebAPI.Services;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AugmentEdit")]
    public class AugmentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public AugmentsController(ApplicationDbContext context, IChangeLogger changeLogger)
        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        // GET: api/Augments
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AugmentViewModel>>> GetAugment(int? heroId)
        {
            if (!heroId.HasValue)
                return BadRequest("Please specify a hero id");

            var augments = context.Augment.Where(Augment => Augment.HeroId == heroId);
            var result = await augments.ToListAsync();

            return Ok(result.Select(AugmentMap.MapToViewModel));
        }

        // GET: api/Augments/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AugmentViewModel>> GetAugment(int id)
        {
            var augment = await context.Augment.FindAsync(id);

            if (augment == null)
            {
                return NotFound();
            }

            return augment.MapToViewModel();
        }

        // PUT: api/Augments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAugment(int id, AugmentViewModel augmentViewModel)
        {
            if (id != augmentViewModel.Id)
            {
                return BadRequest();
            }

            var existing = await context.Augment.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null)
                return NotFound();

            var augment = augmentViewModel.MapToDTO();
            var entry = context.Entry(augment);
            entry.State = EntityState.Modified;

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

        // POST: api/Augments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AugmentViewModel>> PostAugment(AugmentViewModel augmentViewModel)
        {
            var augment = augmentViewModel.MapToDTO();
            context.Augment.Add(augment);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, augment, null);

            return CreatedAtAction("GetAugment", new { id = augment.Id }, augmentViewModel);
        }

        // DELETE: api/Augments/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteAugment(int id)
        {
            var augment = await context.Augment.FindAsync(id);
            if (augment == null)
            {
                return NotFound();
            }

            context.Augment.Remove(augment);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, augment);

            return NoContent();
        }
    }
}
