namespace PFE_reclamation.Migrations
{
    using PFE_reclamation.Models;
    using PFE_reclamation.Services;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PFE_reclamation.DAL.DatabContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(PFE_reclamation.DAL.DatabContext context)
        {
            //admin and some users
          
            
            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            }
    }
}
