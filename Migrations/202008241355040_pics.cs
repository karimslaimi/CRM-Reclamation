namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pics : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "photo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "photo");
        }
    }
}
