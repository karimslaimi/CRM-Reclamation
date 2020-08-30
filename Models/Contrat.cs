using System;
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
        [Required(ErrorMessage = "Le titre ne doit pas etre vide")]
        [MinLength(3, ErrorMessage = "doit etre plus long")]
        public string titre { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy 00:00:00}")]
        public DateTime deb_contrat { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy 00:00:00}")]
        public DateTime fin_contrat { get; set; }


        [Required(ErrorMessage = "La description ne doit pas etre vide")]
        [MinLength(3, ErrorMessage = "doit etre plus long")]
        public string description { get; set; }

        [Required]
        public virtual Client Client { get; set; }
    }
}