using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    [Table("Responsable_departement")]
    public class Responsable_departement :User
    {
        public virtual Departement departement { get; set; }
        [DataType(DataType.Date)]
        public DateTime date_aff { get; set; }
    }
}