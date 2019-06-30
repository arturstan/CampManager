namespace CampManagerWebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTables_UserOrganization_UserEmailAllow : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserEmailAllows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        StartRoles = c.String(),
                        DateExpire = c.DateTime(),
                        Organization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: true)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.UserOrganizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdUser = c.String(),
                        Active = c.Boolean(nullable: false),
                        DateExpire = c.DateTime(),
                        Roles = c.String(),
                        Organization_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserOrganizations", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.UserEmailAllows", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.UserOrganizations", new[] { "Organization_Id" });
            DropIndex("dbo.UserEmailAllows", new[] { "Organization_Id" });
            DropTable("dbo.UserOrganizations");
            DropTable("dbo.UserEmailAllows");
        }
    }
}
