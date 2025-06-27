using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DMCPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingTypeController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public MeetingTypeController(DMCPortalDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.MeetingTypes.OrderBy(x => x.MeetingTypeName).ToListAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _context.MeetingTypes.FindAsync(id);
            return entity == null ? NotFound(new { message = "Not found" }) : Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MeetingTypeDto model)
        {
            if (string.IsNullOrWhiteSpace(model.MeetingTypeName))
                return BadRequest(new { success = false, message = "Meeting type name is required." });

            bool exists = await _context.MeetingTypes
                .AnyAsync(x => x.MeetingTypeName.ToLower() == model.MeetingTypeName.Trim().ToLower());

            if (exists)
                return BadRequest("Meeting Name already exists.");

            var entity = new MeetingType
            {
                MeetingTypeName = model.MeetingTypeName.Trim(),
                MeetingTypeDesc = model.MeetingTypeDesc?.Trim(),
                IsActive = model.IsActive,
                CreatedOn = DateTime.UtcNow
            };

            _context.MeetingTypes.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Meeting type created successfully.", data = entity });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] MeetingTypeDto model)
        {
            var entity = await _context.MeetingTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { success = false, message = "Meeting type not found." });

            if (string.IsNullOrWhiteSpace(model.MeetingTypeName))
                return BadRequest(new { success = false, message = "Meeting type name is required." });

            bool exists = await _context.MeetingTypes
                .AnyAsync(x => x.MeetingTypeName.ToLower() == model.MeetingTypeName.Trim().ToLower() && x.MeetingTypeId != id);

            if (exists)
                return BadRequest(new { success = false, message = $"Meeting type '{model.MeetingTypeName}' already exists." });

            entity.MeetingTypeName = model.MeetingTypeName.Trim();
            entity.MeetingTypeDesc = model.MeetingTypeDesc?.Trim();
            entity.IsActive = model.IsActive;
            entity.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Meeting type updated successfully.", data = entity });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.MeetingTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Not found" });

            _context.MeetingTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Deleted successfully" });
        }
    }
}
