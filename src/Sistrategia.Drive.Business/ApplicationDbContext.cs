﻿using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistrategia.Drive.Business
{
    public class ApplicationDbContext : IdentityDbContext<SecurityUser, SecurityRole
        , int, SecurityUserLogin, SecurityUserRole, SecurityUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultDatabase") {
        }

        public static ApplicationDbContext Create() {
            return new ApplicationDbContext();
        }
        
        //public virtual DbSet<CloudStorageItem> CloudStorageItems { get; set; }
        public virtual DbSet<DriveItem> DriveItems { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.ManyToManyCascadeDeleteConvention>();

            var user = modelBuilder.Entity<SecurityUser>()
                .ToTable("security_user");
            user.Property(u => u.Id).HasColumnName("user_id")
                .HasColumnOrder(1);            
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName)
                .HasColumnName("user_name")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnOrder(2)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_user_name_index") { IsUnique = true }));
            user.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_user_public_key_index") { IsUnique = true }));
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
                .HasColumnName("role_name")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_role_name_index") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);           

            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);

            var userRole = modelBuilder.Entity<SecurityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("security_user_roles");
            userRole.Property(pr1 => pr1.RoleId).HasColumnName("role_id");
            userRole.Property(pr2 => pr2.UserId).HasColumnName("user_id");

            var userLogin = modelBuilder.Entity<SecurityUserLogin>()
                 .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                 .ToTable("security_user_logins");
            userLogin.Property(pr1 => pr1.LoginProvider).HasColumnName("login_provider");
            userLogin.Property(pr2 => pr2.ProviderKey).HasColumnName("provider_key");
            userLogin.Property(pr3 => pr3.UserId).HasColumnName("user_id");

            var userClaim = modelBuilder.Entity<SecurityUserClaim>()
                .ToTable("security_user_claims");
            userClaim.Property(pr1 => pr1.Id).HasColumnName("claim_id");
            userClaim.Property(pr2 => pr2.UserId).HasColumnName("user_id");
            userClaim.Property(pr3 => pr3.ClaimType).HasColumnName("claim_type");
            userClaim.Property(pr4 => pr4.ClaimValue).HasColumnName("claim_value");


            var cloudStorageItem = modelBuilder.Entity<DriveItem>()
                .ToTable("drive_item");
            cloudStorageItem.Property(p => p.DriveItemId)
                .HasColumnName("drive_item_id");
            
            cloudStorageItem.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));

            cloudStorageItem.Property(p => p.ProviderKey)
                .HasColumnName("provider_key");

            cloudStorageItem.Property(p => p.OwnerId)
                .HasColumnName("owner_id");

            cloudStorageItem.Property(p => p.Name)
                .HasColumnName("name");
            cloudStorageItem.Property(p => p.Description)
                .HasColumnName("description");

            cloudStorageItem.Property(p => p.Created)
                .HasColumnName("created");

            cloudStorageItem.Property(p => p.Modified)
                .HasColumnName("modified");

            cloudStorageItem.Property(p => p.ContentType)
                .HasColumnName("content_type");

            cloudStorageItem.Property(p => p.ContentMD5)
                .HasColumnName("content_md5");

            cloudStorageItem.Property(p => p.OriginalName)
                .HasColumnName("original_name");            
            
            cloudStorageItem.Property(p => p.Url)
                .HasColumnName("url");
        }
    }
}
