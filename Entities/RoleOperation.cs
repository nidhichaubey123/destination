using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class RoleOperation
{
    [Key]
    public int Id { get; set; }  

    [ForeignKey("Role")]
    public int RoleId { get; set; }

    [ForeignKey("Operation")]
    public int OperationId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; } 
    public DateTime UpdatedOn { get; set; } 

    public Role Role { get; set; }
    public Operation Operation { get; set; }
}
