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
    public class HeroClassesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public HeroClassesController(ApplicationDbContext context, IChangeLogger changeLogger)
        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        // GET: api/HeroClasses
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HeroClass>>> GetHeroClass()
        {
            return await context.HeroClass.ToListAsync();
        }

        // GET: api/HeroClasses/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<HeroClass>> GetHeroClass(int id)
        {
            var heroClass = await context.HeroClass.FindAsync(id);

            if (heroClass == null)
            {
                return NotFound();
            }

            return heroClass;
        }

        // PUT: api/HeroClasses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHeroClass(int id, HeroClass heroClass)
        {
            if (id != heroClass.Id)
            {
                return BadRequest();
            }

            var existing = await context.HeroClass.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            var entry = context.Entry(heroClass);
            entry.State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                await changeLogger.Log(User, entry.Entity, existing);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeroClassExists(id))
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

        // POST: api/HeroClasses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HeroClass>> PostHeroClass(HeroClass heroClass)
        {
            context.HeroClass.Add(heroClass);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, heroClass);

            return CreatedAtAction("GetHeroClass", new { id = heroClass.Id }, heroClass);
        }

        // DELETE: api/HeroClasses/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteHeroClass(int id)
        {
            var heroClass = await context.HeroClass.FindAsync(id);
            if (heroClass == null)
            {
                return NotFound();
            }

            context.HeroClass.Remove(heroClass);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, heroClass);

            return NoContent();
        }

        private bool HeroClassExists(int id)
        {
            return context.HeroClass.Any(e => e.Id == id);
        }
    }
}
