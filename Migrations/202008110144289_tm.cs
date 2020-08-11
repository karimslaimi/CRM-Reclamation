namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tm : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Traites", new[] { "reclamation_id" });
           
       

            DropPrimaryKey("dbo.Traites");
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false),
                        content = c.String(),
                        sentBy_id = c.Int(),
                        sentTo_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.sentBy_id)
                .ForeignKey("dbo.Users", t => t.sentTo_id)
                .Index(t => t.sentBy_id)
                .Index(t => t.sentTo_id);
            
            AlterColumn("dbo.Traites", "id", c => c.Int(nullable: false));
            AlterColumn("dbo.Traites", "id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Traites", "id");
            CreateIndex("dbo.Traites", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "sentTo_id", "dbo.Users");
            DropForeignKey("dbo.Messages", "sentBy_id", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "sentTo_id" });
            DropIndex("dbo.Messages", new[] { "sentBy_id" });
            DropIndex("dbo.Traites", new[] { "id" });
            DropPrimaryKey("dbo.Traites");
            AlterColumn("dbo.Traites", "id", c => c.Int());
            AlterColumn("dbo.Traites", "id", c => c.Int(nullable: false, identity: true));
            DropTable("dbo.Messages");
            AddPrimaryKey("dbo.Traites", "id");
            RenameColumn(table: "dbo.Traites", name: "id", newName: "reclamation_id");
            AddColumn("dbo.Traites", "id", c => c.Int(nullable: false, identity: true));
            CreateIndex("dbo.Traites", "reclamation_id");
        }
    }
}
