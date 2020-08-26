namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class traitre_reclam_relation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Traites", "id", "dbo.Reclamations");
            AddForeignKey("dbo.Traites", "id", "dbo.Reclamations", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Traites", "id", "dbo.Reclamations");
            AddForeignKey("dbo.Traites", "id", "dbo.Reclamations", "id");
        }
    }
}
