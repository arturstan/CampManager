namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampMeal_reside : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampMeals", "Reside", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampMeals", "Reside");
        }
    }
}
