namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFollowing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Followings",
                c => new
                    {
                        ArtistId = c.String(nullable: false, maxLength: 128),
                        FollowerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ArtistId, t.FollowerId })
                .ForeignKey("dbo.AspNetUsers", t => t.ArtistId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerId, cascadeDelete: true)
                .Index(t => t.ArtistId)
                .Index(t => t.FollowerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Followings", "FollowerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Followings", "ArtistId", "dbo.AspNetUsers");
            DropIndex("dbo.Followings", new[] { "FollowerId" });
            DropIndex("dbo.Followings", new[] { "ArtistId" });
            DropTable("dbo.Followings");
        }
    }
}
