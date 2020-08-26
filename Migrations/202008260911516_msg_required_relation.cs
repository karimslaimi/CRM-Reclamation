namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class msg_required_relation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "sentTo_id", "dbo.Users");
            DropForeignKey("dbo.Messages", "sentBy_id", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "sentTo_id" });
            DropIndex("dbo.Messages", new[] { "sentBy_id" });
            AlterColumn("dbo.Messages", "sentTo_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Messages", "sentBy_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Messages", "sentTo_id");
            CreateIndex("dbo.Messages", "sentBy_id");
            AddForeignKey("dbo.Messages", "sentTo_id", "dbo.Users", "id", cascadeDelete: false);
            AddForeignKey("dbo.Messages", "sentBy_id", "dbo.Users", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "sentBy_id", "dbo.Users");
            DropForeignKey("dbo.Messages", "sentTo_id", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "sentBy_id" });
            DropIndex("dbo.Messages", new[] { "sentTo_id" });
            AlterColumn("dbo.Messages", "sentBy_id", c => c.Int());
            AlterColumn("dbo.Messages", "sentTo_id", c => c.Int());
            CreateIndex("dbo.Messages", "sentBy_id");
            CreateIndex("dbo.Messages", "sentTo_id");
            AddForeignKey("dbo.Messages", "sentBy_id", "dbo.Users", "id");
            AddForeignKey("dbo.Messages", "sentTo_id", "dbo.Users", "id");
        }
    }
}
