namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedepassoc : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DepAssocies", "Departement_id", "dbo.Departements");
            DropForeignKey("dbo.DepAssocies", "Reclamation_id", "dbo.Reclamations");
            DropIndex("dbo.DepAssocies", new[] { "Departement_id" });
            DropIndex("dbo.DepAssocies", new[] { "Reclamation_id" });
            AddColumn("dbo.Reclamations", "Departement_id", c => c.Int());
            CreateIndex("dbo.Reclamations", "Departement_id");
            AddForeignKey("dbo.Reclamations", "Departement_id", "dbo.Departements", "id");
            DropTable("dbo.DepAssocies");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.DepAssocies",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Departement_id = c.Int(),
                        Reclamation_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            DropForeignKey("dbo.Reclamations", "Departement_id", "dbo.Departements");
            DropIndex("dbo.Reclamations", new[] { "Departement_id" });
            DropColumn("dbo.Reclamations", "Departement_id");
            CreateIndex("dbo.DepAssocies", "Reclamation_id");
            CreateIndex("dbo.DepAssocies", "Departement_id");
            AddForeignKey("dbo.DepAssocies", "Reclamation_id", "dbo.Reclamations", "id");
            AddForeignKey("dbo.DepAssocies", "Departement_id", "dbo.Departements", "id");
        }
    }
}
