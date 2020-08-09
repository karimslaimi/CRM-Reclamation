namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reclam_nullable_fields_stupid : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Reclamations", name: "Departement_id", newName: "DepartementId");
            RenameIndex(table: "dbo.Reclamations", name: "IX_Departement_id", newName: "IX_DepartementId");
            DropColumn("dbo.Reclamations", "DepartemendId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reclamations", "DepartemendId", c => c.Int());
            RenameIndex(table: "dbo.Reclamations", name: "IX_DepartementId", newName: "IX_Departement_id");
            RenameColumn(table: "dbo.Reclamations", name: "DepartementId", newName: "Departement_id");
        }
    }
}
