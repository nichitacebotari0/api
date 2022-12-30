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
    public class AugmentCategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AugmentCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AugmentCategories
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AugmentCategory>>> GetAugmentCategory()
        {
            return await _context.AugmentCategory.ToListAsync();
        }

        // GET: api/AugmentCategories/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<AugmentCategory>> GetAugmentCategory(int id)
        {
            var augmentCategory = await _context.AugmentCategory.FindAsync(id);

            if (augmentCategory == null)
            {
                return NotFound();
            }

            return augmentCategory;
        }

        // PUT: api/AugmentCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAugmentCategory(int id, AugmentCategory augmentCategory)
        {
            if (id != augmentCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(augmentCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AugmentCategoryExists(id))
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

        // POST: api/AugmentCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AugmentCategory>> PostAugmentCategory(AugmentCategory augmentCategory)
        {
            _context.AugmentCategory.Add(augmentCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAugmentCategory", new { id = augmentCategory.Id }, augmentCategory);
        }

        // DELETE: api/AugmentCategories/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "Super")]
        public async Task<IActionResult> DeleteAugmentCategory(int id)
        {
            var augmentCategory = await _context.AugmentCategory.FindAsync(id);
            if (augmentCategory == null)
            {
                return NotFound();
            }

            _context.AugmentCategory.Remove(augmentCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AugmentCategoryExists(int id)
        {
            return _context.AugmentCategory.Any(e => e.Id == id);
        }
    }
}
