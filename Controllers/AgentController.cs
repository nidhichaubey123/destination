using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMCPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public AgentController(DMCPortalDBContext context)
        {
            _context = context;
        }

        // ✅ GET: api/Agent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAll()
        {
            return await _context.Agents.ToListAsync();
        }

        // ✅ GET: api/Agent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agent>> GetById(int id)
        {
            var agent = await _context.Agents.FindAsync(id);
            if (agent == null) return NotFound();
            return agent;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Agent agent)
        {
            try
            {
                _context.Agents.Add(agent);
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Agent agent)
        {
            try
            {
                if (id != agent.AgentId)
                    return BadRequest("Agent ID mismatch.");

                var exists = await _context.Agents.AnyAsync(a => a.AgentId == id);
                if (!exists) return NotFound();

                _context.Agents.Update(agent);
                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // ✅ DELETE: api/Agent/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var agent = await _context.Agents.FindAsync(id);
            if (agent == null) return NotFound();

            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }
    }
}
