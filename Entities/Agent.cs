using System.ComponentModel.DataAnnotations;

namespace DMCPortal.API.Entities
{
    public class Agent
    {
        public int AgentId { get; set; }

        [Required]
        [MaxLength(255)]
        public string AgentName { get; set; }



        // Navigation
        public ICollection<SalesVisit> SalesVisits { get; set; }
    }

}
