namespace IdentityWithExistingDatabase.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Milestones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 500),
                        Finished = c.Boolean(nullable: false),
                        TaskId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: true)
                .Index(t => t.TaskId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 255),
                        Email = c.String(nullable: false, maxLength: 255),
                        Birthday = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true);
            
            CreateTable(
                "dbo.UserTasks",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Task_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Task_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.Task_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Task_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Milestones", "TaskId", "dbo.Tasks");
            DropForeignKey("dbo.UserTasks", "Task_Id", "dbo.Tasks");
            DropForeignKey("dbo.UserTasks", "User_Id", "dbo.Users");
            DropIndex("dbo.UserTasks", new[] { "Task_Id" });
            DropIndex("dbo.UserTasks", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Milestones", new[] { "TaskId" });
            DropTable("dbo.UserTasks");
            DropTable("dbo.Users");
            DropTable("dbo.Tasks");
            DropTable("dbo.Milestones");
        }
    }
}
