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
    public class ActivesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public ActivesController(ApplicationDbContext context, IChangeLogger changeLogger)
        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        // GET: api/Actives
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Active>>> GetActive()
        {
            return await context.Active.ToListAsync();
        }

        // GET: api/Actives/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Active>> GetActive(int id)
        {
            var active = await context.Active.FindAsync(id);

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

            var existing = await context.Active.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            var entry = context.Entry(active);
            entry.State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                await changeLogger.Log(User, entry.Entity, existing);
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
            context.Active.Add(active);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, active, null);

            return CreatedAtAction("GetActive", new { id = active.Id }, active);
        }

        // DELETE: api/Actives/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteActive(int id)
        {
            var active = await context.Active.FindAsync(id);
            if (active == null)
            {
                return NotFound();
            }

            context.Active.Remove(active);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, active);

            return NoContent();
        }

        private bool ActiveExists(int id)
        {
            return context.Active.Any(e => e.Id == id);
        }
    }
}
