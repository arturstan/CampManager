namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductExpend : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AmountBuy = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountExpend = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WorthBuy = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WorthExpend = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoicePosition_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InvoicePositions", t => t.InvoicePosition_Id)
                .Index(t => t.InvoicePosition_Id);
            
            CreateTable(
                "dbo.ProductExpends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Worth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoicePosition_Id = c.Int(),
                        ProductOutPosition_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InvoicePositions", t => t.InvoicePosition_Id)
                .ForeignKey("dbo.ProductOutPositions", t => t.ProductOutPosition_Id)
                .Index(t => t.InvoicePosition_Id)
                .Index(t => t.ProductOutPosition_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductExpends", "ProductOutPosition_Id", "dbo.ProductOutPositions");
            DropForeignKey("dbo.ProductExpends", "InvoicePosition_Id", "dbo.InvoicePositions");
            DropForeignKey("dbo.ProductAmounts", "InvoicePosition_Id", "dbo.InvoicePositions");
            DropIndex("dbo.ProductExpends", new[] { "ProductOutPosition_Id" });
            DropIndex("dbo.ProductExpends", new[] { "InvoicePosition_Id" });
            DropIndex("dbo.ProductAmounts", new[] { "InvoicePosition_Id" });
            DropTable("dbo.ProductExpends");
            DropTable("dbo.ProductAmounts");
        }
    }
}
