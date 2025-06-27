using DMCPortal.API.Entities;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AgentController : ControllerBase
{
    private readonly DMCPortalDBContext _context;

    public AgentController(DMCPortalDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Agent>> GetAgents()
    {
        return _context.Agents.ToList();
    }
}
