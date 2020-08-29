namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dateformat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Contrats", "deb_contrat", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Contrats", "fin_contrat", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Contrats", "fin_contrat", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Contrats", "deb_contrat", c => c.DateTime(nullable: false));
        }
    }
}
