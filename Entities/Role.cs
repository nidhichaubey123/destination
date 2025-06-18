using System.ComponentModel.DataAnnotations;

public class Role
{
    [Key]
    public int RoleId { get; set; }

    [Required]
    public string RoleName { get; set; }

    public string? RoleDescription { get; set; }

    public DateTime RoleCreatedOn { get; set; }
}
