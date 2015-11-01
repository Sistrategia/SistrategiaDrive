namespace Sistrategia.Drive.WebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.security_roles",
                c => new
                    {
                        role_id = c.String(nullable: false, maxLength: 128),
                        role_name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.role_id)
                .Index(t => t.role_name, unique: true, name: "role_name_index");
            
            CreateTable(
                "dbo.security_user_roles",
                c => new
                    {
                        user_id = c.String(nullable: false, maxLength: 128),
                        role_id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.user_id, t.role_id })
                .ForeignKey("dbo.security_roles", t => t.role_id, cascadeDelete: true)
                .ForeignKey("dbo.security_user", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id)
                .Index(t => t.role_id);
            
            CreateTable(
                "dbo.security_user",
                c => new
                    {
                        user_id = c.String(nullable: false, maxLength: 128),
                        user_name = c.String(nullable: false, maxLength: 256),
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
                .Index(t => t.user_name, unique: true, name: "user_name_index");
            
            CreateTable(
                "dbo.security_user_claims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.security_user", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.security_user_logins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.security_user", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.security_user_roles", "user_id", "dbo.security_user");
            DropForeignKey("dbo.security_user_logins", "UserId", "dbo.security_user");
            DropForeignKey("dbo.security_user_claims", "UserId", "dbo.security_user");
            DropForeignKey("dbo.security_user_roles", "role_id", "dbo.security_roles");
            DropIndex("dbo.security_user_logins", new[] { "UserId" });
            DropIndex("dbo.security_user_claims", new[] { "UserId" });
            DropIndex("dbo.security_user", "user_name_index");
            DropIndex("dbo.security_user_roles", new[] { "role_id" });
            DropIndex("dbo.security_user_roles", new[] { "user_id" });
            DropIndex("dbo.security_roles", "role_name_index");
            DropTable("dbo.security_user_logins");
            DropTable("dbo.security_user_claims");
            DropTable("dbo.security_user");
            DropTable("dbo.security_user_roles");
            DropTable("dbo.security_roles");
        }
    }
}
