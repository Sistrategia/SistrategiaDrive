using System;
//using System.Data.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistrategia.Drive.Business
{
    public class SecurityUser : IdentityUser<int, SecurityUserLogin, SecurityUserRole, SecurityUserClaim>
    {
        public SecurityUser() 
            : base()
        {
            this.PublicKey = Guid.NewGuid();
        }

        /// <summary>
        ///     Constructor that takes a userName
        /// </summary>
        /// <param name="userName"></param>
        public SecurityUser(string userName)
            : this()
        {
            UserName = userName;
        }

        [Required]
        public Guid PublicKey { get; set; }

        [MaxLength(256)]
        public string FullName { get; set; }

        public virtual IList<DriveItem> DriveItems { get; set; }

        public virtual IList<CloudStorageAccount> CloudStorageAccounts { get; set; }

        public virtual IList<CloudStorageItem> CloudStorageItems { get; set; }

        [ForeignKey("DefaultContainer")]
        public int? DefaultContainerId { get; set; }
        public virtual CloudStorageContainer DefaultContainer { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync( UserManager<SecurityUser, int> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }        
    }
}
