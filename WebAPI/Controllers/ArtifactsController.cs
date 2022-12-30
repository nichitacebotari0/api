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
    public class ArtifactsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtifactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Artifacts
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArtifactViewModel>>> GetArtifact()
        {
            var artifacts = await _context.Artifact.ToListAsync();

            return Ok(artifacts.Select(ArtifactMap.MapToViewModel));
        }

        // GET: api/Artifacts/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArtifactViewModel>> GetArtifact(int id)
        {
            var artifact = await _context.Artifact.FindAsync(id);

            if (artifact == null)
            {
                return NotFound();
            }

            return artifact.MapToViewModel();
        }

        // PUT: api/Artifacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtifact(int id, ArtifactViewModel artifactViewModel)
        {
            if (id != artifactViewModel.Id)
            {
                return BadRequest();
            }

            var artifact = artifactViewModel.MapToDTO();
            _context.Entry(artifact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtifactExists(id))
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

        // POST: api/Artifacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ArtifactViewModel>> PostArtifact(ArtifactViewModel artifactViewModel)
        {
            var artifact = artifactViewModel.MapToDTO();
            _context.Artifact.Add(artifact);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtifact", new { id = artifact.Id }, artifactViewModel);
        }

        // DELETE: api/Artifacts/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteArtifact(int id)
        {
            var artifact = await _context.Artifact.FindAsync(id);
            if (artifact == null)
            {
                return NotFound();
            }

            _context.Artifact.Remove(artifact);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtifactExists(int id)
        {
            return _context.Artifact.Any(e => e.Id == id);
        }
    }
}
