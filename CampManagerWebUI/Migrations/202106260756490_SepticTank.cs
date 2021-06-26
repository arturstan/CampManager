namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SepticTank : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SepticTanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                        Kind_Id = c.Int(nullable: false),
                        Season_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SepticTankKindOrganizations", t => t.Kind_Id, cascadeDelete: true)
                .ForeignKey("dbo.SeasonOrganizations", t => t.Season_Id, cascadeDelete: true)
                .Index(t => t.Kind_Id)
                .Index(t => t.Season_Id);
            
            CreateTable(
                "dbo.SepticTankKindOrganizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Capacity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Organization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: true)
                .Index(t => t.Organization_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SepticTanks", "Season_Id", "dbo.SeasonOrganizations");
            DropForeignKey("dbo.SepticTanks", "Kind_Id", "dbo.SepticTankKindOrganizations");
            DropForeignKey("dbo.SepticTankKindOrganizations", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.SepticTankKindOrganizations", new[] { "Organization_Id" });
            DropIndex("dbo.SepticTanks", new[] { "Season_Id" });
            DropIndex("dbo.SepticTanks", new[] { "Kind_Id" });
            DropTable("dbo.SepticTankKindOrganizations");
            DropTable("dbo.SepticTanks");
        }
    }
}
