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
    public class AbilityTypesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public AbilityTypesController(ApplicationDbContext context, IChangeLogger changeLogger)
        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        // GET: api/AbilityTypes
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AbilityType>>> GetAbilityType()
        {
            return await context.AbilityType.ToListAsync();
        }

        // GET: api/AbilityTypes/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AbilityType>> GetAbilityType(int id)
        {
            var abilityType = await context.AbilityType.FindAsync(id);

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

            var existing = await context.AbilityType.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            var entry = context.Entry(abilityType);
            entry.State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                await changeLogger.Log(User, entry.Entity, existing);
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
            context.AbilityType.Add(abilityType);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, abilityType);


            return CreatedAtAction("GetAbilityType", new { id = abilityType.Id }, abilityType);
        }

        // DELETE: api/AbilityTypes/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteAbilityType(int id)
        {
            var abilityType = await context.AbilityType.FindAsync(id);
            if (abilityType == null)
            {
                return NotFound();
            }

            context.AbilityType.Remove(abilityType);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, abilityType);

            return NoContent();
        }

        private bool AbilityTypeExists(int id)
        {
            return context.AbilityType.Any(e => e.Id == id);
        }
    }
}
