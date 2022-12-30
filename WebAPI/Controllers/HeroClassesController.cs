using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AugmentEdit")]
    public class HeroClassesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HeroClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/HeroClasses
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<HeroClass>>> GetHeroClass()
        {
            return await _context.HeroClass.ToListAsync();
        }

        // GET: api/HeroClasses/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<HeroClass>> GetHeroClass(int id)
        {
            var heroClass = await _context.HeroClass.FindAsync(id);

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

            _context.Entry(heroClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            _context.HeroClass.Add(heroClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHeroClass", new { id = heroClass.Id }, heroClass);
        }

        // DELETE: api/HeroClasses/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteHeroClass(int id)
        {
            var heroClass = await _context.HeroClass.FindAsync(id);
            if (heroClass == null)
            {
                return NotFound();
            }

            _context.HeroClass.Remove(heroClass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HeroClassExists(int id)
        {
            return _context.HeroClass.Any(e => e.Id == id);
        }
    }
}
