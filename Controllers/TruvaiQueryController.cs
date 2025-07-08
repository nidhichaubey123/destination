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

        [HttpGet]
        public async Task<IActionResult> GetLeads([FromQuery] string? search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var query = _context.TruvaiQueries.Where(x => !x.IsDeleted).AsQueryable();

                // Server-side Search
                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(q =>
                        q.Name.Contains(search) ||
                        q.Email.Contains(search) ||
                        q.Phone.Contains(search) ||
                        q.Destination.Contains(search) ||
                        q.Zone.Contains(search)
                    );
                }

                // Total Count for Pagination
                var totalRecords = await query.CountAsync();

                // Sorting by Name Ascending
                query = query.OrderBy(q => q.Name);

                // Pagination
                var leads = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(new
                {
                    TotalRecords = totalRecords,
                    Leads = leads
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error fetching leads: " + ex.Message);
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var lead = _context.TruvaiQueries.FirstOrDefault(l => l.Id == id);

            if (lead == null)
                return NotFound();

            return Ok(lead);
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


        [HttpDelete("{id}")]
        public IActionResult DeleteLead(int id, [FromQuery] string deletedBy)
        {
            var lead = _context.TruvaiQueries.FirstOrDefault(l => l.Id == id);
            if (lead == null)
                return NotFound();

            lead.IsDeleted = true;
            lead.DeletedOn = DateTime.Now;
            lead.DeletedBy = string.IsNullOrEmpty(deletedBy) ? "System" : deletedBy;

            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateLead([FromBody] TruvaiQuery query)
        {
            if (query == null || query.Id <= 0)
                return BadRequest("Invalid data");

            var existing = await _context.TruvaiQueries.FindAsync(query.Id);
            if (existing == null)
                return NotFound("Lead not found");

            // Only update fields that were provided - avoid overwriting existing values with defaults
            existing.Name = query.Name ?? existing.Name;
            existing.Email = query.Email ?? existing.Email;
            existing.Phone = query.Phone ?? existing.Phone;
            existing.Zone = query.Zone ?? existing.Zone;
            existing.GitFit = query.GitFit ?? existing.GitFit;
            existing.Destination = query.Destination ?? existing.Destination;
            existing.PaxCount = query.PaxCount > 0 ? query.PaxCount : existing.PaxCount;
            existing.Budget = query.Budget > 0 ? query.Budget : existing.Budget;
            existing.Status = query.Status ?? existing.Status;
            existing.Source = query.Source ?? existing.Source;
            existing.QueryCode = query.QueryCode ?? existing.QueryCode;
            existing.Notes = query.Notes ?? existing.Notes;

            // Optional fields - only overwrite if valid
            if (query.QueryDate != DateTime.MinValue && query.QueryDate != default)
                existing.QueryDate = query.QueryDate;

            if (query.StartDate.HasValue)
                existing.StartDate = query.StartDate;

            if (query.EndDate.HasValue)
                existing.EndDate = query.EndDate;

            if (query.ConversionProbability.HasValue)
                existing.ConversionProbability = query.ConversionProbability;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error updating lead: " + ex.Message);
            }
        }


    }
}
