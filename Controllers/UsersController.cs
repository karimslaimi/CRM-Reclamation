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
using RestSharp;

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
            if (TempData["error"] != null) {
                ViewBag.passerr = TempData["error"];
                }


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signin(User user)
        {
              User u = authservice.AuthUser(user.username, user.password);
            if (u != null && u.enabled==true)
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




        public ActionResult forgotpasswor(string email) {

            User _user = db.Users.Where(x => x.mail == email).FirstOrDefault();
            if(_user != null) {
                //send verification sms
                //and store the code in cookies must be encrypted

                const string chars = "0123456789";
                Random rand = new Random();
                string key = new string(Enumerable.Repeat(chars, 6).Select(s => s[rand.Next(10)]).ToArray());

                var cookie = new System.Web.HttpCookie("cookie");
                cookie["key"] = authservice.Encrypt(key);
                Response.Cookies.Add(cookie);

                ApiService apiService = new ApiService();
              //  apiService.sendSMS("Votre code de confirmation est : " + key, _user.tel);
                ViewBag.email = email;
                return View();

                } else {
                TempData["error"] = "email non trouvé";
                return RedirectToAction("Signin");
                }

            }

        [HttpGet]
        public ActionResult verifyCode(string email,string code) {

            System.Web.HttpCookie myCookie = Request.Cookies["cookie"];

            if (myCookie != null && authservice.Decrypt(myCookie["key"]) == code) {
                var cookie = new System.Web.HttpCookie("cookievalid");

                cookie["valide"] = "true";
                Response.Cookies.Add(myCookie); 
                Response.Cookies.Add(cookie);


                return RedirectToAction("NewPassword", new { email = email });

                } else {

                myCookie["valide"] = "false";
                Response.Cookies.Add(myCookie);
                ViewBag.error = "code incorrect";


                return RedirectToAction("forgotpasswor");
                }


            }

       
        [HttpGet]
        //it will return the view to put the new password
        public ActionResult NewPassword(string email) {

            System.Web.HttpCookie myCookie = Request.Cookies["cookievalid"];
            if (myCookie != null && myCookie["valide"] == "true") {

                ViewBag.email = email;
                return View();
                } else {
                return RedirectToAction("Signin");
                }

            }  


        [HttpPost]
        public ActionResult NewPassword(string email, string pass, string cpass) {

            System.Web.HttpCookie myCookie = Request.Cookies["cookievalid"];
            if (myCookie != null && myCookie["valide"] == "true") {
                if (pass.Equals(cpass) && !String.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(pass)) {
                    User _us = db.Users.FirstOrDefault(x => x.mail == email);
                    _us.password = authservice.HashPassword(pass);
                    db.Entry(_us).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("signin");
                    } else {
                    ViewBag.error = "les mots de passe ne correspondent pas";
                    ViewBag.email = email;
                    return View();
                    }

               
                } else {
                return RedirectToAction("Signin");
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
