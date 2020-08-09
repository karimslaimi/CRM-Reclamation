﻿using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Security;
using PFE_reclamation.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
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
        public ActionResult newClient(Client _client,string cpass) {
            //checl if the 2 pass are equals
            if (_client.password.Equals(cpass) ) {

               
            if (ModelState.IsValid) {
                db.Clients.Add(_client);
                db.SaveChanges(); 
                return RedirectToAction("clients");
                } else {
                    ViewBag.error = "vérifier les données saisi";
                    }
                } else {
                ViewBag.passerr = "vérifier les mots de passe";
                }
            return View(_client);
            }



        public ActionResult Editc(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            Client _client = db.Clients.Where(x => x.id == id).Include(s=>s.Contrats).Include(r=>r.Reclamations).FirstOrDefault();
            if (_client == null) {
                return HttpNotFound();
                }
            return View(_client);
            }

        // POST: Users/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editc(Client _client) {
            _client.password = db.Clients.Find(_client.id).password;


            if (ModelState.IsValid) {
                db.Entry(_client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("clients");
                }
            return View(_client);
            }

        // GET: Users/Delete/5
        public ActionResult Deletec(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            Client _client = db.Clients.Find(id);
            if (_client == null) {
                return HttpNotFound();
                }
            return Redirect("clients");
            }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Deletec")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Client _client = db.Clients.Find(id);
            db.Clients.Remove(_client);
            db.SaveChanges();
            return RedirectToAction("Index");
            }


        public ActionResult newContrat(string titre, string descr,DateTime debut,DateTime fin,string clid) {

            int idc = int.Parse(clid);
         

            Contrat contrat = new Contrat();
            contrat.Client = db.Clients.Find(idc);
            contrat.deb_contrat = debut;
            contrat.fin_contrat = fin;
            contrat.titre = titre;
            contrat.description = descr;
            db.Contrats.Add(contrat);
            db.SaveChanges();
            return RedirectToAction("clientContrat", new { id = idc });


            }


        public ActionResult clientContrat(int id) {
            if (id == 0) {
                return Redirect("clients");
                }
            List<Contrat> contrats = db.Contrats.Where(x => x.Client.id == id).ToList();
            ViewBag.clientid = id;
            return View(contrats);


            }

        public ActionResult clientReclam(int id) {
            if (id == 0) {
                return Redirect("clients");
             }

            List<Reclamation> reclamations = db.Reclamations.Where(x => x.Client.id == id).ToList();
            return View(reclamations);

            }

        }
    }