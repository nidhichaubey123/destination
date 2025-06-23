using System.ComponentModel.DataAnnotations;

namespace DMCPortal.API.Entities
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        public string emailAddress { get; set; }

        public string password { get; set; }

        public string firstName { get; set; }

        public string? middleName { get; set; }

        public string lastName { get; set; }

        public string? mobileNo { get; set; }

        public DateTime createdOn { get; set; }

        public DateTime? lastLoggedOn { get; set; }

 

        public bool UserIsActive { get; set; } = false;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
