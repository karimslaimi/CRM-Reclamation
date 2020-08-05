using PFE_reclamation.Models;
using System.Data.Entity;

namespace PFE_reclamation.DAL
{
    public class DatabContext : DbContext
    {

        public DatabContext() : base("name=Reclamations")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {




        }



        public DbSet<Admin> Admins { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Client> Clients { get; set; }

        public DbSet<Contrat> Contrats { get; set; }
        public DbSet<Departement> Departements { get; set; }
        public DbSet<DepAssocie> DepAssocies { get; set; }
       // public DbSet<Etat> Etats { get; set; }
        public DbSet<Reclamation> Reclamations { get; set; }
        public DbSet<Responsable_departement> Responsable_Departements { get; set; }
        public DbSet<Responsable_relation_client> Responsable_Relation_Clients { get; set; }
        public DbSet<Superviseur> Superviseurs { get; set; }
        public DbSet<Traite> Traites { get; set; }
        //public DbSet<Types> Types { get; set; }
        public DbSet<User> Users { get; set; }











    }
}
