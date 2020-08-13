namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class msssss : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Responsable_departement", "id", "dbo.Departements");
            AddColumn("dbo.Responsable_departement", "departementId", c => c.Int(nullable: false));
            CreateIndex("dbo.Responsable_departement", "departementId");
            AddForeignKey("dbo.Responsable_departement", "departementId", "dbo.Departements", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Responsable_departement", "departementId", "dbo.Departements");
            DropIndex("dbo.Responsable_departement", new[] { "departementId" });
            DropColumn("dbo.Responsable_departement", "departementId");
            AddForeignKey("dbo.Responsable_departement", "id", "dbo.Departements", "id", cascadeDelete: true);
        }
    }
}
