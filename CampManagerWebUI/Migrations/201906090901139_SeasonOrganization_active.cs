namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeasonOrganization_active : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SeasonOrganizations", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SeasonOrganizations", "Active");
        }
    }
}
