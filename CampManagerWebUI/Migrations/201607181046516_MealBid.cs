namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MealBid : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MealBids",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Bid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Season_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SeasonOrganizations", t => t.Season_Id)
                .Index(t => t.Season_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MealBids", "Season_Id", "dbo.SeasonOrganizations");
            DropIndex("dbo.MealBids", new[] { "Season_Id" });
            DropTable("dbo.MealBids");
        }
    }
}
