using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    public class Reclamation
    {

        public int id { get; set; }

        public DateTime debut_reclam { get; set; }
        public DateTime fin_reclam { get; set; }
        
        public Etat etat{get;set;}

        public Types type { get; set; }

        public string description { get; set; }

        public virtual Departement Departement { get; set; }


    }
}