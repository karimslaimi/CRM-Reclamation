using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models {
    public class Message {

        public int id { get; set; }

        public DateTime date { get; set; }
        public string content { get; set; }

        [Required]
        public User sentTo { get; set; }
        [Required]
        public User sentBy { get; set; }


        }
    }