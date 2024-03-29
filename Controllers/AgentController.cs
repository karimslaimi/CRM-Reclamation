﻿using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Security;
using PFE_reclamation.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PFE_reclamation.Controllers {
    [CustomAuthorize("AGENT")]
    public class AgentController : Controller
    {
        DatabContext db = new DatabContext();
        Authentication authservice = new Authentication();
        ApiService apiservice = new ApiService();

        // GET: Agent
        public ActionResult Index()
        {
            //nothing to show in the index so redirect to untreated reclam


            return RedirectToAction("reclams");
        }
        protected bool verifyFiles(HttpPostedFileBase item) {
            bool flag = true;

            if (item != null) {
                if (item.ContentLength > 0 && item.ContentLength < 5000000) {
                    if (!(Path.GetExtension(item.FileName).ToLower() == ".jpg" ||
                        Path.GetExtension(item.FileName).ToLower() == ".png" ||
                        Path.GetExtension(item.FileName).ToLower() == ".jpeg")) {
                        flag = false;
                        }
                    } else { flag = false; }
                } else { flag = false; }

            return flag;
            }


        [HttpGet]
        public ActionResult profile() {
            //get the userid from claims principal where we stored the user data after login
            string userid = ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value;
            int id = int.Parse(userid);
            Agent _agent = db.Agents.FirstOrDefault(x => x.id == id);



            if (TempData["error"] != null) {
                ViewBag.passerr = TempData["error"];
                }
            if (TempData["msg"] != null) {
                ViewBag.passmsg = TempData["msg"];
                }
            return View(_agent);



            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult profile(Agent _agent, HttpPostedFileBase postedFile) {

            //we get the object so we can fill the fields left empty
            Agent agent = db.Agents.Include(x=>x.departement).AsNoTracking().FirstOrDefault(x => x.id == _agent.id);

            _agent.password = agent.password;
            //remove the password to ignore password validation
            ModelState.Remove("password");

            if (ModelState.IsValid) {
                try {
                    //check the filed if it s valid and check if there is a file with the same name so delete it and put the new pic in place

                    if (postedFile != null && verifyFiles(postedFile)) {
                        string path = Server.MapPath("/Content/images/");
                        if (!Directory.Exists(path)) {
                            Directory.CreateDirectory(path);
                            }

                        if (System.IO.File.Exists(Path.GetFullPath(path + "profile_" + _agent.id + Path.GetExtension(postedFile.FileName))))
                            System.IO.File.Delete(path + "profile_" + _agent.id + Path.GetExtension(postedFile.FileName));

                        postedFile.SaveAs(path + "profile_" + _agent.id + Path.GetExtension(postedFile.FileName));
                        _agent.photo = Path.GetFileName("profile_" + _agent.id + Path.GetExtension(postedFile.FileName));
                        }


                    db.Entry(_agent).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.msg = "Profile modifié avec succés";

                    } catch (Exception e) {
                    ViewBag.error = "Erreur est survenu réessayer";
                    }
                } else {
                ViewBag.error = "Erreur est survenu réessayer";
                }
            _agent.departement = agent.departement;

            return View(_agent);



            }


        public ActionResult passwordchange(string pass, string cpass, int id) {

            if (pass.Equals(cpass) && !string.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(pass)) {
                Agent _agent = db.Agents.Find(id);
                _agent.password = authservice.HashPassword(pass);
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["error"] = "les mots de passe ne correspondent pas";
                return Redirect("profile");
                }



            return Redirect("profile");
            }

        public ActionResult traiter(int idrec,string detaille) {



            //get the reclam and change its state and the end date
            Reclamation _reclam = db.Reclamations.Find(idrec);
            _reclam.etat = Etat.Traite;
            _reclam.fin_reclam = DateTime.Now;
            db.Entry(_reclam).State = EntityState.Modified;

            //get the traite object that where created by the Respo Dep find by reclam id 

            Traite _traite = db.Traites.Include(x => x.agent).Include(w => w.Reclamation.Client).Where(x => x.Reclamation.id == idrec).FirstOrDefault();



            //set the date and the details
            _traite.date = DateTime.Now;
            _traite.detaille = detaille;
            

            //update
            db.Entry(_traite).State = EntityState.Modified;
            db.SaveChanges();
            apiservice.sendmail("Votre réclamation "+_traite.Reclamation.titre+" a été traité par l'agent"+_traite.agent.nom+" "+_traite.agent.prenom+"\n"+_traite.detaille, "Réclamation traité", _traite.Reclamation.Client.mail);

            //i think the sms works fine wish it won't raise exception later
            apiservice.sendSMS("Votre réclamation " + _reclam.titre + " a été traité veuillez consulter votre compte",_reclam.Client.tel) ;


            return Redirect("reclams");

            }

        public ActionResult reclams() {
            //get agent the claims that where sent to him and still untreated
            int id = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
            List<Reclamation> _reclams = db.Reclamations.Where(x => x.Traite.agent.id == id && x.etat==Etat.En_cours).ToList();
            return View(_reclams);

            }

        public ActionResult reclams_traite() {
            //his treated claims
            int id = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
            List<Reclamation> _reclams = db.Reclamations.Include(x => x.Traite.agent).Where(x => x.etat == Etat.Traite && x.Traite.agent.id==id).ToList();
            return View(_reclams);
           
            
            }


        public ActionResult messages(int? id) {
            //the agent can discuss with his manager
            //get his id from the claimsprincipal and let s get the departement he belongs to and get his manager
         int myid = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(c => c.Type == "id").Value);
            Departement dep = db.Agents.Find(myid).departement;
            ViewBag.rd = db.Responsable_Departements.Include(x => x.receivedmessages).Include("receivedmessages.sentBy").Include("receivedmessages.sentTo").Include(s => s.sentmessages).Include("sentmessages.sentTo").Include("sentmessages.sentBy").Where(d=>d.departementId==dep.id).ToList();
         
            if (id != null) {
                 Responsable_departement _rd = db.Responsable_Departements.Find(id);
               if (_rd != null) {
                    ViewBag.msgs = db.Messages.Include(i => i.sentBy).Include(c => c.sentTo).
                    Where(x => (x.sentBy.id == id && x.sentTo.id == myid) || (x.sentTo.id == id && x.sentBy.id == myid)).ToList();


                    ViewBag.rdpic = _rd.photo;
                ViewBag.rdname = _rd.nom + " " + _rd.prenom;
                ViewBag.rdid = id;
                    }
                }

            return View();

            }

        public ActionResult sendmsg(int to, string msg) {
            if (!string.IsNullOrEmpty(msg)) {
                Message _msg = new Message();
                _msg.content = msg;
                int sentbyid = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(c => c.Type == "id").Value);
                _msg.sentBy = db.Users.FirstOrDefault(x => x.id == sentbyid);
                _msg.sentTo = db.Users.FirstOrDefault(x => x.id == to);
                _msg.date = DateTime.Now;
                db.Messages.Add(_msg);
                db.SaveChanges();
                }
            return RedirectToAction("messages", new { id = to });
            }




        }
    }