namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoicePosition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Camps", "DateStart", c => c.DateTime(nullable: false));
            AddColumn("dbo.Camps", "DateEnd", c => c.DateTime(nullable: false));
            AddColumn("dbo.Camps", "PersonCount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Camps", "PricePerPerson", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.InvoicePositions", "Worth", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoicePositions", "Worth");
            DropColumn("dbo.Camps", "PricePerPerson");
            DropColumn("dbo.Camps", "PersonCount");
            DropColumn("dbo.Camps", "DateEnd");
            DropColumn("dbo.Camps", "DateStart");
        }
    }
}
