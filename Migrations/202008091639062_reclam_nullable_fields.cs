namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reclam_nullable_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reclamations", "DepartemendId", c => c.Int());
            AlterColumn("dbo.Reclamations", "fin_reclam", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reclamations", "fin_reclam", c => c.DateTime(nullable: false));
            DropColumn("dbo.Reclamations", "DepartemendId");
        }
    }
}
