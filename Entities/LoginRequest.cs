using System.ComponentModel.DataAnnotations;

namespace DMCPortal.API.Entities
{

    public class LoginRequest
    {
    
        public string EmailAddress { get; set; }


        public string Password { get; set; }
    }
}
