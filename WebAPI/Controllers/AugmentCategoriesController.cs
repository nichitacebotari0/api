using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AugmentEdit")]
    public class AugmentCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public AugmentCategoriesController(ApplicationDbContext context, IChangeLogger changeLogger)
        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        // GET: api/AugmentCategories
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AugmentCategory>>> GetAugmentCategory()
        {
            return await context.AugmentCategory.ToListAsync();
        }

        // GET: api/AugmentCategories/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AugmentCategory>> GetAugmentCategory(int id)
        {
            var augmentCategory = await context.AugmentCategory.FindAsync(id);

            if (augmentCategory == null)
            {
                return NotFound();
            }

            return augmentCategory;
        }

        // PUT: api/AugmentCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAugmentCategory(int id, AugmentCategory augmentCategory)
        {
            if (id != augmentCategory.Id)
            {
                return BadRequest();
            }

            var existing = await context.AugmentCategory.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
           
            var entry = context.Entry(augmentCategory);
            entry.State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                await changeLogger.Log(User, entry.Entity, existing);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AugmentCategoryExists(id))
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

        // POST: api/AugmentCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AugmentCategory>> PostAugmentCategory(AugmentCategory augmentCategory)
        {
            context.AugmentCategory.Add(augmentCategory);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, augmentCategory, null);

            return CreatedAtAction("GetAugmentCategory", new { id = augmentCategory.Id }, augmentCategory);
        }

        // DELETE: api/AugmentCategories/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteAugmentCategory(int id)
        {
            var augmentCategory = await context.AugmentCategory.FindAsync(id);
            if (augmentCategory == null)
            {
                return NotFound();
            }

            context.AugmentCategory.Remove(augmentCategory);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, augmentCategory);

            return NoContent();
        }

        private bool AugmentCategoryExists(int id)
        {
            return context.AugmentCategory.Any(e => e.Id == id);
        }
    }
}
