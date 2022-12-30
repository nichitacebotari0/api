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
    public class ActivesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActivesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Actives
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Active>>> GetActive()
        {
            return await _context.Active.ToListAsync();
        }

        // GET: api/Actives/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Active>> GetActive(int id)
        {
            var active = await _context.Active.FindAsync(id);

            if (active == null)
            {
                return NotFound();
            }

            return active;
        }

        // PUT: api/Actives/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActive(int id, Active active)
        {
            if (id != active.Id)
            {
                return BadRequest();
            }

            _context.Entry(active).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActiveExists(id))
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

        // POST: api/Actives
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Active>> PostActive(Active active)
        {
            _context.Active.Add(active);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActive", new { id = active.Id }, active);
        }

        // DELETE: api/Actives/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteActive(int id)
        {
            var active = await _context.Active.FindAsync(id);
            if (active == null)
            {
                return NotFound();
            }

            _context.Active.Remove(active);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActiveExists(int id)
        {
            return _context.Active.Any(e => e.Id == id);
        }
    }
}
