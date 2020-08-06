using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFE_reclamation.Services
{
    public class UserService
    {
        private DatabContext db = new DatabContext();
        public User findusername(string username){

            return db.Users.Where(x => x.username == username).FirstOrDefault();
            
         }
    }
}