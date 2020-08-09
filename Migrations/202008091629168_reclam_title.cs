namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reclam_title : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reclamations", "titre", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reclamations", "titre");
        }
    }
}
