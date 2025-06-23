using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMCPortal.API.Controllers
{
    // Expose under /api/leads instead of /api/TruvaiQuery
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public LeadsController(DMCPortalDBContext context)
        {
            _context = context;
        }

        // GET: api/leads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TruvaiQuery>>> GetAll()
        {
            try
            {
                var data = await _context.TruvaiQueries.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error fetching leads: " + ex.Message);
            }
        }

        // GET: api/leads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TruvaiQuery>> GetById(int id)
        {
            var query = await _context.TruvaiQueries.FindAsync(id);
            if (query == null) return NotFound();
            return query;
        }

        // POST: api/leads
        // Note: No custom route name, so this responds to POST /api/leads
        [HttpPost]

        public async Task<ActionResult<TruvaiQuery>> Create([FromBody] TruvaiQuery query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.TruvaiQueries.Add(query);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = query.Id }, query);
        }


        // PUT: api/leads/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TruvaiQuery query)
        {
            if (id != query.Id) return BadRequest();

            _context.Entry(query).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/leads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var query = await _context.TruvaiQueries.FindAsync(id);
            if (query == null) return NotFound();

            _context.TruvaiQueries.Remove(query);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
