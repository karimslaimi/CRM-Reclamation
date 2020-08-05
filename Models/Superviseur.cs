using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models
{
    [Table("Superviseur")]
    public class Superviseur :User
    {
        public DateTime date_aff { get; set; }
    }
}