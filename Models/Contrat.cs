using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    public class Contrat
    {
        [Key]
        public int id { get; set; }
        public virtual Client Client { get; set; }
    }
}