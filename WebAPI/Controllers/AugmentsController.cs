using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Infrastructure.Mapping;
using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AugmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AugmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Augments?HeroId
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<AugmentViewModel>>> GetAugmentForHero([Required] int heroId)
        //{
        //    var augments = await _context.Augment
        //        .Where(augment => augment.Id == heroId)
        //        .ToListAsync();
        //    if (!augments.Any())
        //    {
        //        return NotFound();
        //    }

        //    return Ok(augments.Select(AugmentMap.MapToViewModel));
        //}

        // GET: api/Augments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AugmentViewModel>>> GetAugment()
        {
            var augments = await _context.Augment.ToListAsync();

            return Ok(augments.Select(AugmentMap.MapToViewModel));
        }

        // GET: api/Augments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AugmentViewModel>> GetAugment(int id)
        {
            var augment = await _context.Augment.FindAsync(id);

            if (augment == null)
            {
                return NotFound();
            }

            return augment.MapToViewModel();
        }

        // PUT: api/Augments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAugment(int id, AugmentViewModel augmentViewModel)
        {
            if (id != augmentViewModel.Id)
            {
                return BadRequest();
            }

            var augment = augmentViewModel.MapToDTO();
            _context.Entry(augment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AugmentExists(id))
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

        // POST: api/Augments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AugmentViewModel>> PostAugment(AugmentViewModel augmentViewModel)
        {
            var augment = augmentViewModel.MapToDTO();
            _context.Augment.Add(augment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAugment", new { id = augment.Id }, augmentViewModel);
        }

        // DELETE: api/Augments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAugment(int id)
        {
            var augment = await _context.Augment.FindAsync(id);
            if (augment == null)
            {
                return NotFound();
            }

            _context.Augment.Remove(augment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AugmentExists(int id)
        {
            return _context.Augment.Any(e => e.Id == id);
        }
    }
}
