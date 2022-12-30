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
    public class ArtifactTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtifactTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ArtifactTypes
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ArtifactType>>> GetArtifactType()
        {
            return await _context.ArtifactType.ToListAsync();
        }

        // GET: api/ArtifactTypes/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArtifactType>> GetArtifactType(int id)
        {
            var artifactType = await _context.ArtifactType.FindAsync(id);

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

            _context.Entry(artifactType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
            _context.ArtifactType.Add(artifactType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtifactType", new { id = artifactType.Id }, artifactType);
        }

        // DELETE: api/ArtifactTypes/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteArtifactType(int id)
        {
            var artifactType = await _context.ArtifactType.FindAsync(id);
            if (artifactType == null)
            {
                return NotFound();
            }

            _context.ArtifactType.Remove(artifactType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtifactTypeExists(int id)
        {
            return _context.ArtifactType.Any(e => e.Id == id);
        }
    }
}
