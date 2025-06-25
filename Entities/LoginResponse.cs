namespace DMCPortal.API.Entities.Helpers
{
    public class LoginResponse
    {
        public Guid SessionId { get; set; }
        public List<string> Operations { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
