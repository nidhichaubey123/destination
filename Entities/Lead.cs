namespace DMCPortal.API.Entities
{
    public class Lead
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? QueryDate { get; set; }
        public string? Zone { get; set; }
        public string? Destination { get; set; }
        public int? PaxCount { get; set; }
        public decimal? Budget { get; set; }
        public string? GitFit { get; set; }
        public string? Status { get; set; }
        public string? Source { get; set; }
        public string? QueryCode { get; set; }
    }

}
