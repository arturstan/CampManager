namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Menu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Breakfast = c.String(),
                        Dinner = c.String(),
                        Supper = c.String(),
                        Season_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SeasonOrganizations", t => t.Season_Id)
                .Index(t => t.Season_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Menus", "Season_Id", "dbo.SeasonOrganizations");
            DropIndex("dbo.Menus", new[] { "Season_Id" });
            DropTable("dbo.Menus");
        }
    }
}
