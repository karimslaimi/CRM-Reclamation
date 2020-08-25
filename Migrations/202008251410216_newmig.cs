namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "photo", c => c.String());
            AlterColumn("dbo.Users", "nom", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "prenom", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "cin", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.Users", "tel", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.Users", "mail", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Users", "username", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "password", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Departements", "label", c => c.String(nullable: false));
            AlterColumn("dbo.Reclamations", "titre", c => c.String(nullable: false));
            AlterColumn("dbo.Reclamations", "description", c => c.String(nullable: false));
            AlterColumn("dbo.Contrats", "titre", c => c.String(nullable: false));
            AlterColumn("dbo.Contrats", "description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contrats", "description", c => c.String());
            AlterColumn("dbo.Contrats", "titre", c => c.String());
            AlterColumn("dbo.Reclamations", "description", c => c.String());
            AlterColumn("dbo.Reclamations", "titre", c => c.String());
            AlterColumn("dbo.Departements", "label", c => c.String());
            AlterColumn("dbo.Users", "password", c => c.String());
            AlterColumn("dbo.Users", "username", c => c.String());
            AlterColumn("dbo.Users", "mail", c => c.String());
            AlterColumn("dbo.Users", "tel", c => c.String());
            AlterColumn("dbo.Users", "cin", c => c.String());
            AlterColumn("dbo.Users", "prenom", c => c.String());
            AlterColumn("dbo.Users", "nom", c => c.String());
            DropColumn("dbo.Users", "photo");
        }
    }
}
