namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MealBid_expend_people : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MealBids", "Expend", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.MealBids", "PeopleCount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MealBids", "PeopleCount");
            DropColumn("dbo.MealBids", "Expend");
        }
    }
}
