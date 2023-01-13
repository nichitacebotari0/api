using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AugmentEdit")]
    public class ChangesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ChangesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/Changes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Change>>> GetChangeLog([FromQuery] int? previousId, [FromQuery] int? pageDirection)
        {
            if (!pageDirection.HasValue)
                return await context.ChangeLog
                        .OrderByDescending(x => x.Id)
                        .Take(100)
                        .ToListAsync();

            if (!previousId.HasValue)
                return BadRequest("provide previous Id as well to get page");

            var changeLogs = pageDirection > 0
                ? context.ChangeLog.Where(x => x.Id < previousId)
                : context.ChangeLog.Where(x => x.Id > previousId);
            return await changeLogs
                    .OrderByDescending(x => x.Id)
                    .Take(100)
                    .ToListAsync();

        }

    }
}
