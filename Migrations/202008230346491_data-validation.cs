namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datavalidation : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Messages", name: "sentBy_id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Messages", name: "sentTo_id", newName: "sentBy_id");
            RenameColumn(table: "dbo.Messages", name: "__mig_tmp__0", newName: "sentTo_id");
            RenameIndex(table: "dbo.Messages", name: "IX_sentBy_id", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.Messages", name: "IX_sentTo_id", newName: "IX_sentBy_id");
            RenameIndex(table: "dbo.Messages", name: "__mig_tmp__0", newName: "IX_sentTo_id");
            AlterColumn("dbo.Users", "nom", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "prenom", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "cin", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.Users", "tel", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.Users", "mail", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Users", "username", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "password", c => c.String(nullable: false, maxLength: 254));
            AlterColumn("dbo.Departements", "label", c => c.String(nullable: false));
            AlterColumn("dbo.Reclamations", "titre", c => c.String(nullable: false));
            AlterColumn("dbo.Contrats", "titre", c => c.String(nullable: false));
            CreateIndex("dbo.Users", "cin", unique: true);
            CreateIndex("dbo.Users", "mail", unique: true);
         
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "username" });
            DropIndex("dbo.Users", new[] { "mail" });
            DropIndex("dbo.Users", new[] { "cin" });
            AlterColumn("dbo.Contrats", "titre", c => c.String());
            AlterColumn("dbo.Reclamations", "titre", c => c.String());
            AlterColumn("dbo.Departements", "label", c => c.String());
            AlterColumn("dbo.Users", "password", c => c.String());
            AlterColumn("dbo.Users", "username", c => c.String());
            AlterColumn("dbo.Users", "mail", c => c.String());
            AlterColumn("dbo.Users", "tel", c => c.String());
            AlterColumn("dbo.Users", "cin", c => c.String());
            AlterColumn("dbo.Users", "prenom", c => c.String());
            AlterColumn("dbo.Users", "nom", c => c.String());
            RenameIndex(table: "dbo.Messages", name: "IX_sentTo_id", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.Messages", name: "IX_sentBy_id", newName: "IX_sentTo_id");
            RenameIndex(table: "dbo.Messages", name: "__mig_tmp__0", newName: "IX_sentBy_id");
            RenameColumn(table: "dbo.Messages", name: "sentTo_id", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.Messages", name: "sentBy_id", newName: "sentTo_id");
            RenameColumn(table: "dbo.Messages", name: "__mig_tmp__0", newName: "sentBy_id");
        }
    }
}
