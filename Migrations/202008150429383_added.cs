namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Agent", "departement_id", "dbo.Departements");
            DropIndex("dbo.Agent", new[] { "departement_id" });
            RenameColumn(table: "dbo.Agent", name: "departement_id", newName: "departementId");
            AlterColumn("dbo.Agent", "departementId", c => c.Int(nullable: false));
            CreateIndex("dbo.Agent", "departementId");
            AddForeignKey("dbo.Agent", "departementId", "dbo.Departements", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Agent", "departementId", "dbo.Departements");
            DropIndex("dbo.Agent", new[] { "departementId" });
            AlterColumn("dbo.Agent", "departementId", c => c.Int());
            RenameColumn(table: "dbo.Agent", name: "departementId", newName: "departement_id");
            CreateIndex("dbo.Agent", "departement_id");
            AddForeignKey("dbo.Agent", "departement_id", "dbo.Departements", "id");
        }
    }
}
