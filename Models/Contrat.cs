﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    public class Contrat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        [Required(ErrorMessage ="Le titre ne doit pas etre vide")]
        public string titre { get; set; }
        public DateTime deb_contrat { get; set; }
        public DateTime fin_contrat { get; set; }
        public string description { get; set; }
        public virtual Client Client { get; set; }
    }
}