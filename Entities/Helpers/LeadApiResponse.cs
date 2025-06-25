namespace DMCPortal.API.Entities.Helpers
{
    public class LeadApiResponse
    {
        public int TotalRecords { get; set; }
        public List<TruvaiQuery> Leads { get; set; }
    }
}
