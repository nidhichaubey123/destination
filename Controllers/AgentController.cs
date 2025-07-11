using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAll()
        {
            return await _context.Agents.ToListAsync();
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var agent = await _context.Agents.FindAsync(id);
            if (agent == null) return NotFound();

            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        // ✅ INSERT from AppSheet
        [HttpPost("appsheet-insert")]
        public IActionResult InsertFromAppSheet([FromBody] Agent agent)
        {
            try
            {
                if (agent == null || string.IsNullOrEmpty(agent.AppSheetId))
                    return BadRequest("Agent or AppSheetId is null");

                var exists = _context.Agents.Any(a =>
                    a.AgentName.ToLower() == agent.AgentName.ToLower() ||
                    a.emailAddress.ToLower() == agent.emailAddress.ToLower() ||
                    a.AppSheetId == agent.AppSheetId);

                if (exists)
                    return Conflict(new { message = "Agent already exists" });

                _context.Agents.Add(agent);
                _context.SaveChanges();

                System.IO.File.AppendAllText("C:\\Temp\\agent_log.txt", $"INSERT: {JsonSerializer.Serialize(agent)}\n");

                return Ok(new
                {
                    message = "Inserted via AppSheet",
                    AgentId = agent.AgentId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPost("update-from-appsheet")]
        public async Task<IActionResult> UpdateFromAppSheet([FromBody] Agent updated)
        {
            if (string.IsNullOrEmpty(updated.AppSheetId))
                return BadRequest("AppSheetId is required.");

            var existing = await _context.Agents
                .FirstOrDefaultAsync(a => a.AppSheetId == updated.AppSheetId && a.IsDeleted != true);

            if (existing == null)
                return NotFound("Agent not found.");

            existing.AgentName = updated.AgentName;
            existing.AgentPoc1 = updated.AgentPoc1;
            existing.Agency_Company = updated.Agency_Company;
            existing.phoneno = updated.phoneno;
            existing.emailAddress = updated.emailAddress;
            existing.Zone = updated.Zone;
            existing.AgentAddress = updated.AgentAddress;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Updated successfully." });
        }
        [HttpPost("delete-from-appsheet")]
        public async Task<IActionResult> DeleteFromAppSheet([FromBody] Agent input)
        {
            if (string.IsNullOrEmpty(input.AppSheetId))
                return BadRequest("AppSheetId is required.");

            var agent = await _context.Agents
                .FirstOrDefaultAsync(a => a.AppSheetId == input.AppSheetId);

            if (agent == null)
                return NotFound("Agent not found.");

            _context.Agents.Remove(agent);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Deleted successfully." });
        }
    }
}
