namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migsami : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Traites", "traite_id", "dbo.Traites");
            DropIndex("dbo.Traites", new[] { "traite_id" });
            DropIndex("dbo.Traites", new[] { "Agent_id" });
            CreateIndex("dbo.Traites", "agent_id");
            DropColumn("dbo.Traites", "traite_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Traites", "traite_id", c => c.Int());
            DropIndex("dbo.Traites", new[] { "agent_id" });
            CreateIndex("dbo.Traites", "Agent_id");
            CreateIndex("dbo.Traites", "traite_id");
            AddForeignKey("dbo.Traites", "traite_id", "dbo.Traites", "id");
        }
    }
}
