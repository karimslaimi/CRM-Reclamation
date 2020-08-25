namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascademig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reclamations", "Client_id", "dbo.Client");
            AddColumn("dbo.Reclamations", "Client_id1", c => c.Int());
            CreateIndex("dbo.Reclamations", "Client_id1");
            AddForeignKey("dbo.Reclamations", "Client_id1", "dbo.Client", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reclamations", "Client_id1", "dbo.Client");
            DropIndex("dbo.Reclamations", new[] { "Client_id1" });
            DropColumn("dbo.Reclamations", "Client_id1");
            AddForeignKey("dbo.Reclamations", "Client_id", "dbo.Client", "id");
        }
    }
}
