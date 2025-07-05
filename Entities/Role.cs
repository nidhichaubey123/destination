using System.ComponentModel.DataAnnotations;

public class Role
{
    [Key]
    public int RoleId { get; set; }

    [Required]
    public string RoleName { get; set; }

    public string? RoleDescription { get; set; }

    public DateTime RoleCreatedOn { get; set; }
    public int? RoleCreatedBy { get; set; }

    public DateTime? RoleUpdatedOn { get; set; }
    public int? RoleUpdatedBy { get; set; }

}
