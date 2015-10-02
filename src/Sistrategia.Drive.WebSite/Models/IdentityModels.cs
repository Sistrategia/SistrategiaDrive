//using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;

namespace Sistrategia.Drive.WebSite.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IUser // : IdentityUser
    {
        private string id;
        private string userName;

        public string Id {
            get { return this.id; }
            set { this.id = value; } // This must be removed, IUser doesn't include the setter.
        }

        public string UserName {
            get { return this.userName; }
            set { this.userName = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        //{
        //    public ApplicationDbContext()
        //        : base("DefaultConnection", throwIfV1Schema: false) {
        //    }

        //    public static ApplicationDbContext Create() {
        //        return new ApplicationDbContext();
        //    }
        //}

        
    }
}