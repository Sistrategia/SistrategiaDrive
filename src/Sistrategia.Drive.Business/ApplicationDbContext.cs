﻿//using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistrategia.Drive.Business
{
    public class ApplicationDbContext : IdentityDbContext<SecurityUser, SecurityRole
        , string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        //public ApplicationDbContext()
        //    : base("DefaultDatabase", throwIfV1Schema: false) {
        //}

        public ApplicationDbContext()
            : base("DefaultDatabase") {
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder) {
            //  base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<IdentityUser>()
            //    .ToTable("security_user", "dbo").Property(p => p.Id).HasColumnName("user_id");

            var user = modelBuilder.Entity<SecurityUser>()
                .ToTable("security_user");
            user.Property(u => u.Id).HasColumnName("user_id");
            //user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .HasColumnName("user_name")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("user_name_index") { IsUnique = true }));
            user.Property(u => u.Email)
                .HasColumnName("email")
                .HasMaxLength(256);
            user.Property(u => u.EmailConfirmed)
                .HasColumnName("email_confirmed");
            user.Property(u => u.PasswordHash)
                .HasColumnName("password_hash");
            user.Property(u => u.SecurityStamp)
                .HasColumnName("security_stamp");
            user.Property(u => u.PhoneNumber)
                .HasColumnName("phone_number");
            user.Property(u => u.PhoneNumberConfirmed)
                .HasColumnName("phone_number_confirmed");
            user.Property(u => u.TwoFactorEnabled)
                .HasColumnName("two_factor_enabled");
            user.Property(u => u.LockoutEndDateUtc)
                .HasColumnName("lockout_end_date_utc");
            user.Property(u => u.LockoutEnabled)
                .HasColumnName("lockout_enabled");
            user.Property(u => u.AccessFailedCount)
                .HasColumnName("access_failed_count");

            var role = modelBuilder.Entity<SecurityRole>()
               .ToTable("security_roles");
            role.Property(r => r.Id).HasColumnName("role_id");
            role.Property(r => r.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("RoleNameIndex") { IsUnique = true }));

            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);

            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);

            //modelBuilder.Entity<User>()
            //    .ToTable("Users", "dbo").Property(p => p.Id).HasColumnName("User_Id");

            //modelBuilder.Entity<IdentityRole>().ToTable("security_roles");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("security_user_roles");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("security_user_logins");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("security_user_claims");



            // CONSIDER: u.Email is Required if set on options?
            // user.Property(u => u.Email).HasMaxLength(256);

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("security_user_roles");

            modelBuilder.Entity<IdentityUserLogin>()
                 .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                 .ToTable("security_user_logins");

            modelBuilder.Entity<IdentityUserClaim>().ToTable("security_user_claims");

            



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
