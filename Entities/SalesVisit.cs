using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMCPortal.API.Entities
{
    public class SalesVisit
    {
        public int SalesVisitId { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }


        [Required]
        public DateTime VisitDate { get; set; }

        [Required]
        public TimeSpan VisitTime { get; set; }

        [ForeignKey("Agent")]
        public int? AgentId { get; set; }
        [ForeignKey("DiscussionType")]
        public int? DiscussionTypeId { get; set; }

        [ForeignKey("MeetingType")]
        public int? MeetingTypeId { get; set; }

        [MaxLength(255)]
        public string MeetingVenueName { get; set; }

        public decimal? MeetingLatitude { get; set; }
        public decimal? MeetingLongitude { get; set; }


        public User? User { get; set; }
        public Agent? Agent { get; set; }
        public MeetingType? MeetingType { get; set; }
        public DiscussionType? DiscussionType { get; set; }


        public decimal? EntryLatitude { get; set; }
        public decimal? EntryLongitude { get; set; }
        public string MeetingNotes { get; set; }

        [MaxLength(100)]
        public string SalesVisitCode { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }


        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } 
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }






    }

}
