namespace DMCPortal.API.Entities
{
    public class RegisterRequest
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? MobileNo { get; set; }
    }

}
