namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mtt : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Responsable_departement", new[] { "departementId" });
            CreateIndex("dbo.Responsable_departement", "departementId", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Responsable_departement", new[] { "departementId" });
            CreateIndex("dbo.Responsable_departement", "departementId");
        }
    }
}
