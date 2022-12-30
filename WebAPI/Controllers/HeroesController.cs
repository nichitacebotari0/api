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
    public class HeroesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HeroesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Heroes
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HeroViewModel>>> GetHero()
        {
            var heroes = await _context.Hero.ToListAsync();

            return Ok(heroes.Select(HeroMap.MapToViewModel));
        }

        // GET: api/Heroes/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<HeroViewModel>> GetHero(int id)
        {
            var hero = await _context.Hero.FindAsync(id);

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

            var hero = heroViewModel.MapToDTO();
            _context.Entry(hero).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            _context.Hero.Add(hero);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHero", new { id = hero.Id }, heroViewModel);
        }

        // DELETE: api/Heroes/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteHero(int id)
        {
            var hero = await _context.Hero.FindAsync(id);
            if (hero == null)
            {
                return NotFound();
            }

            _context.Hero.Remove(hero);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HeroExists(int id)
        {
            return _context.Hero.Any(e => e.Id == id);
        }
    }
}
