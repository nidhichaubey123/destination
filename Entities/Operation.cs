
using System.ComponentModel.DataAnnotations;

public class Operation
{
    [Key]
    public int OperationId { get; set; }

    [Required]
    public string OperationName { get; set; }

    public string? OperationDescription { get; set; }

    public DateTime OperationCreatedOn { get; set; }

    public bool OperationIsActive { get; set; }


}
