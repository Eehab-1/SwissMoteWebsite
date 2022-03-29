using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwissMoteWebsite.Models
{
    public class Project
    {

        [Key]
        public int Id { get; set; }

        [Required]

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Notes (Optional)")]
        public string Note { get; set; }




        public string CreatedByUserName { get; set; }

        public string CreatedByUserId { get; set; }

        [Display(Name = "Created On")]
        public DateTime? CreationDate { get; set; }




        [DataType(DataType.EmailAddress)]
        [Display(Name = "Invited Email")]
        public string Invited_Email_UserName { get; set; }

        public string Invited_UserId { get; set; }

        [Display(Name = "Invitation Sent Date")]
        public DateTime? InvitationSentDate { get; set; }

        [Display(Name = "Invitation Accepted Date")]
        public DateTime? InvitationAcceptedDate { get; set; }
        [Display(Name = "Invited Role ")]
        public InvitedRole invitedrole { get; set; }
        [Display(Name = "Invitation Status")]
        public InvitationStatus invitationstatus { get; set; }

        public string UniqueId { get; set; }

        public bool IsOn { get; set; }

        public Project()
        {
            IsOn = true;
        }

        public int TeamId { get; set; }

       
        public virtual Team Team { get; set; }






    }


    public enum InvitedRole
    {
        Worker,
        Supervisor
    }

    public enum InvitationStatus
    {
        NonSent = 0,
        Sent = 1,
        Accepted = 2

    }





}