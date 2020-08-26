namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class requiredrelation : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Reclamations", new[] { "Client_id" });
            DropIndex("dbo.Contrats", new[] { "Client_id" });
            AlterColumn("dbo.Reclamations", "Client_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Contrats", "Client_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Reclamations", "Client_id");
            CreateIndex("dbo.Contrats", "Client_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Contrats", new[] { "Client_id" });
            DropIndex("dbo.Reclamations", new[] { "Client_id" });
            AlterColumn("dbo.Contrats", "Client_id", c => c.Int());
            AlterColumn("dbo.Reclamations", "Client_id", c => c.Int());
            CreateIndex("dbo.Contrats", "Client_id");
            CreateIndex("dbo.Reclamations", "Client_id");
        }
    }
}
