using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SwissMoteWebsite.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {


        public string ChatKey { get; set; }

        [Display(Name = "Full Name")]
        public string FullPaymentName { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address { get; set; }
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        public string IBAN { get; set; }

        [Display(Name = "PayPal Email")]
        public string PayPalEmail { get; set; }







        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<SwissMoteWebsite.Models.Team> Teams { get; set; }

        public System.Data.Entity.DbSet<SwissMoteWebsite.Models.Project> Projects { get; set; }
    }
}