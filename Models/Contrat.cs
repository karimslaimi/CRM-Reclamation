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
        public string titre { get; set; }
        public DateTime deb_contrat { get; set; }
        public DateTime fin_contrat { get; set; }
        public string description { get; set; }
        public virtual Client Client { get; set; }
    }
}