namespace PFE_reclamation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idnull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "sentBy_id", "dbo.Users");
            DropForeignKey("dbo.Messages", "sentTo_id", "dbo.Users");
            DropForeignKey("dbo.Admin", "id", "dbo.Users");
            DropForeignKey("dbo.Agent", "id", "dbo.Users");
            DropForeignKey("dbo.Client", "id", "dbo.Users");
            DropForeignKey("dbo.Responsable_departement", "id", "dbo.Users");
            DropForeignKey("dbo.Responsable_relation_client", "id", "dbo.Users");
            DropForeignKey("dbo.Superviseur", "id", "dbo.Users");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Users", "id");
            AddForeignKey("dbo.Messages", "sentBy_id", "dbo.Users", "id");
            AddForeignKey("dbo.Messages", "sentTo_id", "dbo.Users", "id");
            AddForeignKey("dbo.Admin", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Agent", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Client", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Responsable_departement", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Responsable_relation_client", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Superviseur", "id", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Superviseur", "id", "dbo.Users");
            DropForeignKey("dbo.Responsable_relation_client", "id", "dbo.Users");
            DropForeignKey("dbo.Responsable_departement", "id", "dbo.Users");
            DropForeignKey("dbo.Client", "id", "dbo.Users");
            DropForeignKey("dbo.Agent", "id", "dbo.Users");
            DropForeignKey("dbo.Admin", "id", "dbo.Users");
            DropForeignKey("dbo.Messages", "sentTo_id", "dbo.Users");
            DropForeignKey("dbo.Messages", "sentBy_id", "dbo.Users");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Users", "id");
            AddForeignKey("dbo.Superviseur", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Responsable_relation_client", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Responsable_departement", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Client", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Agent", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Admin", "id", "dbo.Users", "id");
            AddForeignKey("dbo.Messages", "sentTo_id", "dbo.Users", "id");
            AddForeignKey("dbo.Messages", "sentBy_id", "dbo.Users", "id");
        }
    }
}
