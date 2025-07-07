using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DMCPortal.API.Entities;
using DMCPortal.API.Entities.Helpers;

namespace DMCPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesVisitController : ControllerBase
    {
        private readonly DMCPortalDBContext _context;

        public SalesVisitController(DMCPortalDBContext context)
        {
            _context = context;
        }

        [HttpGet]
public async Task<ActionResult<IEnumerable<SalesVisitViewModel>>> GetSalesVisits()
{
    var sales = await (from s in _context.SalesVisits
                       join u in _context.Users on s.CreatedBy equals u.userId
                       join m in _context.MeetingTypes on s.MeetingTypeId equals m.MeetingTypeId into mt
                       from m in mt.DefaultIfEmpty()
                       join d in _context.DiscussionTypes on s.DiscussionTypeId equals d.DiscussionTypeId into dt
                       from d in dt.DefaultIfEmpty()
                       join a in _context.Agents on s.AgentId equals a.AgentId into at
                       from a in at.DefaultIfEmpty()
                       orderby s.VisitDate descending
                       select new SalesVisitViewModel
                       {
                           SalesVisitId = s.SalesVisitId,
                           UserName = u.firstName + " " + u.lastName,
                           MeetingVenueName = s.MeetingVenueName,
                           MeetingNotes = s.MeetingNotes,
                           SalesVisitCode = s.SalesVisitCode,
                           VisitDate = s.VisitDate,
                           VisitTime = s.VisitTime.ToString(@"hh\:mm"),
                           MeetingTypeName = m != null ? m.MeetingTypeName : "",
                           DiscussionTypeName = d != null ? d.DiscussionTypeName : "",
                           AgentName = a != null ? a.AgentName : ""
                       }).ToListAsync();

    return Ok(sales);
}


        [HttpGet("{id}")]
        public async Task<ActionResult<SalesVisit>> GetSalesVisit(int id)
        {
            var visit = await _context.SalesVisits.FindAsync(id);
            if (visit == null)
                return NotFound();
            return visit;

        }
        [HttpPost]
        public async Task<ActionResult<SalesVisit>> PostSalesVisit([FromBody] SalesVisit visit)
        {
            // Remove claim check
            if (visit.CreatedBy <= 0)
                return BadRequest("CreatedBy is required.");

            visit.CreatedOn = DateTime.Now;

            _context.SalesVisits.Add(visit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalesVisit), new { id = visit.SalesVisitId }, visit);
        }


        [HttpPut("{id}")]
       public async Task<IActionResult> PutSalesVisit(int id, SalesVisit visit)
        {
              if (id != visit.SalesVisitId)
         return BadRequest();

        var existing = await _context.SalesVisits.FindAsync(id);
        if (existing == null)
        return NotFound();


            existing.MeetingVenueName = visit.MeetingVenueName;
            existing.MeetingNotes = visit.MeetingNotes;
         
            existing.AgentId = visit.AgentId;
            existing.MeetingTypeId = visit.MeetingTypeId;
            existing.DiscussionTypeId = visit.DiscussionTypeId;
            existing.VisitDate = visit.VisitDate;
            existing.VisitTime = visit.VisitTime;
            existing.UpdatedBy = visit.UpdatedBy;
            existing.UpdatedOn = DateTime.Now;

            // Preserve CreatedBy and CreatedOn
            visit.CreatedBy = existing.CreatedBy;
      visit.CreatedOn = existing.CreatedOn;
            visit.EntryLatitude = existing.EntryLatitude;
            visit.EntryLongitude = existing.EntryLongitude;
            // Set UpdatedBy and UpdatedOn
            visit.UpdatedBy = visit.UpdatedBy;  // Set from client-side or API (recommend setting in API)
      visit.UpdatedOn = DateTime.Now;

          _context.Entry(existing).CurrentValues.SetValues(visit);
         await _context.SaveChangesAsync();

          return NoContent();
            }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesVisit(int id)
        {
            var visit = await _context.SalesVisits.FindAsync(id);
            if (visit == null)
                return NotFound();

            _context.SalesVisits.Remove(visit);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("UserWise/{userId}")]
        public async Task<ActionResult<IEnumerable<SalesVisitViewModel>>> GetSalesByUser(int userId)
        {
            try
            {
                var sales = await (from s in _context.SalesVisits
                                   join u in _context.Users on s.CreatedBy equals u.userId
                                   join m in _context.MeetingTypes on s.MeetingTypeId equals m.MeetingTypeId into mt
                                   from m in mt.DefaultIfEmpty()
                                   join d in _context.DiscussionTypes on s.DiscussionTypeId equals d.DiscussionTypeId into dt
                                   from d in dt.DefaultIfEmpty()
                                   join a in _context.Agents on s.AgentId equals a.AgentId into at
                                   from a in at.DefaultIfEmpty()
                                   where s.CreatedBy == userId
                                   orderby s.VisitDate descending
                                   select new SalesVisitViewModel
                                   {
                                       SalesVisitId = s.SalesVisitId,
                                       UserName = u.firstName + " " + u.lastName,
                                       MeetingVenueName = s.MeetingVenueName,
                                       MeetingNotes = s.MeetingNotes,
                                       SalesVisitCode = s.SalesVisitCode,
                                       VisitDate = s.VisitDate,
                                       VisitTime = s.VisitTime.ToString(@"hh\:mm"),
                                       MeetingTypeName = m != null ? m.MeetingTypeName : "",
                                       DiscussionTypeName = d != null ? d.DiscussionTypeName : "",
                                       AgentName = a != null ? a.AgentName : ""
                                   }).ToListAsync();

                return Ok(sales);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR in GetSalesByUser: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Paged")]
        public async Task<ActionResult<PagedResult<SalesVisitViewModel>>> GetPaged(
       int page = 1, int pageSize = 10, string search = "", int? userId = null)
        {
            try
            {
                search = search?.ToLower() ?? "";

                var baseQuery = from s in _context.SalesVisits
                                join u in _context.Users on s.CreatedBy equals u.userId
                                join m in _context.MeetingTypes on s.MeetingTypeId equals m.MeetingTypeId into mt
                                from m in mt.DefaultIfEmpty()
                                join d in _context.DiscussionTypes on s.DiscussionTypeId equals d.DiscussionTypeId into dt
                                from d in dt.DefaultIfEmpty()
                                join a in _context.Agents on s.AgentId equals a.AgentId into at
                                from a in at.DefaultIfEmpty()
                                where string.IsNullOrEmpty(search)
                                      || s.MeetingVenueName.ToLower().Contains(search)
                                      || s.MeetingNotes.ToLower().Contains(search)
                                      || s.SalesVisitCode.ToLower().Contains(search)
                                      || (m != null && m.MeetingTypeName.ToLower().Contains(search))
                                      || (d != null && d.DiscussionTypeName.ToLower().Contains(search))
                                      || (a != null && a.AgentName.ToLower().Contains(search))
                                      || (u.firstName + " " + u.lastName).ToLower().Contains(search)
                                select new
                                {
                                    s.SalesVisitId,
                                    s.MeetingVenueName,
                                    s.MeetingNotes,
                                    s.SalesVisitCode,
                                    s.VisitDate,
                                    s.VisitTime,
                                    s.MeetingTypeId,
                                    s.DiscussionTypeId,
                                    s.AgentId,
                                    u.firstName,
                                    u.lastName,
                                    m.MeetingTypeName,
                                    d.DiscussionTypeName,
                                    a.AgentName,
                                    s.CreatedBy
                                };

                if (userId.HasValue)
                    baseQuery = baseQuery.Where(s => s.CreatedBy == userId.Value);

                var totalCount = await baseQuery.CountAsync();

                var items = await baseQuery
                    .OrderByDescending(s => s.VisitDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(s => new SalesVisitViewModel
                    {
                        SalesVisitId = s.SalesVisitId,
                        UserName = s.firstName + " " + s.lastName,
                        MeetingVenueName = s.MeetingVenueName,
                        MeetingNotes = s.MeetingNotes,
                        SalesVisitCode = s.SalesVisitCode,
                        VisitDate = s.VisitDate,
                        VisitTime = s.VisitTime.ToString(@"hh\:mm"),
                        MeetingTypeName = s.MeetingTypeName ?? "",
                        DiscussionTypeName = s.DiscussionTypeName ?? "",
                        AgentName = s.AgentName ?? ""
                    })
                    .ToListAsync();

                return Ok(new PagedResult<SalesVisitViewModel>
                {
                    Items = items,
                    TotalCount = totalCount
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetPaged: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


    }
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }

    public class PaginatedSalesVisitViewModel
    {
        public List<SalesVisitViewModel> SalesVisits { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}