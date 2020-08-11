using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    public class Traite
    {
        [ForeignKey("Reclamation")]
        public int id { get; set; }
        public DateTime date { get; set; }
        public string detaille { get; set; }
        public Agent agent { get; set; }

   
        public Reclamation Reclamation { get; set; }
    }
}