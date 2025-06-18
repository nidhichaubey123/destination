using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DMCPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public UserRoleController(DMCPortalDBContext context)
        {
            _context = context;
        }

        // Assign Roles
        [HttpPost("Assign")]
        public IActionResult AssignRoles(int userId, [FromBody] List<int> roleIds)
        {
            var existing = _context.UserRoles.Where(ur => ur.UserId == userId).ToList();
            _context.UserRoles.RemoveRange(existing);

            foreach (var roleId in roleIds)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = userId,
                    RoleId = roleId,
                    IsActive = true,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                });
            }

            _context.SaveChanges();
            return Ok(new { message = "Roles assigned successfully." });
        }

        // Get Assigned Roles
        [HttpGet("User/{userId}")]
        public IActionResult GetRolesByUser(int userId)
        {
            var roles = _context.UserRoles
                .Where(ur => ur.UserId == userId && ur.IsActive)
                .Select(ur => new { ur.RoleId })
                .ToList();

            return Ok(roles);
        }

        [HttpGet("GetRoleNamesForUser/{userId}")]
        public IActionResult GetRoleNamesForUser(int userId)
        {
            var roleNames = _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_context.Roles,
                      ur => ur.RoleId,
                      r => r.RoleId,
                      (ur, r) => r.RoleName)
                .ToList();

            return Ok(roleNames);
        }



        // Optional: Get user with roles
        [HttpGet("WithRoles")]
        public IActionResult GetUsersWithRoles()
        {
            var data = _context.Users.Select(u => new
            {
                u.userId,
                u.emailAddress,
                Roles = _context.UserRoles
                    .Where(ur => ur.UserId == u.userId)
                    .Select(ur => ur.Role.RoleName)
                    .ToList()
            }).ToList();

            return Ok(data);
        }
        [HttpPost("AssignRoles")]
        public IActionResult AssignRoles([FromBody] RoleAssignmentDto dto)
        {
            if (dto == null || dto.RoleIds == null || dto.RoleIds.Count == 0)
                return BadRequest("Invalid role assignment request.");

            var user = _context.Users.Find(dto.UserId);
            if (user == null)
                return NotFound("User not found.");

            // Get existing active roles
            var existingRoles = _context.UserRoles
                .Where(ur => ur.UserId == dto.UserId && ur.IsActive)
                .ToList();

            var existingRoleIds = existingRoles.Select(r => r.RoleId).ToList();

            // ✅ Add only roles not already assigned
            var rolesToAdd = dto.RoleIds.Except(existingRoleIds);
            foreach (var roleId in rolesToAdd)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = dto.UserId,
                    RoleId = roleId,
                    IsActive = true,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                });
            }

            // ❌ Optional: remove roles that were unchecked
            var rolesToRemove = existingRoles.Where(r => !dto.RoleIds.Contains(r.RoleId)).ToList();
            _context.UserRoles.RemoveRange(rolesToRemove);

            _context.SaveChanges();
            return Ok(new { message = "Roles updated successfully." });
        }


        public class RoleAssignmentDto
        {
            public int UserId { get; set; }
            public List<int> RoleIds { get; set; }
        }


    }
}
