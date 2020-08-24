using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Services;

namespace PFE_reclamation.Controllers
{
    public class UsersController : Controller
    {
        private DatabContext db = new DatabContext();

        private Authentication authservice = new Authentication();


        [HttpGet]
        public ActionResult Signin()
        {
            if (User.Identity.IsAuthenticated)
                {
                return RedirectToAction("logout");

                }
           

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signin(User user)
        {
              User u = authservice.AuthUser(user.username, user.password);
            if (u != null)
            {
                string role="";
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
                    claims.Add(new Claim(ClaimTypes.Name, u.username));
                   
                    claims.Add(new Claim("name", u.nom + " " + u.prenom));
                    claims.Add(new Claim("email", u.mail));
                    claims.Add(new Claim("id", u.id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Role, role));


                    var claimIdenties = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                    var ctx = HttpContext.GetOwinContext();
                    var authenticationManager = ctx.Authentication;
                    // Sign In.


                    authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(new ClaimsPrincipal(claimIdenties), new AuthenticationProperties() { IsPersistent = true, ExpiresUtc = DateTime.UtcNow.AddDays(7) });
                    return redirectrole(role);

                }
                catch (Exception ex)
                {
                    // Info
                    throw ex;
                }

              
            }  else
                {
                    ViewBag.error= "Authentification invalide";
                    return View();
                }

            
           
        }


        public ActionResult redirectrole(string role)
        {/*
            
            */
            if (role == "" || role==null)
            {
            IEnumerable<Claim> claims = ClaimsPrincipal.Current.Claims;
             role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            }
            switch (role)
            {
                case "ADMIN": return RedirectToAction("index", "Admin");
                case "CLIENT": return RedirectToAction("index", "Client");
                case "AGENT": return RedirectToAction("index","Agent"); 
               case "RD": return RedirectToAction("index","RD");
                case "RRC": return RedirectToAction("index","RRC");
                case "SUPERVISEUR": return RedirectToAction("index","Superviseur");
                default: return RedirectToAction("Signin", "Users");
            }
        }



        public ActionResult Logout()
        {
            IOwinContext context = HttpContext.GetOwinContext();
            IAuthenticationManager authenticationManager = context.Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
           

            return Redirect("signin");
        }




        public JsonResult IsUsernameExists(string UserName,int ? id) {
            if (id != 0) {
                return Json(!db.Users.Any(x => x.username == UserName && x.id!=id), JsonRequestBehavior.AllowGet);
                } else {
        return Json(!db.Users.Any(x => x.username == UserName), JsonRequestBehavior.AllowGet);
                }
    
            }

        public JsonResult IsmailExists(string mail, int? id) {

            if (id != 0) {
                return Json(!db.Users.Any(x => x.mail == mail && x.id != id), JsonRequestBehavior.AllowGet);
                } else {
                return Json(!db.Users.Any(x => x.mail == mail), JsonRequestBehavior.AllowGet);
                }
            }
        public JsonResult IscinExists(string cin, int? id) {

            if (id != 0) {
                return Json(!db.Users.Any(x => x.cin == cin && x.id != id), JsonRequestBehavior.AllowGet);
                } else {
                return Json(!db.Users.Any(x => x.cin == cin), JsonRequestBehavior.AllowGet);
                }
            }





        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
