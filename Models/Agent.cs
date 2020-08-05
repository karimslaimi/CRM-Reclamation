using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{

    [Table("Agent")]
    public class Agent :User
    {
        public virtual Departement departement { get; set; }

        //collection of treated claims
        public ICollection<Traite> reclamTraite { get; set; }
        //date when his account created
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime date_aff { get; set; }
    }
}