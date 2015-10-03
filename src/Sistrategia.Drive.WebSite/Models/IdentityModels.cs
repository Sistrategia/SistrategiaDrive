//using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistrategia.Drive.WebSite.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    //public class ApplicationUser : IUser
    {
        //private string id;
        //private string userName;

        //public string Id {
        //    get { return this.id; }
        //    set { this.id = value; } // This must be removed, IUser doesn't include the setter.
        //}

        //public string UserName {
        //    get { return this.userName; }
        //    set { this.userName = value; }
        //}

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultDatabase", throwIfV1Schema: false) {
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder) {
          //  base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<IdentityUser>()
            //    .ToTable("security_user", "dbo").Property(p => p.Id).HasColumnName("user_id");

            var user = modelBuilder.Entity<IdentityUser>()
                .ToTable("security_user");
            user.Property(u => u.Id).HasColumnName("user_id");
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .HasColumnName("user_name")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("user_name_index") { IsUnique = true }));

            //modelBuilder.Entity<User>()
            //    .ToTable("Users", "dbo").Property(p => p.Id).HasColumnName("User_Id");

            //modelBuilder.Entity<IdentityRole>().ToTable("security_roles");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("security_user_roles");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("security_user_logins");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("security_user_claims");

            

            // CONSIDER: u.Email is Required if set on options?
            user.Property(u => u.Email).HasMaxLength(256);

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("security_user_roles");

            modelBuilder.Entity<IdentityUserLogin>()
                 .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                 .ToTable("security_user_logins");

            modelBuilder.Entity<IdentityUserClaim>().ToTable("security_user_claims");

            var role = modelBuilder.Entity<IdentityRole>()
               .ToTable("security_roles");
            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);

            

            //modelBuilder.Entity<User>()
            //    .ToTable("Users", "dbo").Property(p => p.Id).HasColumnName("User_Id");


            //if (modelBuilder == null) {
            //    throw new ArgumentNullException("modelBuilder");
            //}

            //// Needed to ensure subclasses share the same table
            //var user = modelBuilder.Entity<TUser>()
            //    .ToTable("AspNetUsers");
            //user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            //user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            //user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            //user.Property(u => u.UserName)
            //    .IsRequired()
            //    .HasMaxLength(256)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("UserNameIndex") { IsUnique = true }));

            //// CONSIDER: u.Email is Required if set on options?
            //user.Property(u => u.Email).HasMaxLength(256);

            //modelBuilder.Entity<TUserRole>()
            //    .HasKey(r => new { r.UserId, r.RoleId })
            //    .ToTable("AspNetUserRoles");

            //modelBuilder.Entity<TUserLogin>()
            //    .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
            //    .ToTable("AspNetUserLogins");

            //modelBuilder.Entity<TUserClaim>()
            //    .ToTable("AspNetUserClaims");

            //var role = modelBuilder.Entity<TRole>()
            //    .ToTable("AspNetRoles");
            //role.Property(r => r.Name)
            //    .IsRequired()
            //    .HasMaxLength(256)
            //    .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true }));
            //role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);

        }
    }



}