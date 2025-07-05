
using Microsoft.EntityFrameworkCore;

namespace DMCPortal.API.Entities
{
    public class DMCPortalDBContext:DbContext
    {

        public DMCPortalDBContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<RoleOperation> RoleOperations { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<TruvaiQuery> TruvaiQueries { get; set; }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<DiscussionType> DiscussionTypes { get; set; }
        public DbSet<MeetingType> MeetingTypes { get; set; }
        public DbSet<SalesVisit> SalesVisits { get; set; }
        public DbSet<Zone> Zones { get; set; }

    }
}
