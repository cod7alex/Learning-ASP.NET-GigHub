namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedGigIdtoNotificationmodel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Notifications", name: "Gig_Id", newName: "GigId");
            RenameIndex(table: "dbo.Notifications", name: "IX_Gig_Id", newName: "IX_GigId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Notifications", name: "IX_GigId", newName: "IX_Gig_Id");
            RenameColumn(table: "dbo.Notifications", name: "GigId", newName: "Gig_Id");
        }
    }
}
