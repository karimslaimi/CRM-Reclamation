namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialmigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nom = c.String(),
                        prenom = c.String(),
                        cin = c.String(),
                        tel = c.String(),
                        mail = c.String(),
                        username = c.String(),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Departements",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        label = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Traites",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        reclamation_id = c.Int(),
                        traite_id = c.Int(),
                        Agent_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Reclamations", t => t.reclamation_id)
                .ForeignKey("dbo.Traites", t => t.traite_id)
                .ForeignKey("dbo.Agent", t => t.Agent_id)
                .Index(t => t.reclamation_id)
                .Index(t => t.traite_id)
                .Index(t => t.Agent_id);
            
            CreateTable(
                "dbo.Reclamations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        debut_reclam = c.DateTime(nullable: false),
                        fin_reclam = c.DateTime(nullable: false),
                        etat = c.Int(nullable: false),
                        type = c.Int(nullable: false),
                        description = c.String(),
                        Client_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Client", t => t.Client_id)
                .Index(t => t.Client_id);
            
            CreateTable(
                "dbo.Contrats",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Client_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Client", t => t.Client_id)
                .Index(t => t.Client_id);
            
            CreateTable(
                "dbo.DepAssocies",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Departement_id = c.Int(),
                        Reclamation_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Departements", t => t.Departement_id)
                .ForeignKey("dbo.Reclamations", t => t.Reclamation_id)
                .Index(t => t.Departement_id)
                .Index(t => t.Reclamation_id);
            
            CreateTable(
                "dbo.Admin",
                c => new
                    {
                        id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.Agent",
                c => new
                    {
                        id = c.Int(nullable: false),
                        departement_id = c.Int(),
                        date_aff = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.id)
                .ForeignKey("dbo.Departements", t => t.departement_id)
                .Index(t => t.id)
                .Index(t => t.departement_id);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.Responsable_departement",
                c => new
                    {
                        id = c.Int(nullable: false),
                        departement_id = c.Int(),
                        date_aff = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.id)
                .ForeignKey("dbo.Departements", t => t.departement_id)
                .Index(t => t.id)
                .Index(t => t.departement_id);
            
            CreateTable(
                "dbo.Responsable_relation_client",
                c => new
                    {
                        id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.Superviseur",
                c => new
                    {
                        id = c.Int(nullable: false),
                        date_aff = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.id)
                .Index(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Superviseur", "id", "dbo.Users");
            DropForeignKey("dbo.Responsable_relation_client", "id", "dbo.Users");
            DropForeignKey("dbo.Responsable_departement", "departement_id", "dbo.Departements");
            DropForeignKey("dbo.Responsable_departement", "id", "dbo.Users");
            DropForeignKey("dbo.Client", "id", "dbo.Users");
            DropForeignKey("dbo.Agent", "departement_id", "dbo.Departements");
            DropForeignKey("dbo.Agent", "id", "dbo.Users");
            DropForeignKey("dbo.Admin", "id", "dbo.Users");
            DropForeignKey("dbo.DepAssocies", "Reclamation_id", "dbo.Reclamations");
            DropForeignKey("dbo.DepAssocies", "Departement_id", "dbo.Departements");
            DropForeignKey("dbo.Reclamations", "Client_id", "dbo.Client");
            DropForeignKey("dbo.Contrats", "Client_id", "dbo.Client");
            DropForeignKey("dbo.Traites", "Agent_id", "dbo.Agent");
            DropForeignKey("dbo.Traites", "traite_id", "dbo.Traites");
            DropForeignKey("dbo.Traites", "reclamation_id", "dbo.Reclamations");
            DropIndex("dbo.Superviseur", new[] { "id" });
            DropIndex("dbo.Responsable_relation_client", new[] { "id" });
            DropIndex("dbo.Responsable_departement", new[] { "departement_id" });
            DropIndex("dbo.Responsable_departement", new[] { "id" });
            DropIndex("dbo.Client", new[] { "id" });
            DropIndex("dbo.Agent", new[] { "departement_id" });
            DropIndex("dbo.Agent", new[] { "id" });
            DropIndex("dbo.Admin", new[] { "id" });
            DropIndex("dbo.DepAssocies", new[] { "Reclamation_id" });
            DropIndex("dbo.DepAssocies", new[] { "Departement_id" });
            DropIndex("dbo.Contrats", new[] { "Client_id" });
            DropIndex("dbo.Reclamations", new[] { "Client_id" });
            DropIndex("dbo.Traites", new[] { "Agent_id" });
            DropIndex("dbo.Traites", new[] { "traite_id" });
            DropIndex("dbo.Traites", new[] { "reclamation_id" });
            DropTable("dbo.Superviseur");
            DropTable("dbo.Responsable_relation_client");
            DropTable("dbo.Responsable_departement");
            DropTable("dbo.Client");
            DropTable("dbo.Agent");
            DropTable("dbo.Admin");
            DropTable("dbo.DepAssocies");
            DropTable("dbo.Contrats");
            DropTable("dbo.Reclamations");
            DropTable("dbo.Traites");
            DropTable("dbo.Departements");
            DropTable("dbo.Users");
        }
    }
}
