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
        [Key, ForeignKey("Reclamation")]
        public int id { get; set; }
     
        public DateTime date { get; set; }
        public string detaille { get; set; }
        public Agent agent { get; set; }

   
        [Required]
        public virtual Reclamation Reclamation { get; set; }
    }
}