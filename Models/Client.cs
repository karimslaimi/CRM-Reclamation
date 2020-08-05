using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    [Table("Client")]
    public class Client : User
    {
        public virtual ICollection<Contrat> Contrats { get; set; }
        public virtual ICollection<Reclamation> Reclamations { get; set; }
    }
}