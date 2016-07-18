namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixNotificationColumnName : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Notifications", new[] { "gig_Id" });
            CreateIndex("dbo.Notifications", "Gig_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Notifications", new[] { "Gig_Id" });
            CreateIndex("dbo.Notifications", "gig_Id");
        }
    }
}
