namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Garbage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Garbages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Collection = c.Boolean(nullable: false),
                        Kind_Id = c.Int(),
                        Season_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GarbageKindOrganizations", t => t.Kind_Id)
                .ForeignKey("dbo.SeasonOrganizations", t => t.Season_Id)
                .Index(t => t.Kind_Id)
                .Index(t => t.Season_Id);
            
            CreateTable(
                "dbo.GarbageKindOrganizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Garbages", "Season_Id", "dbo.SeasonOrganizations");
            DropForeignKey("dbo.Garbages", "Kind_Id", "dbo.GarbageKindOrganizations");
            DropForeignKey("dbo.GarbageKindOrganizations", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.GarbageKindOrganizations", new[] { "Organization_Id" });
            DropIndex("dbo.Garbages", new[] { "Season_Id" });
            DropIndex("dbo.Garbages", new[] { "Kind_Id" });
            DropTable("dbo.GarbageKindOrganizations");
            DropTable("dbo.Garbages");
        }
    }
}
