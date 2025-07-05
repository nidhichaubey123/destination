using Microsoft.AspNetCore.Mvc;
using DMCPortal.API.Entities;

namespace DMCPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public ZoneController(DMCPortalDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetZones()
        {
            return Ok(_context.Zones.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetZone(int id)
        {
            var zone = _context.Zones.Find(id);
            return zone == null ? NotFound() : Ok(zone);
        }

        [HttpPost]
        public IActionResult CreateZone([FromBody] Zone zone)
        {
            try
            {
                zone.ZoneCreatedOn = DateTime.Now;
                _context.Zones.Add(zone);
                _context.SaveChanges();
                return Ok(zone);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error creating zone", error = ex.Message });
            }
        }

        // KEEP ONLY THIS PUT METHOD - Remove any duplicate
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ZoneUpdateModel updated)
        {
            try
            {
                var zone = await _context.Zones.FindAsync(id);
                if (zone == null) return NotFound();

                // Update only the fields that should be changed
                zone.ZoneName = updated.ZoneName;
                zone.ZoneUpdatedBy = updated.ZoneUpdatedBy;
                zone.ZoneUpdatedOn = DateTime.Now;

                await _context.SaveChangesAsync();
                return Ok(zone);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Error updating zone", error = ex.Message });
            }
        }

        // Move the model outside the method or to a separate file
        public class ZoneUpdateModel
        {
            public string ZoneName { get; set; } = string.Empty;
            public string ZoneUpdatedBy { get; set; } = string.Empty;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteZone(int id)
        {
            var zone = _context.Zones.Find(id);
            if (zone == null) return NotFound();

            _context.Zones.Remove(zone);
            _context.SaveChanges();
            return Ok();
        }
    }

    // Move the model outside the controller class
    public class ZoneUpdateModel
    {
        public string ZoneName { get; set; } = string.Empty;
        public string ZoneUpdatedBy { get; set; } = string.Empty;
    }
}