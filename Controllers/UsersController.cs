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
                    ViewBag.error="Mot de passe incorrect";
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




        [HttpPost]
        public ActionResult save_c(Client client)
        {
            

            return View();
        }





        // GET: Users
        public ActionResult Index()
        {
             
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nom,prenom,cin,tel,mail,username,password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nom,prenom,cin,tel,mail,username,password")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
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
