namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validationmig : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "cin" });
            DropIndex("dbo.Users", new[] { "mail" });
            DropIndex("dbo.Users", new[] { "username" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Users", "username", unique: true);
            CreateIndex("dbo.Users", "mail", unique: true);
            CreateIndex("dbo.Users", "cin", unique: true);
        }
    }
}
