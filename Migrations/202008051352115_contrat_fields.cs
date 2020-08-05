namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contrat_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Traites", "date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Traites", "detaille", c => c.String());
            AddColumn("dbo.Contrats", "titre", c => c.String());
            AddColumn("dbo.Contrats", "deb_contrat", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contrats", "fin_contrat", c => c.DateTime(nullable: false));
            AddColumn("dbo.Contrats", "description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Contrats", "description");
            DropColumn("dbo.Contrats", "fin_contrat");
            DropColumn("dbo.Contrats", "deb_contrat");
            DropColumn("dbo.Contrats", "titre");
            DropColumn("dbo.Traites", "detaille");
            DropColumn("dbo.Traites", "date");
        }
    }
}
