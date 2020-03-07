namespace SlackService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UChannels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        dateTime = c.DateTime(nullable: false),
                        ChannelName = c.String(nullable: false),
                        purpose = c.String(nullable: false),
                        isPublic = c.Boolean(nullable: false),
                        group_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UGroups", t => t.group_Id, cascadeDelete: true)
                .Index(t => t.group_Id);
            
            CreateTable(
                "dbo.UGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        dateTime = c.DateTime(nullable: false),
                        GroupName = c.String(nullable: false),
                        creationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MesssageDecription = c.String(nullable: false),
                        dateTime = c.DateTime(nullable: false),
                        channel_Id = c.Int(nullable: false),
                        user_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UChannels", t => t.channel_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_Id, cascadeDelete: true)
                .Index(t => t.channel_Id)
                .Index(t => t.user_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UMessages", "user_Id", "dbo.Users");
            DropForeignKey("dbo.UMessages", "channel_Id", "dbo.UChannels");
            DropForeignKey("dbo.UChannels", "group_Id", "dbo.UGroups");
            DropIndex("dbo.UMessages", new[] { "user_Id" });
            DropIndex("dbo.UMessages", new[] { "channel_Id" });
            DropIndex("dbo.UChannels", new[] { "group_Id" });
            DropTable("dbo.UMessages");
            DropTable("dbo.UGroups");
            DropTable("dbo.UChannels");
        }
    }
}
