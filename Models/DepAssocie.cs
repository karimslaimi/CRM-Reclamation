using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    public class DepAssocie
    {
        [Key]
        public int id { get; set; }
        public virtual Reclamation Reclamation { get; set; }
        public virtual Departement Departement { get; set; }
    }
}