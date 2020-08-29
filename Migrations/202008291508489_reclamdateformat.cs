namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reclamdateformat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reclamations", "debut_reclam", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Reclamations", "fin_reclam", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reclamations", "fin_reclam", c => c.DateTime());
            AlterColumn("dbo.Reclamations", "debut_reclam", c => c.DateTime(nullable: false));
        }
    }
}
