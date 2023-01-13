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
    public class HeroesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public HeroesController(ApplicationDbContext context, IChangeLogger changeLogger)
        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        // GET: api/Heroes
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HeroViewModel>>> GetHero()
        {
            var heroes = await context.Hero.ToListAsync();

            return Ok(heroes.Select(HeroMap.MapToViewModel));
        }

        // GET: api/Heroes/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<HeroViewModel>> GetHero(int id)
        {
            var hero = await context.Hero.FindAsync(id);

            if (hero == null)
            {
                return NotFound();
            }

            return hero.MapToViewModel();
        }

        // PUT: api/Heroes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHero(int id, HeroViewModel heroViewModel)
        {
            if (id != heroViewModel.Id)
            {
                return BadRequest();
            }

            var existing = await context.Hero.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            
            var hero = heroViewModel.MapToDTO();
            var entry = context.Entry(hero);
            entry.State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                await changeLogger.Log(User, entry.Entity, existing);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeroExists(id))
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

        // POST: api/Heroes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AugmentViewModel>> PostHero(HeroViewModel heroViewModel)
        {
            var hero = heroViewModel.MapToDTO();
            context.Hero.Add(hero);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, hero);

            return CreatedAtAction("GetHero", new { id = hero.Id }, heroViewModel);
        }

        // DELETE: api/Heroes/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteHero(int id)
        {
            var hero = await context.Hero.FindAsync(id);
            if (hero == null)
            {
                return NotFound();
            }

            context.Hero.Remove(hero);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, hero);

            return NoContent();
        }

        private bool HeroExists(int id)
        {
            return context.Hero.Any(e => e.Id == id);
        }
    }
}
