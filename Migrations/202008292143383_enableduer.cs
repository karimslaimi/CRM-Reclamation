namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class enableduer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "enabled", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "enabled");
        }
    }
}
