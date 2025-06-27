using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMCPortal.API.Entities;

namespace DMCPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesVisitController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public SalesVisitController(DMCPortalDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesVisit>>> GetSalesVisits()
        {
            return await _context.SalesVisits.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SalesVisit>> GetSalesVisit(int id)
        {
            var visit = await _context.SalesVisits.FindAsync(id);
            if (visit == null)
                return NotFound();
            return visit;

        }

        [HttpPost]
        public async Task<ActionResult<SalesVisit>> PostSalesVisit([FromBody] SalesVisit visit)
        {
            visit.CreatedOn = DateTime.Now; // Ensure CreatedOn is set

            _context.SalesVisits.Add(visit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalesVisit), new { id = visit.SalesVisitId }, visit);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesVisit(int id, SalesVisit visit)
        {
            if (id != visit.SalesVisitId)
                return BadRequest();

            _context.Entry(visit).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesVisit(int id)
        {
            var visit = await _context.SalesVisits.FindAsync(id);
            if (visit == null)
                return NotFound();

            _context.SalesVisits.Remove(visit);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}