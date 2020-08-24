namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class remotevalidation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reclamations", "description", c => c.String(nullable: false));
            AlterColumn("dbo.Contrats", "description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contrats", "description", c => c.String());
            AlterColumn("dbo.Reclamations", "description", c => c.String());
        }
    }
}
