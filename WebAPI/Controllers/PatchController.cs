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
    [Authorize(Policy = "Super")]
    public class PatchController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public PatchController(ApplicationDbContext context, IChangeLogger changeLogger)
        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Patch>>> GetPatch()
        {
            return await context.Patch.ToListAsync();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Patch>> GetPatch(int id)
        {
            var patch = await context.Patch.FindAsync(id);

            if (patch == null)
            {
                return NotFound();
            }

            return patch;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatch(int id, Patch patch)
        {
            if (id != patch.Id)
                return BadRequest();

            var existing = await context.Patch.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (existing == null)
                return NotFound();

            var entry = context.Entry(patch);
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

        [HttpPost]
        public async Task<ActionResult<Patch>> PostPatch(Patch patch)
        {
            context.Patch.Add(patch);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, patch, null);

            return CreatedAtAction("GetPatch", new { id = patch.Id }, patch);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatch(int id)
        {
            var patch = await context.Patch.FindAsync(id);
            if (patch == null)
                return NotFound();

            context.Patch.Remove(patch);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, patch);

            return NoContent();
        }
    }
}
