namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductOrganization_Active : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductOrganizations", "Active", c => c.Boolean(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductOrganizations", "Active");
        }
    }
}
