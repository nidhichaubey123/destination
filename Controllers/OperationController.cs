using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DMCPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public OperationController(DMCPortalDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Operation>> GetOperations()
        {
            return Ok(_context.Operations.OrderBy(u=>u.OperationName).ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Operation> GetOperation(int id)
        {
            var op = _context.Operations.Find(id);
            if (op == null) return NotFound();
            return Ok(op);
        }

        [HttpPost]
        public IActionResult CreateOperation(Operation operation)
        {
            if (_context.Operations.Any(o => o.OperationName == operation.OperationName))
                return BadRequest("Operation Name already exists.");

            operation.OperationCreatedOn = DateTime.Now;
            operation.OperationCreatedBy = GetLoggedInUserId();
            _context.Operations.Add(operation);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOperation(int id, Operation operation)
        {
            if (_context.Operations.Any(o => o.OperationName == operation.OperationName && o.OperationId != id))
                return BadRequest("Operation Name already exists.");

            var existing = _context.Operations.Find(id);
            if (existing == null) return NotFound();

            existing.OperationName = operation.OperationName;
            existing.OperationDescription = operation.OperationDescription;
            existing.OperationIsActive = operation.OperationIsActive;
            existing.OperationUpdatedOn = DateTime.Now;

            existing.OperationUpdatedBy = GetLoggedInUserId();


            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOperation(int id)
        {
            var op = _context.Operations.Find(id);
            if (op == null) return NotFound();

            _context.Operations.Remove(op);
            _context.SaveChanges();
            return Ok();
        }

      
[HttpGet("api/Operation")]
public IActionResult GetAllOperations()
        {
            return Ok(_context.Operations.Where(o => o.OperationIsActive).ToList());
        }

        // GET assigned operations by RoleId
        [HttpGet("api/RoleOperation/Role/{roleId}")]
        public IActionResult GetOperationsByRole(int roleId)
        {
            var ops = _context.RoleOperations
                        .Where(ro => ro.RoleId == roleId && ro.IsActive)
                        .Select(ro => new { ro.OperationId })
                        .ToList();
            return Ok(ops);
        }

        // POST: assign operations to a role
        [HttpPost("api/RoleOperation/AssignOperations")]
        public IActionResult AssignOperations([FromBody] AssignOperationRequest req)
        {
            // Delete old ones
            var existing = _context.RoleOperations.Where(r => r.RoleId == req.RoleId).ToList();
            _context.RoleOperations.RemoveRange(existing);

            // Add new ones
            var newOps = req.OperationIds.Select(opId => new RoleOperation
            {
                RoleId = req.RoleId,
                OperationId = opId,
                IsActive = true,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now
            }).ToList();

            _context.RoleOperations.AddRange(newOps);
            _context.SaveChanges();

            return Ok(new { message = "Operations assigned successfully." });
        }

        public class AssignOperationRequest
        {
            public int RoleId { get; set; }
            public List<int> OperationIds { get; set; }
        }
        [HttpGet("Role/{roleId}")]
        public IActionResult GetOperationsByRoleId(int roleId)
        {
            var operationIds = _context.RoleOperations
                .Where(ro => ro.RoleId == roleId && ro.IsActive)
                .Select(ro => ro.OperationId)
                .ToList();

            return Ok(operationIds);
        }

        private int GetLoggedInUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }

    }
}
