using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;

namespace PFE_reclamation.Services
{
    public class Authentication
    {

        private DatabContext db = new DatabContext();

     



        public User AuthUser(string username, string password)
        {
            User u=db.Users.Where(x => x.username == username && x.password == password).FirstOrDefault();
           

     

            return u; 

        }
    
    
    
    }
}