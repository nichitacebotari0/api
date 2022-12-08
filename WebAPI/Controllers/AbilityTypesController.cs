using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbilityTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AbilityTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AbilityTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AbilityType>>> GetAbilityType()
        {
            return await _context.AbilityType.ToListAsync();
        }

        // GET: api/AbilityTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AbilityType>> GetAbilityType(int id)
        {
            var abilityType = await _context.AbilityType.FindAsync(id);

            if (abilityType == null)
            {
                return NotFound();
            }

            return abilityType;
        }

        // PUT: api/AbilityTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAbilityType(int id, AbilityType abilityType)
        {
            if (id != abilityType.Id)
            {
                return BadRequest();
            }

            _context.Entry(abilityType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AbilityTypeExists(id))
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

        // POST: api/AbilityTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AbilityType>> PostAbilityType(AbilityType abilityType)
        {
            _context.AbilityType.Add(abilityType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAbilityType", new { id = abilityType.Id }, abilityType);
        }

        // DELETE: api/AbilityTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbilityType(int id)
        {
            var abilityType = await _context.AbilityType.FindAsync(id);
            if (abilityType == null)
            {
                return NotFound();
            }

            _context.AbilityType.Remove(abilityType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AbilityTypeExists(int id)
        {
            return _context.AbilityType.Any(e => e.Id == id);
        }
    }
}
