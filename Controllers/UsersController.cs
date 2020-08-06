using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Services;

namespace PFE_reclamation.Controllers
{
    public class UsersController : Controller
    {
        private DatabContext db = new DatabContext();

        private Authentication authservice = new Authentication();


        public ActionResult Signin(User user)
        {
            if (HttpContext.Request.HttpMethod == HttpMethod.Post.Method)
            {
                User auth = authservice.AuthUser(user.username, user.password);

                if (auth != null)
                {

                  

                    if (auth is Admin) return RedirectToAction("index", "Admin");

                    
                    else if (auth is Client) return RedirectToAction("index", "Client");
                    else if (auth is Agent) return RedirectToAction("index", "Agent");
                    else if (auth is Responsable_departement) return RedirectToAction("index", "RD");
                    else if (auth is Responsable_relation_client) return RedirectToAction("index", "RRC");
                    else if (auth is Superviseur) return RedirectToAction("index", "Superviseur");

                }
                else
                {
                    ViewBag.error="Error try again";
                    return View();
                }

            }
            else if(HttpContext.Request.HttpMethod == HttpMethod.Get.Method)
            {


                HttpContext.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);

                return View();
            }
            return View();
        }




        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(Microsoft.AspNet.Identity.DefaultAuthenticationTypes.ApplicationCookie);

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
