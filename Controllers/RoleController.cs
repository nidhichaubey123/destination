using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DMCPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public RoleController(DMCPortalDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var roles = _context.Roles.OrderBy(u=>u.RoleName).ToList();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }

        [HttpPost]
        public IActionResult Post(Role role)
        {
            if (_context.Roles.Any(r => r.RoleName.ToLower() == role.RoleName.ToLower()))
            {
                return BadRequest("Role Name already exists.");
            }

            role.RoleCreatedOn = DateTime.Now;
            role.RoleCreatedBy = GetLoggedInUserId();

            _context.Roles.Add(role);
            _context.SaveChanges();
            return Ok(role);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Role role)
        {
            var existing = _context.Roles.Find(id);
            if (existing == null)
                return NotFound();

            if (_context.Roles.Any(r => r.RoleName.ToLower() == role.RoleName.ToLower() && r.RoleId != id))
            {
                return BadRequest("Role Name already exists.");
            }

            existing.RoleName = role.RoleName;
            existing.RoleDescription = role.RoleDescription;
            existing.RoleCreatedOn = DateTime.Now; existing.RoleName = role.RoleName;
       
            existing.RoleUpdatedOn = DateTime.Now;
            existing.RoleUpdatedBy = GetLoggedInUserId();


            _context.SaveChanges();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
                return NotFound();

            _context.Roles.Remove(role);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("AssignOperations")]
        public IActionResult AssignOperations([FromBody] RoleOperationRequest request)
        {
            var existing = _context.RoleOperations
                .Where(ro => ro.RoleId == request.RoleId)
                .ToList();

            // Mark all existing inactive
            foreach (var ro in existing)
                ro.IsActive = false;

            // Add or update selected operations
            foreach (var opId in request.OperationIds)
            {
                var existingOp = existing.FirstOrDefault(ro => ro.OperationId == opId);
                if (existingOp != null)
                {
                    existingOp.IsActive = true;
                }
                else
                {
                    _context.RoleOperations.Add(new RoleOperation
                    {
                        RoleId = request.RoleId,
                        OperationId = opId,
                        IsActive = true
                    });
                }
            }

            _context.SaveChanges();
            return Ok("Operations assigned.");
        }
        private int GetLoggedInUserId()
        {
            if (Request.Headers.TryGetValue("UserId", out var userIdHeader))
            {
                return int.TryParse(userIdHeader, out int userId) ? userId : 0;
            }
            return 0;
        }

    }

    public class RoleOperationRequest
    {
        public int RoleId { get; set; }
        public List<int> OperationIds { get; set; }
    }


}

