using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    public class Departement
    {
        [Key]
        public int id { get; set; }
        public string label { get; set; }
        public virtual Responsable_departement Responsable_Departement { get; set; }
        public virtual ICollection<Reclamation> Reclamations { get; set; }
    }
}