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
    public class ArtifactTypesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public ArtifactTypesController(ApplicationDbContext context, IChangeLogger changeLogger)

        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        // GET: api/ArtifactTypes
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArtifactType>>> GetArtifactType()
        {
            return await context.ArtifactType.ToListAsync();
        }

        // GET: api/ArtifactTypes/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArtifactType>> GetArtifactType(int id)
        {
            var artifactType = await context.ArtifactType.FindAsync(id);

            if (artifactType == null)
            {
                return NotFound();
            }

            return artifactType;
        }

        // PUT: api/ArtifactTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtifactType(int id, ArtifactType artifactType)
        {
            if (id != artifactType.Id)
            {
                return BadRequest();
            }

            var existing = await context.ArtifactType.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            
            var entry = context.Entry(artifactType);
            entry.State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                await changeLogger.Log(User, entry.Entity, existing);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtifactTypeExists(id))
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

        // POST: api/ArtifactTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArtifactType>> PostArtifactType(ArtifactType artifactType)
        {
            context.ArtifactType.Add(artifactType);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, artifactType, null);

            return CreatedAtAction("GetArtifactType", new { id = artifactType.Id }, artifactType);
        }

        // DELETE: api/ArtifactTypes/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteArtifactType(int id)
        {
            var artifactType = await context.ArtifactType.FindAsync(id);
            if (artifactType == null)
            {
                return NotFound();
            }

            context.ArtifactType.Remove(artifactType);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, artifactType);

            return NoContent();
        }

        private bool ArtifactTypeExists(int id)
        {
            return context.ArtifactType.Any(e => e.Id == id);
        }
    }
}
