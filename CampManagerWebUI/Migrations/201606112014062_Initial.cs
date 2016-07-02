namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseOrganizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Organization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: false)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Camps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        CampOrganization_Id = c.Int(nullable: false),
                        Place_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SeasonOrganizations", t => t.CampOrganization_Id, cascadeDelete: false)
                .ForeignKey("dbo.Places", t => t.Place_Id, cascadeDelete: false)
                .Index(t => t.CampOrganization_Id)
                .Index(t => t.Place_Id);
            
            CreateTable(
                "dbo.SeasonOrganizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateStart = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                        Base_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseOrganizations", t => t.Base_Id, cascadeDelete: false)
                .Index(t => t.Base_Id);
            
            CreateTable(
                "dbo.Places",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Base_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseOrganizations", t => t.Base_Id, cascadeDelete: false)
                .Index(t => t.Base_Id);
            
            CreateTable(
                "dbo.CampMeals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Kind = c.Int(nullable: false),
                        Eat = c.Int(nullable: false),
                        EatSupplies = c.Int(nullable: false),
                        Cash = c.Int(nullable: false),
                        Camp_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Camps", t => t.Camp_Id, cascadeDelete: false)
                .Index(t => t.Camp_Id);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        DateDelivery = c.DateTime(nullable: false),
                        DateIssue = c.DateTime(nullable: false),
                        DateIntroduction = c.DateTime(nullable: false),
                        Season_Id = c.Int(nullable: false),
                        Supplier_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SeasonOrganizations", t => t.Season_Id, cascadeDelete: false)
                .ForeignKey("dbo.SupplierOrganizations", t => t.Supplier_Id, cascadeDelete: false)
                .Index(t => t.Season_Id)
                .Index(t => t.Supplier_Id);
            
            CreateTable(
                "dbo.SupplierOrganizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Organization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: false)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.InvoicePositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 8),
                        Invoice_Id = c.Int(nullable: false),
                        Product_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id, cascadeDelete: false)
                .ForeignKey("dbo.ProductOrganizations", t => t.Product_Id, cascadeDelete: false)
                .Index(t => t.Invoice_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.ProductOrganizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Measure_Id = c.Int(nullable: false),
                        Organization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MeasureOrganizations", t => t.Measure_Id, cascadeDelete: false)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: false)
                .Index(t => t.Measure_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.MeasureOrganizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Organization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: false)
                .Index(t => t.Organization_Id);
            
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
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.InvoicePositions", "Product_Id", "dbo.ProductOrganizations");
            DropForeignKey("dbo.ProductOrganizations", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.ProductOrganizations", "Measure_Id", "dbo.MeasureOrganizations");
            DropForeignKey("dbo.MeasureOrganizations", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.InvoicePositions", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "Supplier_Id", "dbo.SupplierOrganizations");
            DropForeignKey("dbo.SupplierOrganizations", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Invoices", "Season_Id", "dbo.SeasonOrganizations");
            DropForeignKey("dbo.CampMeals", "Camp_Id", "dbo.Camps");
            DropForeignKey("dbo.Camps", "Place_Id", "dbo.Places");
            DropForeignKey("dbo.Places", "Base_Id", "dbo.BaseOrganizations");
            DropForeignKey("dbo.Camps", "CampOrganization_Id", "dbo.SeasonOrganizations");
            DropForeignKey("dbo.SeasonOrganizations", "Base_Id", "dbo.BaseOrganizations");
            DropForeignKey("dbo.BaseOrganizations", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.MeasureOrganizations", new[] { "Organization_Id" });
            DropIndex("dbo.ProductOrganizations", new[] { "Organization_Id" });
            DropIndex("dbo.ProductOrganizations", new[] { "Measure_Id" });
            DropIndex("dbo.InvoicePositions", new[] { "Product_Id" });
            DropIndex("dbo.InvoicePositions", new[] { "Invoice_Id" });
            DropIndex("dbo.SupplierOrganizations", new[] { "Organization_Id" });
            DropIndex("dbo.Invoices", new[] { "Supplier_Id" });
            DropIndex("dbo.Invoices", new[] { "Season_Id" });
            DropIndex("dbo.CampMeals", new[] { "Camp_Id" });
            DropIndex("dbo.Places", new[] { "Base_Id" });
            DropIndex("dbo.SeasonOrganizations", new[] { "Base_Id" });
            DropIndex("dbo.Camps", new[] { "Place_Id" });
            DropIndex("dbo.Camps", new[] { "CampOrganization_Id" });
            DropIndex("dbo.BaseOrganizations", new[] { "Organization_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MeasureOrganizations");
            DropTable("dbo.ProductOrganizations");
            DropTable("dbo.InvoicePositions");
            DropTable("dbo.SupplierOrganizations");
            DropTable("dbo.Invoices");
            DropTable("dbo.CampMeals");
            DropTable("dbo.Places");
            DropTable("dbo.SeasonOrganizations");
            DropTable("dbo.Camps");
            DropTable("dbo.Organizations");
            DropTable("dbo.BaseOrganizations");
        }
    }
}
