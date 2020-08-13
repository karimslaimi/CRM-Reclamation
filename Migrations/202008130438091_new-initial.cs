namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newinitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false),
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
                        ResponsableId = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Reclamations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        titre = c.String(),
                        debut_reclam = c.DateTime(nullable: false),
                        fin_reclam = c.DateTime(),
                        etat = c.Int(nullable: false),
                        type = c.Int(nullable: false),
                        description = c.String(),
                        DepartementId = c.Int(),
                        Client_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Client", t => t.Client_id)
                .ForeignKey("dbo.Departements", t => t.DepartementId)
                .Index(t => t.DepartementId)
                .Index(t => t.Client_id);
            
            CreateTable(
                "dbo.Contrats",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        titre = c.String(),
                        deb_contrat = c.DateTime(nullable: false),
                        fin_contrat = c.DateTime(nullable: false),
                        description = c.String(),
                        Client_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Client", t => t.Client_id)
                .Index(t => t.Client_id);
            
            CreateTable(
                "dbo.Traites",
                c => new
                    {
                        id = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        detaille = c.String(),
                        agent_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Agent", t => t.agent_id)
                .ForeignKey("dbo.Reclamations", t => t.id)
                .Index(t => t.id)
                .Index(t => t.agent_id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false),
                        content = c.String(),
                        sentBy_id = c.Int(),
                        sentTo_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.sentBy_id)
                .ForeignKey("dbo.Users", t => t.sentTo_id)
                .Index(t => t.sentBy_id)
                .Index(t => t.sentTo_id);
            
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
                        date_aff = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.id)
                .ForeignKey("dbo.Departements", t => t.id)
                .Index(t => t.id);
            
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
            DropForeignKey("dbo.Responsable_departement", "id", "dbo.Departements");
            DropForeignKey("dbo.Responsable_departement", "id", "dbo.Users");
            DropForeignKey("dbo.Client", "id", "dbo.Users");
            DropForeignKey("dbo.Agent", "departement_id", "dbo.Departements");
            DropForeignKey("dbo.Agent", "id", "dbo.Users");
            DropForeignKey("dbo.Admin", "id", "dbo.Users");
            DropForeignKey("dbo.Messages", "sentTo_id", "dbo.Users");
            DropForeignKey("dbo.Messages", "sentBy_id", "dbo.Users");
            DropForeignKey("dbo.Traites", "id", "dbo.Reclamations");
            DropForeignKey("dbo.Traites", "agent_id", "dbo.Agent");
            DropForeignKey("dbo.Reclamations", "DepartementId", "dbo.Departements");
            DropForeignKey("dbo.Reclamations", "Client_id", "dbo.Client");
            DropForeignKey("dbo.Contrats", "Client_id", "dbo.Client");
            DropIndex("dbo.Superviseur", new[] { "id" });
            DropIndex("dbo.Responsable_relation_client", new[] { "id" });
            DropIndex("dbo.Responsable_departement", new[] { "id" });
            DropIndex("dbo.Client", new[] { "id" });
            DropIndex("dbo.Agent", new[] { "departement_id" });
            DropIndex("dbo.Agent", new[] { "id" });
            DropIndex("dbo.Admin", new[] { "id" });
            DropIndex("dbo.Messages", new[] { "sentTo_id" });
            DropIndex("dbo.Messages", new[] { "sentBy_id" });
            DropIndex("dbo.Traites", new[] { "agent_id" });
            DropIndex("dbo.Traites", new[] { "id" });
            DropIndex("dbo.Contrats", new[] { "Client_id" });
            DropIndex("dbo.Reclamations", new[] { "Client_id" });
            DropIndex("dbo.Reclamations", new[] { "DepartementId" });
            DropTable("dbo.Superviseur");
            DropTable("dbo.Responsable_relation_client");
            DropTable("dbo.Responsable_departement");
            DropTable("dbo.Client");
            DropTable("dbo.Agent");
            DropTable("dbo.Admin");
            DropTable("dbo.Messages");
            DropTable("dbo.Traites");
            DropTable("dbo.Contrats");
            DropTable("dbo.Reclamations");
            DropTable("dbo.Departements");
            DropTable("dbo.Users");
        }
    }
}
