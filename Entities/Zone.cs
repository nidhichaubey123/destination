using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMCPortal.API.Entities
{
    [Table("Zones")]  // Force table name to Zones
    public class Zone
    {
        [Key]
        public int ZoneId { get; set; }

        [Required]
        public string ZoneName { get; set; }

        [Required]
        public string ZoneCreatedBy { get; set; }

        public DateTime ZoneCreatedOn { get; set; } = DateTime.Now;

        public string? ZoneUpdatedBy { get; set; }

        public DateTime? ZoneUpdatedOn { get; set; }
    }
}