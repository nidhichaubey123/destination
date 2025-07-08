using System.ComponentModel.DataAnnotations;

namespace DMCPortal.API.Entities
{
    public class Agent
    {
        [Key]
        public int AgentId { get; set; }

        [Required]
    
        public string AgentName { get; set; }

        public string AgentPoc1 { get; set; }
        public string Agency_Company { get; set; }


        public string phoneno { get; set; }
        [Required]
        public string emailAddress { get; set; }
        public string Zone { get; set; }

        public string AgentAddress { get; set; }

    }

}
