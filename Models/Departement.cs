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
    }
}