namespace Ogloszenia_drobne.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advertisements",
                c => new
                    {
                        AdvertisementId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                        VisitCounter = c.Int(nullable: false),
                        Reported = c.Boolean(nullable: false),
                        User_Login = c.String(nullable: false),
                        User_Email = c.String(nullable: false),
                        User_Name = c.String(nullable: false),
                        User_Surname = c.String(nullable: false),
                        User_Password = c.String(nullable: false, maxLength: 100),
                        User_ConfirmPassword = c.String(),
                        User_Address = c.String(),
                        User_Phone = c.Int(nullable: false),
                        User_IsAdmin = c.Boolean(nullable: false),
                        User_NumAdOnPg = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdvertisementId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.CategoryAttributes",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        AttributeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.AttributeId })
                .ForeignKey("dbo.Attributes", t => t.AttributeId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.AttributeId);
            
            CreateTable(
                "dbo.Attributes",
                c => new
                    {
                        AttributeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.AttributeId);
            
            CreateTable(
                "dbo.AttributeValues",
                c => new
                    {
                        AdvertisementId = c.Int(nullable: false),
                        AttributeId = c.Int(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.AdvertisementId, t.AttributeId })
                .ForeignKey("dbo.Attributes", t => t.AttributeId, cascadeDelete: true)
                .Index(t => t.AttributeId);
            
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Path = c.String(),
                        Description = c.String(),
                        InDetails = c.Boolean(nullable: false),
                        User_Login = c.String(nullable: false),
                        User_Email = c.String(nullable: false),
                        User_Name = c.String(nullable: false),
                        User_Surname = c.String(nullable: false),
                        User_Password = c.String(nullable: false, maxLength: 100),
                        User_ConfirmPassword = c.String(),
                        User_Address = c.String(),
                        User_Phone = c.Int(nullable: false),
                        User_IsAdmin = c.Boolean(nullable: false),
                        User_NumAdOnPg = c.Int(nullable: false),
                        Advertisement_AdvertisementId = c.Int(),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.Advertisements", t => t.Advertisement_AdvertisementId)
                .Index(t => t.Advertisement_AdvertisementId);
            
            CreateTable(
                "dbo.BannedWords",
                c => new
                    {
                        BannedWordId = c.Int(nullable: false, identity: true),
                        Word = c.String(),
                    })
                .PrimaryKey(t => t.BannedWordId);
            
            CreateTable(
                "dbo.Properties",
                c => new
                    {
                        PropertyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PropertyId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ShortMessages",
                c => new
                    {
                        ShortMessageId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Content = c.String(nullable: false),
                        AddedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ShortMessageId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Files", "Advertisement_AdvertisementId", "dbo.Advertisements");
            DropForeignKey("dbo.Advertisements", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CategoryAttributes", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CategoryAttributes", "AttributeId", "dbo.Attributes");
            DropForeignKey("dbo.AttributeValues", "AttributeId", "dbo.Attributes");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Files", new[] { "Advertisement_AdvertisementId" });
            DropIndex("dbo.AttributeValues", new[] { "AttributeId" });
            DropIndex("dbo.CategoryAttributes", new[] { "AttributeId" });
            DropIndex("dbo.CategoryAttributes", new[] { "CategoryId" });
            DropIndex("dbo.Advertisements", new[] { "CategoryId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ShortMessages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Properties");
            DropTable("dbo.BannedWords");
            DropTable("dbo.Files");
            DropTable("dbo.AttributeValues");
            DropTable("dbo.Attributes");
            DropTable("dbo.CategoryAttributes");
            DropTable("dbo.Categories");
            DropTable("dbo.Advertisements");
        }
    }
}
