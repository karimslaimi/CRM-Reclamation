namespace PFE_reclamation.Migrations {
    using PFE_reclamation.Models;
    using PFE_reclamation.Services;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PFE_reclamation.DAL.DatabContext> {
        public Configuration() {
            AutomaticMigrationsEnabled = false;
            }

        protected override void Seed(PFE_reclamation.DAL.DatabContext context) {
      //      var deps = new List<Departement>

      //{

      //          new Departement { label = "Direction générale" },

      //          new Departement { label = "Direction ressource humaine" },

      //          new Departement { label = "Direction du patrimoine" },

      //          new Departement { label = "Direction de marketing" },

      //          new Departement { label = "Direction financière" },
                
      //          new Departement { label = "Direction sinistre automobile" },

      //          new Departement { label = "Département sinistre auto IDA" },

      //          new Departement { label = "Département sinistre auto Hors IDA" },

      //          new Departement { label = "Direction sinistre divers" },

      //          new Departement { label = "Direction sinistre agricole" },
                
      //          new Departement { label = "Direction du contentieux" },

      //          new Departement { label = "Direction informatique" },

      //          new Departement { label = "Direction de la comptabilité" },

      //          new Departement { label = "Direction technique" },

      //          new Departement { label = "Département technique auto" },

      //          new Departement { label = "Département technique divers" },

      //          new Departement { label = "Département technique agricole" },
               
      //          new Departement { label = "Direction de la Réassurance" },

      //          new Departement { label = "Direction de l'archive" },

      //          new Departement { label = "Direction de maladies" },

      //          new Departement { label = "Direction d'audite" },

      //          new Departement { label = "Direction commerciale" },

      //          new Departement { label = "Département des bureaux directs" },

      //          new Departement { label = "Département des agents généraux" },
            
      //          new Departement { label = "Direction de relations avec le citoyen" },
            
      //          new Departement { label = "Direction ressource humaine" },

      //          new Departement { label = "Direction du patrimoine" },

      //          new Departement { label = "Direction de marketing" },

      //          new Departement { label = "Direction financière" },
                
      //          new Departement { label = "Direction sinistre automobile" },

      //          new Departement { label = "Département sinistre auto IDA" },

      //          new Departement { label = "Département sinistre auto Hors IDA" },

      //          new Departement { label = "Direction sinistre divers" },

      //          new Departement { label = "Direction sinistre agricole" },
                
      //          new Departement { label = "Direction du contentieux" },

      //          new Departement { label = "Direction informatique" },

      //          new Departement { label = "Direction de la comptabilité" },

      //          new Departement { label = "Direction technique" },

      //          new Departement { label = "Département technique auto" },

      //          new Departement { label = "Département technique divers" },

      //          new Departement { label = "Département technique agricole" },
                
      //          new Departement { label = "Direction de la Réassurance" },

      //          new Departement { label = "Direction de l'archive" },

      //          new Departement { label = "Direction de maladies" },

      //          new Departement { label = "Direction d'audite" },

      //          new Departement { label = "Direction commerciale" },

      //          new Departement { label = "Département des bureaux directs" },

      //          new Departement { label = "Département des agents généraux" },

      //          new Departement { label = "Direction de relations avec le citoyen" }

      //      };

      //      deps.ForEach(s => context.Departements.Add(s));

      //      context.SaveChanges();





            }
        }
    }
