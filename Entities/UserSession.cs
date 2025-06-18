using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMCPortal.API.Entities
{
    public class UserSession
    {
        [Key]
        public Guid sessionId { get; set; }

        [Required]
        public int userId { get; set; }

        [Required]
        public string sessionIPAddress { get; set; }

        public string? sessionHostName { get; set; }

        [Required]
        public DateTime createdOn { get; set; }

        [Required]
        public bool isExpired { get; set; }

        public DateTime? expiredOn { get; set; }
    }
}
