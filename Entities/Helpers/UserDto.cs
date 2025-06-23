namespace DMCPortal.API.Entities.Helpers
{
    public class UserDto
    {
        public int userId { get; set; }
        public string emailAddress { get; set; }
        public string firstName { get; set; }

        public string middleName { get; set; }
        public string lastName { get; set; }
        public string? mobileNo { get; set; }
        public DateTime? lastLoggedOn { get; set; }
        public bool UserIsActive { get; set; }

        public List<string> Roles { get; set; } = new();
    }

}
