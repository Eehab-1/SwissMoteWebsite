using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwissMoteWebsite.Models
{
    public class Team
    {


        [Key]
        public int TeamId { get; set; }

        public string TeamUniqueId { get; set; }
        public string TeamCreatedByUserId { get; set; }
        public string TeamName { get; set; }

        public string TeamMember { get; set; }

        public double TeamInsights { get; set; }

        public double MemberInsights { get; set; }



        public double MemberHourlyRate { get; set; }

        public bool TeamStatus { get; set; }

        public bool MemberStatus { get; set; }

        public string ClientName { get; set; }

        public string MemberChatKey { get; set; }

        public virtual ICollection<Project> projects { get; set; }






    }
}