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
        public string label { get; set; }

        [ForeignKey("responsable")]
        public int responsableId { get; set; }


        public virtual Responsable_departement responsable { get; set; }

        public virtual ICollection<Reclamation> Reclamations { get; set; }
        }
    }