namespace IdentityWithExistingDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentityRoleUpdateUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityRoleUsers",
                c => new
                    {
                        IdentityRole_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdentityRole_Id, t.User_Id })
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityRoleUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.IdentityRoleUsers", "IdentityRole_Id", "dbo.IdentityRoles");
            DropIndex("dbo.IdentityRoleUsers", new[] { "User_Id" });
            DropIndex("dbo.IdentityRoleUsers", new[] { "IdentityRole_Id" });
            DropColumn("dbo.Users", "UserName");
            DropTable("dbo.IdentityRoleUsers");
            DropTable("dbo.IdentityRoles");
        }
    }
}
