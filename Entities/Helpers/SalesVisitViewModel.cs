namespace DMCPortal.API.Models.ViewModels
{
    public class SalesVisitViewModel
    {
        public int SalesVisitId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string AgentName { get; set; } = string.Empty;
        public string MeetingTypeName { get; set; } = string.Empty;
        public string DiscussionTypeName { get; set; } = string.Empty;
        public string VisitDate { get; set; } = string.Empty;
        public string VisitTime { get; set; } = string.Empty;
        public string MeetingVenueName { get; set; } = string.Empty;
        public string MeetingNotes { get; set; } = string.Empty;
        public string SalesVisitCode { get; set; } = string.Empty;
    }
}
