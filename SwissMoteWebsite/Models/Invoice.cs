using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwissMoteWebsite.Models
{
    public class Invoice
    {


        public int Id { get; set; }

        [Display(Name = "Project")]
        public string ProjectName { get; set; }

        [Display(Name = "Invoice From")]
        public string InvoiceFromUser { get; set; }

        public string FromUserId { get; set; }

        [Display(Name = "Invoice To")]
        public string InvoiceToUser { get; set; }

        public string ToUserId { get; set; }

        [Display(Name = "Total Hours")]
        public double TotalHours { get; set; }

        [Required]
        [Display(Name = "Hourly Rate (USD)")]
        public double HourlyRate { get; set; }

        [Display(Name = "Total Payment (USD)")]
        public double TotalPayment { get; set; }

        [Required]
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Required]
        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime? CreationDate { get; set; }

        public bool IsPaid { get; set; }

        public string ProjectSharedUniqueId { get; set; }

        public Invoice()
        {
            IsPaid = false;
            CreationDate = DateTime.UtcNow;
        }












    }
}