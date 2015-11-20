namespace Sistrategia.Drive.WebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.cloud_storage_account",
                c => new
                    {
                        cloud_storage_account_id = c.Int(nullable: false, identity: true),
                        cloud_storage_provider_id = c.String(maxLength: 128),
                        public_key = c.Guid(nullable: false),
                        provider_key = c.String(nullable: false, maxLength: 128),
                        account_name = c.String(maxLength: 512),
                        account_key = c.String(maxLength: 1024),
                        alias = c.String(maxLength: 256),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.cloud_storage_account_id)
                .ForeignKey("dbo.cloud_storage_provider", t => t.cloud_storage_provider_id)
                .Index(t => t.cloud_storage_provider_id)
                .Index(t => t.public_key);
            
            CreateTable(
                "dbo.cloud_storage_container",
                c => new
                    {
                        cloud_storage_container_id = c.Int(nullable: false, identity: true),
                        cloud_storage_account_id = c.Int(nullable: false),
                        public_key = c.Guid(nullable: false),
                        provider_key = c.String(nullable: false, maxLength: 128),
                        container_name = c.String(maxLength: 512),
                        alias = c.String(maxLength: 256),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.cloud_storage_container_id)
                .ForeignKey("dbo.cloud_storage_account", t => t.cloud_storage_account_id, cascadeDelete: true)
                .Index(t => t.cloud_storage_account_id)
                .Index(t => t.public_key);
            
            CreateTable(
                "dbo.cloud_storage_item",
                c => new
                    {
                        cloud_storage_item_id = c.Int(nullable: false, identity: true),
                        cloud_storage_container_id = c.Int(nullable: false),
                        public_key = c.Guid(nullable: false),
                        provider_key = c.String(nullable: false, maxLength: 1024),
                        name = c.String(nullable: false, maxLength: 2048),
                        description = c.String(),
                        created = c.DateTime(nullable: false),
                        modified = c.DateTime(nullable: false),
                        content_type = c.String(nullable: false, maxLength: 255),
                        content_md5 = c.String(),
                        original_name = c.String(maxLength: 2048),
                        url = c.String(),
                        SecurityUser_Id = c.Int(),
                    })
                .PrimaryKey(t => t.cloud_storage_item_id)
                .ForeignKey("dbo.cloud_storage_container", t => t.cloud_storage_container_id, cascadeDelete: true)
                .ForeignKey("dbo.security_user", t => t.SecurityUser_Id)
                .Index(t => t.cloud_storage_container_id)
                .Index(t => t.public_key)
                .Index(t => t.SecurityUser_Id);
            
            CreateTable(
                "dbo.cloud_storage_provider",
                c => new
                    {
                        cloud_storage_provider_id = c.String(nullable: false, maxLength: 128),
                        name = c.String(maxLength: 512),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.cloud_storage_provider_id);
            
            CreateTable(
                "dbo.drive_item",
                c => new
                    {
                        drive_item_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        owner_id = c.Int(nullable: false),
                        cloud_storage_container_id = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 2048),
                        description = c.String(),
                        created = c.DateTime(nullable: false),
                        modified = c.DateTime(nullable: false),
                        content_type = c.String(nullable: false, maxLength: 255),
                        content_md5 = c.String(),
                        original_name = c.String(maxLength: 2048),
                        url = c.String(),
                    })
                .PrimaryKey(t => t.drive_item_id)
                .ForeignKey("dbo.cloud_storage_item", t => t.cloud_storage_container_id, cascadeDelete: true)
                .ForeignKey("dbo.security_user", t => t.owner_id, cascadeDelete: true)
                .Index(t => t.public_key)
                .Index(t => t.owner_id)
                .Index(t => t.cloud_storage_container_id);
            
            CreateTable(
                "dbo.security_user",
                c => new
                    {
                        user_id = c.Int(nullable: false, identity: true),
                        user_name = c.String(nullable: false, maxLength: 256),
                        public_key = c.Guid(nullable: false),
                        default_container_id = c.Int(),
                        email = c.String(maxLength: 256),
                        email_confirmed = c.Boolean(nullable: false),
                        password_hash = c.String(),
                        security_stamp = c.String(),
                        phone_number = c.String(),
                        phone_number_confirmed = c.Boolean(nullable: false),
                        two_factor_enabled = c.Boolean(nullable: false),
                        lockout_end_date_utc = c.DateTime(),
                        lockout_enabled = c.Boolean(nullable: false),
                        access_failed_count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.user_id)
                .ForeignKey("dbo.cloud_storage_container", t => t.default_container_id)
                .Index(t => t.user_name, unique: true, name: "ix_user_name_index")
                .Index(t => t.public_key, unique: true, name: "ix_user_public_key_index")
                .Index(t => t.default_container_id);
            
            CreateTable(
                "dbo.security_user_claims",
                c => new
                    {
                        claim_id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        claim_type = c.String(),
                        claim_value = c.String(),
                    })
                .PrimaryKey(t => t.claim_id)
                .ForeignKey("dbo.security_user", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.security_user_logins",
                c => new
                    {
                        login_provider = c.String(nullable: false, maxLength: 128),
                        provider_key = c.String(nullable: false, maxLength: 128),
                        user_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.login_provider, t.provider_key, t.user_id })
                .ForeignKey("dbo.security_user", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.security_user_roles",
                c => new
                    {
                        user_id = c.Int(nullable: false),
                        role_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.user_id, t.role_id })
                .ForeignKey("dbo.security_user", t => t.user_id, cascadeDelete: true)
                .ForeignKey("dbo.security_roles", t => t.role_id, cascadeDelete: true)
                .Index(t => t.user_id)
                .Index(t => t.role_id);
            
            CreateTable(
                "dbo.security_roles",
                c => new
                    {
                        role_id = c.Int(nullable: false, identity: true),
                        role_name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.role_id)
                .Index(t => t.role_name, unique: true, name: "ix_role_name_index");
            
            CreateTable(
                "dbo.security_user_cloud_storage_account",
                c => new
                    {
                        user_id = c.Int(nullable: false),
                        cloud_storage_account_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.user_id, t.cloud_storage_account_id })
                .ForeignKey("dbo.security_user", t => t.user_id)
                .ForeignKey("dbo.cloud_storage_account", t => t.cloud_storage_account_id)
                .Index(t => t.user_id)
                .Index(t => t.cloud_storage_account_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.security_user_roles", "role_id", "dbo.security_roles");
            DropForeignKey("dbo.drive_item", "owner_id", "dbo.security_user");
            DropForeignKey("dbo.security_user_roles", "user_id", "dbo.security_user");
            DropForeignKey("dbo.security_user_logins", "user_id", "dbo.security_user");
            DropForeignKey("dbo.security_user", "default_container_id", "dbo.cloud_storage_container");
            DropForeignKey("dbo.cloud_storage_item", "SecurityUser_Id", "dbo.security_user");
            DropForeignKey("dbo.security_user_cloud_storage_account", "cloud_storage_account_id", "dbo.cloud_storage_account");
            DropForeignKey("dbo.security_user_cloud_storage_account", "user_id", "dbo.security_user");
            DropForeignKey("dbo.security_user_claims", "user_id", "dbo.security_user");
            DropForeignKey("dbo.drive_item", "cloud_storage_container_id", "dbo.cloud_storage_item");
            DropForeignKey("dbo.cloud_storage_account", "cloud_storage_provider_id", "dbo.cloud_storage_provider");
            DropForeignKey("dbo.cloud_storage_item", "cloud_storage_container_id", "dbo.cloud_storage_container");
            DropForeignKey("dbo.cloud_storage_container", "cloud_storage_account_id", "dbo.cloud_storage_account");
            DropIndex("dbo.security_user_cloud_storage_account", new[] { "cloud_storage_account_id" });
            DropIndex("dbo.security_user_cloud_storage_account", new[] { "user_id" });
            DropIndex("dbo.security_roles", "ix_role_name_index");
            DropIndex("dbo.security_user_roles", new[] { "role_id" });
            DropIndex("dbo.security_user_roles", new[] { "user_id" });
            DropIndex("dbo.security_user_logins", new[] { "user_id" });
            DropIndex("dbo.security_user_claims", new[] { "user_id" });
            DropIndex("dbo.security_user", new[] { "default_container_id" });
            DropIndex("dbo.security_user", "ix_user_public_key_index");
            DropIndex("dbo.security_user", "ix_user_name_index");
            DropIndex("dbo.drive_item", new[] { "cloud_storage_container_id" });
            DropIndex("dbo.drive_item", new[] { "owner_id" });
            DropIndex("dbo.drive_item", new[] { "public_key" });
            DropIndex("dbo.cloud_storage_item", new[] { "SecurityUser_Id" });
            DropIndex("dbo.cloud_storage_item", new[] { "public_key" });
            DropIndex("dbo.cloud_storage_item", new[] { "cloud_storage_container_id" });
            DropIndex("dbo.cloud_storage_container", new[] { "public_key" });
            DropIndex("dbo.cloud_storage_container", new[] { "cloud_storage_account_id" });
            DropIndex("dbo.cloud_storage_account", new[] { "public_key" });
            DropIndex("dbo.cloud_storage_account", new[] { "cloud_storage_provider_id" });
            DropTable("dbo.security_user_cloud_storage_account");
            DropTable("dbo.security_roles");
            DropTable("dbo.security_user_roles");
            DropTable("dbo.security_user_logins");
            DropTable("dbo.security_user_claims");
            DropTable("dbo.security_user");
            DropTable("dbo.drive_item");
            DropTable("dbo.cloud_storage_provider");
            DropTable("dbo.cloud_storage_item");
            DropTable("dbo.cloud_storage_container");
            DropTable("dbo.cloud_storage_account");
        }
    }
}
