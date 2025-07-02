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
            // Remove claim check
            if (visit.CreatedBy <= 0)
                return BadRequest("CreatedBy is required.");

            visit.CreatedOn = DateTime.Now;

            _context.SalesVisits.Add(visit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalesVisit), new { id = visit.SalesVisitId }, visit);
        }


        [HttpPut("{id}")]
       public async Task<IActionResult> PutSalesVisit(int id, SalesVisit visit)
        {
              if (id != visit.SalesVisitId)
         return BadRequest();

        var existing = await _context.SalesVisits.FindAsync(id);
        if (existing == null)
        return NotFound();

       // Preserve CreatedBy and CreatedOn
       visit.CreatedBy = existing.CreatedBy;
      visit.CreatedOn = existing.CreatedOn;

      // Set UpdatedBy and UpdatedOn
       visit.UpdatedBy = visit.UpdatedBy;  // Set from client-side or API (recommend setting in API)
      visit.UpdatedOn = DateTime.Now;

          _context.Entry(existing).CurrentValues.SetValues(visit);
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
        [HttpGet("UserWise/{userId}")]
        public async Task<ActionResult<IEnumerable<SalesVisit>>> GetSalesByUser(int userId)
        {
            try
            {
                Console.WriteLine("UserWise API Called. UserId: " + userId);

                var sales = await _context.SalesVisits
                                          .Where(s => s.CreatedBy == userId)
                                          .OrderByDescending(s => s.VisitDate)
                                          .ToListAsync();

                return Ok(sales);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR in GetSalesByUser: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }



    }
}