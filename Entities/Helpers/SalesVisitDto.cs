namespace DMCPortal.API.Entities.Helpers
{
    namespace DMCPortal.API.Entities.Helpers
    {
        public class SalesVisitDto
        {
            public int SalesVisitId { get; set; }
            public string? UserName { get; set; }
            public DateTime VisitDate { get; set; }
            public TimeSpan VisitTime { get; set; }
            public string? MeetingVenueName { get; set; }
            public string? MeetingNotes { get; set; }
            public string? MeetingTypeName { get; set; }
            public string? DiscussionTypeName { get; set; }
            public string? AgentName { get; set; }
            public string? SalesVisitCode { get; set; }
        }

    }

}
