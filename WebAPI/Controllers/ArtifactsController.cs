using System;
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
    public class ArtifactsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IChangeLogger changeLogger;

        public ArtifactsController(ApplicationDbContext context, IChangeLogger changeLogger)
        {
            this.context = context;
            this.changeLogger = changeLogger;
        }

        // GET: api/Artifacts
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArtifactViewModel>>> GetArtifact()
        {
            var artifacts = await context.Artifact.ToListAsync();

            return Ok(artifacts.Select(ArtifactMap.MapToViewModel));
        }

        // GET: api/Artifacts/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArtifactViewModel>> GetArtifact(int id)
        {
            var artifact = await context.Artifact.FindAsync(id);

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

            var existing = await context.Artifact.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            var artifact = artifactViewModel.MapToDTO();
            var entry = context.Entry(artifact);
            entry.State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
                await changeLogger.Log(User, entry.Entity, existing);
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
            context.Artifact.Add(artifact);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, artifact, null);

            return CreatedAtAction("GetArtifact", new { id = artifact.Id }, artifactViewModel);
        }

        // DELETE: api/Artifacts/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteArtifact(int id)
        {
            var artifact = await context.Artifact.FindAsync(id);
            if (artifact == null)
            {
                return NotFound();
            }

            context.Artifact.Remove(artifact);
            await context.SaveChangesAsync();
            await changeLogger.Log(User, null, artifact);

            return NoContent();
        }

        private bool ArtifactExists(int id)
        {
            return context.Artifact.Any(e => e.Id == id);
        }
    }
}
