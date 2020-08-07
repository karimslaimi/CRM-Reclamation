using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Security;
using PFE_reclamation.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PFE_reclamation.Controllers {
    [CustomAuthorize("ADMIN")]
    public class AdminController : Controller {
        DatabContext db = new DatabContext();
        Authentication authservice = new Authentication();


        // GET: Admin

        public ActionResult Index() {




            return View();
            }



        [HttpGet]
        public ActionResult profile() {
            //get the userid from claims principal where we stored the user data after login
            string userid = ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value;
            int id = int.Parse(userid);
            Admin _admin = db.Admins.FirstOrDefault(x => x.id == id);



            if (TempData["error"] != null) {
                ViewBag.passerr = TempData["error"];
                }
            if (TempData["msg"] != null) {
                ViewBag.passmsg = TempData["msg"];
                }
            return View(_admin);



            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult profile(Admin _admin) {

            //we get the object so we can fill the fields left empty
            Admin ad = db.Admins.AsNoTracking().FirstOrDefault(x => x.id == _admin.id);

            _admin.password = ad.password;



            if (ModelState.IsValid) {
                try {




                    db.Entry(_admin).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.msg = "Profile modifié avec succés";
                    } catch (Exception e) {
                    ViewBag.error = "Erreur est survenu réessayer";
                    }
                } else {
                ViewBag.error = "Erreur est survenu réessayer";
                }


            return View(_admin);



            }


        public ActionResult passwordchange(string pass, string cpass, int id) {

            if (pass.Equals(cpass) && !String.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(pass)) {
                Admin _admin = db.Admins.Find(id);
                _admin.password = authservice.HashPassword(pass);
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["error"] = "les mots de passe ne correspondent pas";
                return Redirect("profile");
                }



            return Redirect("profile");
            }



        /////////// client management section


        //this method will return client list 
        public ActionResult clients() {

            return View(db.Clients.ToList<Client>());

            }

        //add new client get method
        [HttpGet]
        public ActionResult newClient() {

            return View();
            }

        //post method for new client
        [HttpPost]
        public ActionResult newClient(Client _client) {
            if (ModelState.IsValid) {
                db.Clients.Add(_client);
                db.SaveChanges();
                }
            return RedirectToAction("clients");
            }







        }
    }