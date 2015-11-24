using System;
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

        public virtual DbSet<DriveItem> DriveItems { get; set; }

        public virtual DbSet<CloudStorageProvider>  CloudStorageProviders { get; set; }
        public virtual DbSet<CloudStorageAccount>   CloudStorageAccounts { get; set; }
        public virtual DbSet<CloudStorageContainer> CloudStorageContainers { get; set; }
        public virtual DbSet<CloudStorageItem>      CloudStorageItems { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder) {
            // Guide and How To's:
            // https://msdn.microsoft.com/en-us/data/jj591617.aspx

            // Horrible temp hack:
            // http://stackoverflow.com/questions/13705441/how-to-disable-cascade-delete-for-link-tables-in-ef-code-first
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.ManyToManyCascadeDeleteConvention>();

            //public override int SaveChanges()
            //{
            //    Bookings.Local
            //            .Where(r => r.ContactId == null)
            //            .ToList()
            //            .ForEach(r => Bookings.Remove(r));

            //    return base.SaveChanges();
            // }

            var user = modelBuilder.Entity<SecurityUser>()
                .ToTable("security_user");
            user.Property(u => u.Id).HasColumnName("user_id")
                .HasColumnOrder(1);
            //user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
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

            user.Property(p => p.FullName)
                .HasColumnName("full_name")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_user_full_name_index") { IsUnique = false }));

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

            user.Property(p => p.DefaultContainerId)
                .HasColumnName("default_container_id");

            var role = modelBuilder.Entity<SecurityRole>()
               .ToTable("security_roles");
            role.Property(r => r.Id).HasColumnName("role_id");
            role.Property(r => r.Name)
                .HasColumnName("role_name")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_role_name_index") { IsUnique = true }));

            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);
            //var f = role.HasMany(r => r.Users).WithRequired();
            //f.HasForeignKey(ur => ur.RoleId);
            //f.Map(pr => pr.MapKey(("role_id")));
            
            //role.HasMany(r => r.Users).WithRequired().Map(pr=>pr.MapKey("RoleId")).WillCascadeOnDelete  .HasForeignKey(ur => ur.RoleId);

            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);

            //modelBuilder.Entity<User>()
            //    .ToTable("Users", "dbo").Property(p => p.Id).HasColumnName("User_Id");

            //modelBuilder.Entity<IdentityRole>().ToTable("security_roles");
            //modelBuilder.Entity<IdentityUserRole>().ToTable("security_user_roles");
            //modelBuilder.Entity<IdentityUserLogin>().ToTable("security_user_logins");
            //modelBuilder.Entity<IdentityUserClaim>().ToTable("security_user_claims");



            // CONSIDER: u.Email is Required if set on options?
            // user.Property(u => u.Email).HasMaxLength(256);

            var userRole = modelBuilder.Entity<SecurityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("security_user_roles")
                //.Property(pr1 => pr1.RoleId).HasColumnName("role_id");                
                ;
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

            var cloudStorageProvider = modelBuilder.Entity<CloudStorageProvider>()
                .ToTable("cloud_storage_provider");
            cloudStorageProvider.Property(p => p.CloudStorageProviderId)
                .HasColumnName("cloud_storage_provider_id");
            cloudStorageProvider.Property(p => p.Name)
                .HasColumnName("name");
            cloudStorageProvider.Property(p => p.Description)
                .HasColumnName("description");

            var cloudStorageAccount = modelBuilder.Entity<CloudStorageAccount>()
                .ToTable("cloud_storage_account");
            cloudStorageAccount.Property(p => p.CloudStorageAccountId)
                .HasColumnName("cloud_storage_account_id")                
                //.HasColumnOrder(1)
                ;

            cloudStorageAccount.Property(p => p.CloudStorageProviderId)
                .HasColumnName("cloud_storage_provider_id")                
                ;
            //cloudStorageAccount.HasRequired<CloudStorageProvider>(a => a.CloudStorageProvider)
            //    .WithMany().Map(p => p.MapKey("cloud_storage_provider_id"));

            // If you want to control all mapping here without DataAnnotations on Model Classes use this:
            //cloudStorageAccount.Property(p => p.CloudStorageProviderId)
            //    .HasColumnName("cloud_storage_provider_id")//.HasColumnOrder(2)                
            //    ;
            //cloudStorageAccount.HasRequired<CloudStorageProvider>(a => a.CloudStorageProvider)
            //    .WithMany().HasForeignKey(f => f.CloudStorageProviderId).WillCascadeOnDelete(false);

            cloudStorageAccount.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));

            cloudStorageAccount.Property(p => p.ProviderKey)
                .HasColumnName("provider_key");

            cloudStorageAccount.Property(p => p.AccountName)
                .HasColumnName("account_name");

            cloudStorageAccount.Property(p => p.Alias)
                .HasColumnName("alias");
            cloudStorageAccount.Property(p => p.Description)
                .HasColumnName("description");

            cloudStorageAccount.Property(p => p.AccountKey)
                .HasColumnName("account_key");


            modelBuilder.Entity<SecurityUser>()
                .HasMany(u => u.CloudStorageAccounts)
                .WithMany()
                //.WithMany(i => i.Courses)
                .Map(t => t.MapLeftKey("user_id") // security_user_id
                .MapRightKey("cloud_storage_account_id")
                .ToTable("security_user_cloud_storage_account"))
                ;

            //modelBuilder.Entity<CloudStorageAccount>()
                


            var cloudStorageContainer = modelBuilder.Entity<CloudStorageContainer>()
                .ToTable("cloud_storage_container");
            cloudStorageContainer.Property(p => p.CloudStorageContainerId)
                .HasColumnName("cloud_storage_container_id");
            cloudStorageContainer.Property(p => p.CloudStorageAccountId)
                .HasColumnName("cloud_storage_account_id")
                ;

            cloudStorageContainer.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));

            cloudStorageContainer.Property(p => p.ProviderKey)
                .HasColumnName("provider_key");

            cloudStorageContainer.Property(p => p.ContainerName)
                .HasColumnName("container_name");

            cloudStorageContainer.Property(p => p.Alias)
                .HasColumnName("alias");
            cloudStorageContainer.Property(p => p.Description)
                .HasColumnName("description");

            //cloudStorageContainer.Property(p => p.AccountKey)
            //    .HasColumnName("account_key");

            //modelBuilder.Entity<CloudStorageAccount>()
            //    .HasMany(u => u.CloudStorageAccounts)
            //    .WithMany()
            //    //.WithMany(i => i.Courses)
            //    .Map(t => t.MapLeftKey("user_id") // security_user_id
            //    .MapRightKey("cloud_storage_account_id")
            //    .ToTable("security_user_cloud_storage_account"))
            //    ;

            var cloudStorageItem = modelBuilder.Entity<CloudStorageItem>()
                .ToTable("cloud_storage_item");
            cloudStorageItem.Property(p => p.CloudStorageItemId)
                .HasColumnName("cloud_storage_item_id");
            cloudStorageItem.Property(p => p.CloudStorageContainerId)
                .HasColumnName("cloud_storage_container_id");

            //cloudStorageItem.Property(p => p.OwnerId)
            //    .HasColumnName("owner_id");

            cloudStorageItem.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));

            cloudStorageItem.Property(p => p.ProviderKey)
                .HasColumnName("provider_key");

            //cloudStorageItem.Property(p => p.OwnerId)
            //    .HasColumnName("owner_id");

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



            var driveItem = modelBuilder.Entity<DriveItem>()
                .ToTable("drive_item");
            driveItem.Property(p => p.DriveItemId)
                .HasColumnName("drive_item_id");

            driveItem.Property(p => p.PublicKey)
                .HasColumnName("public_key")
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute()));

            //driveItem.Property(p => p.ProviderKey)
            //    .HasColumnName("provider_key");

            driveItem.Property(p => p.OwnerId)
                .HasColumnName("owner_id");

            driveItem.Property(p => p.CloudStorageItemId)
                .HasColumnName("cloud_storage_item_id");

            driveItem.Property(p => p.Name)
                .HasColumnName("name");
            driveItem.Property(p => p.Description)
                .HasColumnName("description");

            driveItem.Property(p => p.Created)
                .HasColumnName("created");

            driveItem.Property(p => p.Modified)
                .HasColumnName("modified");

            driveItem.Property(p => p.ContentType)
                .HasColumnName("content_type");

            driveItem.Property(p => p.ContentMD5)
                .HasColumnName("content_md5");

            driveItem.Property(p => p.OriginalName)
                .HasColumnName("original_name");

            driveItem.Property(p => p.Url)
                .HasColumnName("url");

            
        }
    }
}
