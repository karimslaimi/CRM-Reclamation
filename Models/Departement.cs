using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models {
    public class Departement {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; } 
        [Required(ErrorMessage ="le nom de département ne peut pas etre vide")]
        public string label { get; set; }


        public virtual ICollection<Reclamation> Reclamations { get; set; }
        public virtual ICollection<Agent> Agents { get; set; }
        }
    }