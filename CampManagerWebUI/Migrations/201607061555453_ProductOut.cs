namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductOut : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductOuts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Season_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SeasonOrganizations", t => t.Season_Id)
                .Index(t => t.Season_Id);
            
            CreateTable(
                "dbo.ProductOutPositions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Worth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Product_Id = c.Int(nullable: false),
                        ProductOut_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductOrganizations", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.ProductOuts", t => t.ProductOut_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.ProductOut_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductOuts", "Season_Id", "dbo.SeasonOrganizations");
            DropForeignKey("dbo.ProductOutPositions", "ProductOut_Id", "dbo.ProductOuts");
            DropForeignKey("dbo.ProductOutPositions", "Product_Id", "dbo.ProductOrganizations");
            DropIndex("dbo.ProductOutPositions", new[] { "ProductOut_Id" });
            DropIndex("dbo.ProductOutPositions", new[] { "Product_Id" });
            DropIndex("dbo.ProductOuts", new[] { "Season_Id" });
            DropTable("dbo.ProductOutPositions");
            DropTable("dbo.ProductOuts");
        }
    }
}
