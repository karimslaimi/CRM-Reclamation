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

        private IOwinContext context=new OwinContext();


        public User AuthUser(string username, string password)
        {
            User u=db.Users.Where(x => x.username == username && x.password == password).FirstOrDefault();
            string role="";

            if (u != null)
            {

                if (u is Admin) role = "ADMIN";


                else if (u is Client) role = "CLIENT";
                else if (u is Agent) role = "AGENT";
                else if (u is Responsable_departement) role = "RD";
                else if (u is Responsable_relation_client) role = "RRC";
                else if (u is Superviseur) role = "SUPERVISEUR";




                var claims = new List<Claim>();

                try
                {
                    // Setting
                    claims.Add(new Claim(ClaimTypes.Name, username));
                    claims.Add(new Claim("username", u.username));
                    claims.Add(new Claim("name", u.nom + " " + u.prenom));
                    claims.Add(new Claim("email", u.mail));
                    claims.Add(new Claim("id", u.id.ToString()));
                    claims.Add(new Claim("role", role));


                    var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                    var ctx = HttpContext.Current.GetOwinContext();
                    var authenticationManager = ctx.Authentication;
                    // Sign In.
               
                  
                    authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(claimIdenties), new AuthenticationProperties() { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddDays(7) });
                }
                catch (Exception ex)
                {
                    // Info
                    throw ex;
                }
                 
            }

            return u; 

        }
    
    
    
    }
}