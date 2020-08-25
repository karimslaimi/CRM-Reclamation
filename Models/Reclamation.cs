using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    public class Reclamation
    {

        public int id { get; set; }

        [RegularExpression(@"^[0-9A-Za-z ]+$", ErrorMessage = "Les caractères ne sont pas autorisés.")]
        [Required(ErrorMessage = "Le titre ne doit pas etre vide")]
        [MinLength(3, ErrorMessage = "doit etre plus long")]
        public string titre { get; set; }

        public DateTime debut_reclam { get; set; }
        public DateTime? fin_reclam { get; set; }
        
        public Etat etat{get;set;}

        public Types type { get; set; }
        [Required(ErrorMessage = "La description ne doit pas etre vide")]
        [MinLength(3, ErrorMessage = "doit etre plus long")]
        public string description { get; set; }

        public int? DepartementId { get; set; }    
        public virtual Departement Departement { get; set; }


        public virtual Client Client { get; set; }

  
        public virtual Traite Traite { get; set; }

        }
}