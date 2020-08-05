using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    public class Traite
    {
        [Key]
        public int id { get; set; }
        public Traite traite { get; set; }
        public Reclamation reclamation { get; set; }
    }
}