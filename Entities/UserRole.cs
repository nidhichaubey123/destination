using DMCPortal.API.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserRole
{
    [Key]
    public int Id { get; set; }  

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Role")]
    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; } 
    public DateTime UpdatedOn { get; set; } 

    public Role Role { get; set; }
    public User User { get; set; }
}
