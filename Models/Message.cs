using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Models {
    public class Message {

        public int id { get; set; }

        public DateTime date { get; set; }
        public string content { get; set; }

        public User sentTo { get; set; }
        public User sentBy { get; set; }


        }
    }