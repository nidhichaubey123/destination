using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMCPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscussionTypeController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public DiscussionTypeController(DMCPortalDBContext context) => _context = context;

        // GET api/DiscussionType
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var discussionTypes = await _context.DiscussionTypes
                    .OrderBy(d => d.DiscussionTypeName)
                    .ToListAsync();
                return Ok(discussionTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving discussion types", error = ex.Message });
            }
        }

        // GET api/DiscussionType/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid ID provided" });

                var discussionType = await _context.DiscussionTypes.FindAsync(id);
                if (discussionType == null)
                    return NotFound(new { message = $"Discussion type with ID {id} not found" });

                return Ok(discussionType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving discussion type", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DiscussionType model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrWhiteSpace(model.DiscussionTypeName))
                    return BadRequest(new { message = "Name required" });

                model.CreatedOn = DateTime.UtcNow; // API sets CreatedOn
                model.IsActive = model.IsActive;

                _context.DiscussionTypes.Add(model);
                await _context.SaveChangesAsync();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating", error = ex.Message });
            }
        }

        // PUT api/DiscussionType/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] DiscussionType model)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid ID provided" });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (string.IsNullOrWhiteSpace(model.DiscussionTypeName))
                    return BadRequest(new { message = "Discussion type name is required" });

                var existing = await _context.DiscussionTypes.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Discussion type with ID {id} not found" });

                // Check for duplicate names (excluding current record)
                var duplicateType = await _context.DiscussionTypes
                    .FirstOrDefaultAsync(d => d.DiscussionTypeName.ToLower() == model.DiscussionTypeName.ToLower()
                                            && d.DiscussionTypeId != id);

                if (duplicateType != null)
                    return Conflict(new { message = "Discussion type with this name already exists" });

                // Update properties
                existing.DiscussionTypeName = model.DiscussionTypeName.Trim();
                existing.DiscussionTypeDesc = model.DiscussionTypeDesc?.Trim();
                existing.IsActive = model.IsActive;

                await _context.SaveChangesAsync();
                return Ok(existing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating discussion type", error = ex.Message });
            }
        }

        // DELETE api/DiscussionType/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { message = "Invalid ID provided" });

                var existing = await _context.DiscussionTypes.FindAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Discussion type with ID {id} not found" });

                // Check if the discussion type is being used elsewhere (optional)
                // You might want to add this check based on your business logic
                // var isInUse = await _context.SomeOtherTable.AnyAsync(x => x.DiscussionTypeId == id);
                // if (isInUse)
                //     return BadRequest(new { message = "Cannot delete discussion type as it is being used" });

                _context.DiscussionTypes.Remove(existing);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Discussion type deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting discussion type", error = ex.Message });
            }
        }
    }
}