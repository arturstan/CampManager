namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Camp_note : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Camps", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Camps", "Note");
        }
    }
}
