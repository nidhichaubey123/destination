namespace DMCPortal.API.Entities.Helpers
{
    public class UserUpdateDto { 
         public int UserId { get; set; }
    public string FirstName { get; set; } = "";
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string? MobileNo { get; set; }
   
    }
}
