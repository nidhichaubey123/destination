using System.ComponentModel.DataAnnotations;

namespace DMCPortal.API.Entities
{
    public class MeetingType
    {
        public int MeetingTypeId { get; set; }

        [Required]
        [MaxLength(255)]
        public string MeetingTypeName { get; set; }

        [MaxLength(500)]
        public string MeetingTypeDesc { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

    }

}
