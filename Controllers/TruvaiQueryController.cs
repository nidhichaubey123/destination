using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace DMCPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgentController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;
        private readonly ILogger<AgentController> _logger;

        public AgentController(DMCPortalDBContext context, ILogger<AgentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agent>>> GetAll()
        {
            return await _context.Agents
                .Where(a => a.IsDeleted != true)
                .ToListAsync();
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
                if (string.IsNullOrEmpty(agent.AppSheetId))
                    agent.AppSheetId = Guid.NewGuid().ToString();

                _context.Agents.Add(agent);
                await _context.SaveChangesAsync();

                return Ok(new AgentResponse
                {
                    Success = true,
                    Message = "Agent inserted successfully",
                    AgentId = agent.AgentId,
                    AppSheetId = agent.AppSheetId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating agent");
                return StatusCode(500, new BasicResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Agent agent)
        {
            try
            {
                if (id != agent.AgentId) return BadRequest("Agent ID mismatch.");

                var exists = await _context.Agents.AnyAsync(a => a.AgentId == id);
                if (!exists) return NotFound();

                _context.Agents.Update(agent);
                await _context.SaveChangesAsync();

                return Ok(new BasicResponse { Success = true, Message = "Agent updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating agent with ID {AgentId}", id);
                return StatusCode(500, new BasicResponse { Success = false, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var agent = await _context.Agents.FindAsync(id);
            if (agent == null) return NotFound();

            agent.IsDeleted = true;
            agent.DeletedOn = DateTime.Now;
            agent.DeletedBy = "admin";
            await _context.SaveChangesAsync();

            return Ok(new BasicResponse { Success = true, Message = "Agent deleted successfully" });
        }

        [HttpPost("appsheet-insert")]
        public async Task<IActionResult> InsertFromAppSheet([FromBody] Agent agent)
        {
            try
            {
                _logger.LogInformation("AppSheet Insert - Received agent: {AgentName}, AppSheetId: {AppSheetId}",
                    agent?.AgentName, agent?.AppSheetId);

                if (agent == null)
                    return BadRequest(new BasicResponse { Success = false, Message = "Agent is null" });

                if (string.IsNullOrEmpty(agent.AppSheetId))
                    return BadRequest(new BasicResponse { Success = false, Message = "AppSheetId is required" });

                var exists = await _context.Agents.AnyAsync(a =>
                    a.AppSheetId == agent.AppSheetId ||
                    (a.AgentName.ToLower() == agent.AgentName.ToLower() && a.IsDeleted != true) ||
                    (a.emailAddress.ToLower() == agent.emailAddress.ToLower() && a.IsDeleted != true));

                if (exists)
                    return Conflict(new BasicResponse { Success = false, Message = "Agent already exists" });

                _context.Agents.Add(agent);
                await _context.SaveChangesAsync();

                return Ok(new AgentResponse
                {
                    Success = true,
                    Message = "Agent inserted via AppSheet",
                    AgentId = agent.AgentId,
                    AppSheetId = agent.AppSheetId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting agent from AppSheet");
                return StatusCode(500, new BasicResponse { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("update-from-appsheet")]
        public async Task<IActionResult> UpdateFromAppSheet([FromBody] AppSheetUpdateRequest request)
        {
            try
            {
                _logger.LogInformation("AppSheet Update - Received request for AppSheetId: {AppSheetId}",
                    request?.AppSheetId);

                // Log the raw request body for debugging
                var rawBody = await ReadRawRequestBody();
                _logger.LogInformation("Raw request body: {RequestBody}", rawBody);

                if (request == null)
                    return BadRequest(new BasicResponse { Success = false, Message = "Request is null" });

                if (string.IsNullOrEmpty(request.AppSheetId))
                    return BadRequest(new BasicResponse { Success = false, Message = "AppSheetId is required" });

                var existing = await _context.Agents
                    .FirstOrDefaultAsync(a => a.AppSheetId == request.AppSheetId && a.IsDeleted != true);

                if (existing == null)
                {
                    _logger.LogWarning("Agent not found for AppSheetId: {AppSheetId}", request.AppSheetId);
                    return NotFound(new BasicResponse { Success = false, Message = "Agent not found" });
                }

                // Update only non-null fields
                if (!string.IsNullOrEmpty(request.AgentName))
                    existing.AgentName = request.AgentName;
                if (!string.IsNullOrEmpty(request.AgentPoc1))
                    existing.AgentPoc1 = request.AgentPoc1;
                if (!string.IsNullOrEmpty(request.Agency_Company))
                    existing.Agency_Company = request.Agency_Company;
                if (!string.IsNullOrEmpty(request.phoneno))
                    existing.phoneno = request.phoneno;
                if (!string.IsNullOrEmpty(request.emailAddress))
                    existing.emailAddress = request.emailAddress;
                if (!string.IsNullOrEmpty(request.Zone))
                    existing.Zone = request.Zone;
                if (!string.IsNullOrEmpty(request.AgentAddress))
                    existing.AgentAddress = request.AgentAddress;


                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated agent with AppSheetId: {AppSheetId}", request.AppSheetId);
                return Ok(new BasicResponse { Success = true, Message = "Agent updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating agent from AppSheet");
                return StatusCode(500, new BasicResponse { Success = false, Message = ex.Message });
            }
        }

        [HttpPost("delete-from-appsheet")]
        public async Task<IActionResult> DeleteFromAppSheet([FromBody] AppSheetDeleteRequest request)
        {
            try
            {
                _logger.LogInformation("AppSheet Delete - Received request for AppSheetId: {AppSheetId}",
                    request?.AppSheetId);

                // Log the raw request body for debugging
                var rawBody = await ReadRawRequestBody();
                _logger.LogInformation("Raw request body: {RequestBody}", rawBody);

                if (request == null)
                    return BadRequest(new BasicResponse { Success = false, Message = "Request is null" });

                if (string.IsNullOrEmpty(request.AppSheetId))
                    return BadRequest(new BasicResponse { Success = false, Message = "AppSheetId is required" });

                var agent = await _context.Agents
                    .FirstOrDefaultAsync(a => a.AppSheetId == request.AppSheetId);

                if (agent == null)
                {
                    _logger.LogWarning("Agent not found for deletion with AppSheetId: {AppSheetId}", request.AppSheetId);
                    return NotFound(new BasicResponse { Success = false, Message = "Agent not found" });
                }

                agent.IsDeleted = true;
                agent.DeletedOn = DateTime.Now;
                agent.DeletedBy = "AppSheetBot";

                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted agent with AppSheetId: {AppSheetId}", request.AppSheetId);
                return Ok(new BasicResponse { Success = true, Message = "Agent deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting agent from AppSheet");
                return StatusCode(500, new BasicResponse { Success = false, Message = ex.Message });
            }
        }

        // Helper method to read raw request body for debugging
        private async Task<string> ReadRawRequestBody()
        {
            try
            {
                Request.EnableBuffering();
                Request.Body.Position = 0;
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                Request.Body.Position = 0;
                return body;
            }
            catch
            {
                return "Unable to read request body";
            }
        }

        // Alternative endpoints that accept different data formats
        [HttpPost("update-from-appsheet-alt")]
        public async Task<IActionResult> UpdateFromAppSheetAlternative([FromBody] JsonElement jsonElement)
        {
            try
            {
                _logger.LogInformation("AppSheet Update Alt - Raw JSON: {Json}", jsonElement.ToString());

                // Try to extract AppSheetId from various possible field names
                string appSheetId = null;
                if (jsonElement.TryGetProperty("AppSheetId", out var appSheetIdProp))
                    appSheetId = appSheetIdProp.GetString();
                else if (jsonElement.TryGetProperty("appSheetId", out var appSheetIdProp2))
                    appSheetId = appSheetIdProp2.GetString();
                else if (jsonElement.TryGetProperty("id", out var idProp))
                    appSheetId = idProp.GetString();

                if (string.IsNullOrEmpty(appSheetId))
                    return BadRequest(new BasicResponse { Success = false, Message = "AppSheetId not found in request" });

                var existing = await _context.Agents
                    .FirstOrDefaultAsync(a => a.AppSheetId == appSheetId && a.IsDeleted != true);

                if (existing == null)
                    return NotFound(new BasicResponse { Success = false, Message = "Agent not found" });

                // Update fields if they exist in the JSON
                if (jsonElement.TryGetProperty("AgentName", out var agentNameProp))
                    existing.AgentName = agentNameProp.GetString();
                if (jsonElement.TryGetProperty("AgentPoc1", out var agentPoc1Prop))
                    existing.AgentPoc1 = agentPoc1Prop.GetString();
                if (jsonElement.TryGetProperty("Agency_Company", out var agencyCompanyProp))
                    existing.Agency_Company = agencyCompanyProp.GetString();
                if (jsonElement.TryGetProperty("phoneno", out var phonenoProp))
                    existing.phoneno = phonenoProp.GetString();
                if (jsonElement.TryGetProperty("emailAddress", out var emailAddressProp))
                    existing.emailAddress = emailAddressProp.GetString();
                if (jsonElement.TryGetProperty("Zone", out var zoneProp))
                    existing.Zone = zoneProp.GetString();
                if (jsonElement.TryGetProperty("AgentAddress", out var agentAddressProp))
                    existing.AgentAddress = agentAddressProp.GetString();

           

                await _context.SaveChangesAsync();

                return Ok(new BasicResponse { Success = true, Message = "Agent updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating agent from AppSheet (alternative method)");
                return StatusCode(500, new BasicResponse { Success = false, Message = ex.Message });
            }
        }
    }

    // Request models for AppSheet operations
    public class AppSheetUpdateRequest
    {
        public string AppSheetId { get; set; }
        public string AgentName { get; set; }
        public string AgentPoc1 { get; set; }
        public string Agency_Company { get; set; }
        public string phoneno { get; set; }
        public string emailAddress { get; set; }
        public string Zone { get; set; }
        public string AgentAddress { get; set; }
    }

    public class AppSheetDeleteRequest
    {
        public string AppSheetId { get; set; }
    }

    public class AgentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int AgentId { get; set; }
        public string AppSheetId { get; set; }
    }

    public class BasicResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}