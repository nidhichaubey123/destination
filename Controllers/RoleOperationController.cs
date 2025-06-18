using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DMCPortal.API.Entities;

namespace DMCPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleOperationController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public RoleOperationController(DMCPortalDBContext context)
        {
            _context = context;
        }

        // ✅ GET all operations with assigned status for a role
        [HttpGet("GetAllWithAssigned/{roleId}")]
        public IActionResult GetAllWithAssigned(int roleId)
        {
            var allOps = _context.Operations
                .Select(o => new
                {
                    o.OperationId,
                    o.OperationName
                })
                .ToList();

            var assignedIds = _context.RoleOperations
                .Where(ro => ro.RoleId == roleId && ro.IsActive)
                .Select(ro => ro.OperationId)
                .ToHashSet();

            var result = allOps.Select(o => new
            {
                o.OperationId,
                o.OperationName,
                assigned = assignedIds.Contains(o.OperationId)
            });

            return Ok(result);
        }

        // ✅ POST: Assign new operations and update existing ones
        [HttpPost("AssignOperations")]
        public IActionResult AssignOperations([FromBody] AssignOperationsDto dto)
        {
            if (dto == null || dto.RoleId <= 0 || dto.OperationIds == null)
                return BadRequest("Invalid input.");

            if (!_context.Roles.Any(r => r.RoleId == dto.RoleId))
                return NotFound($"Role {dto.RoleId} not found.");

            var existingMappings = _context.RoleOperations
                .Where(ro => ro.RoleId == dto.RoleId)
                .ToList();

            var selectedIds = dto.OperationIds.Distinct().ToHashSet();

            // ✅ 1. Deactivate ones that are unchecked
            foreach (var mapping in existingMappings)
            {
                if (!selectedIds.Contains(mapping.OperationId) && mapping.IsActive)
                {
                    mapping.IsActive = false;
                    mapping.UpdatedOn = DateTime.Now;
                }
            }

            // ✅ 2. Reactivate previously deactivated ones that are now selected
            foreach (var mapping in existingMappings)
            {
                if (selectedIds.Contains(mapping.OperationId) && !mapping.IsActive)
                {
                    mapping.IsActive = true;
                    mapping.UpdatedOn = DateTime.Now;
                }
            }

            // ✅ 3. Add new ones
            var existingActiveIds = existingMappings
                .Where(x => x.IsActive)
                .Select(x => x.OperationId)
                .ToHashSet();

            var newIdsToAdd = selectedIds.Except(existingActiveIds);

            foreach (var opId in newIdsToAdd)
            {
                _context.RoleOperations.Add(new RoleOperation
                {
                    RoleId = dto.RoleId,
                    OperationId = opId,
                    IsActive = true,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                });
            }

            _context.SaveChanges();

            return Ok(new { message = "Operations assigned successfully." });
        }

        public class AssignOperationsDto
        {
            public int RoleId { get; set; }
            public List<int> OperationIds { get; set; }
        }
    }
}
