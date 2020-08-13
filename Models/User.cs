using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string cin { get; set; }
        public string tel { get; set; }
        public string mail { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}