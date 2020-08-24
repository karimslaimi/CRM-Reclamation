using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Security;
using PFE_reclamation.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;
using System.IO;
using Syncfusion.HtmlConverter;
using SelectPdf;
using System.Web.Security;

namespace PFE_reclamation.Controllers {
    [CustomAuthorize("ADMIN")]
    public class AdminController : Controller {


        //here i have to add reclam mmethods the cheeck and the verify methods
        //and checkfor sami the superviseur management 


        DatabContext db = new DatabContext();
        Authentication authservice = new Authentication();
        ApiService apiservice = new ApiService();


        // GET: Admin

        public ActionResult Index() {

            List<Reclamation> _reclams = db.Reclamations.ToList();


            ViewBag.traitereclam = _reclams.Where(x => x.etat == Etat.Finis).Count();
            ViewBag.encourreclam = _reclams.Where(x => x.etat == Etat.En_cours).Count();
            ViewBag.nbreclam = _reclams.Count();
            ViewBag.clnb = db.Clients.Count();

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

            ModelState.Remove("password");

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
                db.Entry(_admin).State = EntityState.Modified;
                db.SaveChanges();
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
        public ActionResult newClient(Client _client, string cpass) {
            //checl if the 2 pass are equals
            if (_client.password.Equals(cpass)) {


                if (ModelState.IsValid) {
                    try {
                        _client.password = authservice.HashPassword(_client.password);
                        db.Clients.Add(_client);
                        db.SaveChanges();
                        apiservice.sendmail("Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe ", "Compte créé", _client.mail);
                        } catch (Exception e) {
                        ViewBag.error = "vérifier mail ou nom d'utilisateur";
                        return View(_client);
                        }


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
            Client _client = db.Clients.Where(x => x.id == id).Include(s => s.Contrats).Include(r => r.Reclamations).FirstOrDefault();
            if (_client == null) {
                return HttpNotFound();
                }
            if (TempData["passerr"] != null) {
                ViewBag.passerr = TempData["error"];
                }
            if (TempData["msg"] != null) {
                ViewBag.passmsg = TempData["msg"];
                }
            return View(_client);
            }

        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editc(Client _client) {
            _client.password = db.Clients.AsNoTracking().FirstOrDefault(x => x.id == _client.id).password;


            if (ModelState.IsValid) {
                db.Entry(_client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("clients");
                }
            return View(_client);
            }



        public ActionResult passwordchangeclient(string pass, string cpass, int id) {
            if (pass.Equals(cpass) && !string.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(cpass)) {

                Client _client = db.Clients.Find(id);
                _client.password = authservice.HashPassword(pass);
                db.Entry(_client).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["passerr"] = "Erreur est survenu réessayer";
                }
            return RedirectToAction("Editc", new { id = id });
            }


        // GET: Users/Delete/5
        public ActionResult Deletec(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            Client _client = db.Clients.Find(id);
            if (_client == null) {
                return HttpNotFound();
                } else {
                db.Clients.Remove(_client);
                db.SaveChanges();
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


        public ActionResult newContrat(string titre, string descr, DateTime debut, DateTime fin, string clid) {

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
            Client _client = db.Clients.FirstOrDefault(x => x.id == id);
            ViewBag.clientname = _client.nom + " " + _client.prenom;
            return View(contrats);


            }

        public ActionResult clientReclam(int id) {
            if (id == 0) {
                return Redirect("clients");
                }

            List<Reclamation> reclamations = db.Reclamations.Where(x => x.Client.id == id).ToList();
            return View(reclamations);

            }
        public ActionResult Vérifier_reclam(int? id) {
            if (id != null) {
                // i have to implement mail to tell client about the verification

                Reclamation _reclam = db.Reclamations.Find(id);
                if (_reclam.etat == Etat.Nouveau) {
                    _reclam.etat = Etat.En_cours;
                    db.Entry(_reclam).State = EntityState.Modified;
                    db.SaveChanges();
                    apiservice.sendmail("Votre réclamation "+_reclam.titre+" a été approuvez et en cours de traitement", "réclamation vérifié", _reclam.Client.mail);
                    }



                }
            return RedirectToAction("reclams");
            }

        public ActionResult reclams() {
            List<Reclamation> _reclamas = db.Reclamations.OrderBy(x => x.etat).ToList();
            return View(_reclamas);
            }

        public ActionResult traite_reclams() {
            IList<Reclamation> _reclams = db.Reclamations.Include(x => x.Traite.agent).Where(x => x.etat == Etat.Finis).ToList();
            return View(_reclams);
            }
        public ActionResult encours_reclams() {
            IList<Reclamation> _reclams = db.Reclamations.Where(x => x.etat == Etat.En_cours).ToList();
            return View(_reclams);
            }














        //-----------------------Respo dep

        // créer un responsable

        [HttpGet]
        public ActionResult newResponsableDep() {
            List<Departement> dp = db.Departements.ToList();
            ViewBag.list = dp;
            return View();
            }


        [HttpPost]
        public ActionResult newResponsableDep(Responsable_departement responsable, string cpass, string select) {

            if (responsable.password.Equals(cpass)) {
                int id = Int32.Parse(select);
                if (id != 0)
                    responsable.departementId = id;

                if (ModelState.IsValid) {
                    Authentication authservice = new Authentication();
                    responsable.password = authservice.HashPassword(responsable.password);
                    db.Responsable_Departements.Add(responsable);
                    try {
                        db.SaveChanges();
                        apiservice.sendmail("Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe ", "Compte créé", responsable.mail);
                        return RedirectToAction("responsables");
                        } catch (Exception ee) { ViewBag.error = "Ce département à déja un responsable"; }

                    } else {
                    ViewBag.error = "vérifier les données saisi";
                    }
                } else {
                ViewBag.passerr = "vérifier les mots de passe";
                }

            List<Departement> dp = db.Departements.ToList();
            ViewBag.list = dp;
            return View(responsable);
            }

        [HttpGet]
        public ActionResult editResponsableDep(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            Responsable_departement _rs = db.Responsable_Departements.FirstOrDefault(x => x.id == id);
            if (_rs == null) {
                return HttpNotFound();
                }
            return View(_rs);
            }

        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editResponsableDep(Responsable_departement _rs) {

            Responsable_departement rd = db.Responsable_Departements.AsNoTracking().FirstOrDefault(x => x.id == _rs.id);
            _rs.password = rd.password;


            if (ModelState.IsValid) {
                db.Entry(_rs).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("responsables");
                }
            return View(_rs);
            }

        // afficher la liste des responsables dep
        public ActionResult responsables() {
            DatabContext db = new DatabContext();
            List<Responsable_departement> rs = db.Responsable_Departements.ToList();

            return View(rs);
            }

        // supprimer un responsable département
        public ActionResult deleteResponsableDep(int id) {

            Responsable_departement rs = db.Responsable_Departements.Find(id);

            db.Responsable_Departements.Remove(rs);
            db.SaveChanges();
            return RedirectToAction("responsables");

            }
        // methode pour  affichage des details d'un resp departement
        public ActionResult detailsResponsableDep(int id) {

            Responsable_departement rs = db.Responsable_Departements.Find(id);

            return View(rs);
            }

        public ActionResult passwordchangerd(string pass, string cpass, int id) {
            if (pass.Equals(cpass) && !string.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(cpass)) {

                Responsable_departement _rd = db.Responsable_Departements.Find(id);
                _rd.password = authservice.HashPassword(pass);
                db.Entry(_rd).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["passerr"] = "Erreur est survenu réessayer";
                }
            return RedirectToAction("editResponsableDep", new { id = id });
            }


        //------------------------superviseur-----------------------------

        // créer un superviseur
        [HttpGet]
        public ActionResult newSuperviseur() {
            return View();
            }
        [HttpPost]
        public ActionResult newSuperviseur(Superviseur superviseur, string cpass) {
            if (superviseur.password.Equals(cpass)) {
                if (ModelState.IsValid) {
                    Authentication authservice = new Authentication();
                    superviseur.password = authservice.HashPassword(superviseur.password);
                    db.Superviseurs.Add(superviseur);
                    apiservice.sendmail("Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe ", "Compte créé", superviseur.mail);

                    db.SaveChanges();

                    return RedirectToAction("superviseurs");
                    } else {
                    ViewBag.error = "vérifier les données saisi";
                    }
                } else {
                ViewBag.passerr = "vérifier les mots de passe";
                }
            return View(superviseur);
            }

        // afficher la liste des superviseurs
        public ActionResult superviseurs() {

            List<Superviseur> rs = db.Superviseurs.ToList();

            return View(rs);
            }

        // supprimer un responsable département
        public ActionResult deleteSuperviseur(int id) {

            Superviseur rs = db.Superviseurs.Find(id);
            db.Superviseurs.Remove(rs);
            db.SaveChanges();
            return RedirectToAction("superviseurs");

            }
        // methode pour  affichage des details d'un resp departement
        public ActionResult detailsSuperviseur(int id) {


            Superviseur rs = db.Superviseurs.Find(id);


            return View(rs);
            }
        [HttpGet]
        public ActionResult editSuperviseur(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            Superviseur _rs = db.Superviseurs.FirstOrDefault(x => x.id == id);
            if (_rs == null) {
                return HttpNotFound();
                }
            if (TempData["passerr"] != null) {
                ViewBag.passerr = TempData["error"];
                }
            if (TempData["msg"] != null) {
                ViewBag.passmsg = TempData["msg"];
                }
            return View(_rs);
            }

        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editSuperviseur(Superviseur _rs) {
            Superviseur sr = db.Superviseurs.AsNoTracking().FirstOrDefault(x => x.id == _rs.id);
            _rs.password = sr.password;


            if (ModelState.IsValid) {
                db.Entry(_rs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("superviseurs");
                }
            return View(_rs);
            }



        public ActionResult passwordchangesuperv(string pass, string cpass, int id) {
            if (pass.Equals(cpass) && !string.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(cpass)) {

                Superviseur _superv = db.Superviseurs.Find(id);
                _superv.password = authservice.HashPassword(pass);
                db.Entry(_superv).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["passerr"] = "Erreur est survenu réessayer";
                }
            return RedirectToAction("editSuperviseur", new { id = id });
            }


        //---------------Agents------------------------------

        // créer un agent
        [HttpGet]
        public ActionResult newAgent() {
            List<Departement> dp = db.Departements.ToList();
            ViewBag.list = dp;
            return View();
            }
        [HttpPost]
        public ActionResult newAgent(Agent agent, string cpass, string select) {


            if (agent.password.Equals(cpass)) {
                int id = Int32.Parse(select);
                if (id != 0)
                    agent.departement = db.Departements.Find(id);

                if (ModelState.IsValid) {
                    Authentication authservice = new Authentication();
                    agent.password = authservice.HashPassword(agent.password);
                    db.Agents.Add(agent);
                    db.SaveChanges();
                    apiservice.sendmail("Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe ", "Compte créé", agent.mail);
                    return RedirectToAction("agents");
                    } else {
                    ViewBag.error = "vérifier les données saisi";
                    }
                } else {
                ViewBag.passerr = "vérifier les mots de passe";
                }

            List<Departement> dp = db.Departements.ToList();
            ViewBag.list = dp;
            return View(agent);
            }

        // afficher la liste des agents
        public ActionResult agents() {

            List<Agent> rs = db.Agents.ToList();

            return View(rs);
            }

        // supprimer un agent

        [ValidateAntiForgeryToken]
        public ActionResult deleteAgent(int id) {

            Agent rs = db.Agents.Find(id);
            db.Agents.Remove(rs);
            db.SaveChanges();
            return RedirectToAction("agents");

            }
        // methode pour  affichage des details d'un agent
        public ActionResult detailsAgent(int id) {

            Agent rs = db.Agents.Find(id);

            return View(rs);
            }

        [HttpGet]
        public ActionResult editAgent(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            Agent _rs = db.Agents.FirstOrDefault(x => x.id == id);
            if (_rs == null) {
                return HttpNotFound();
                }
            return View(_rs);
            }

        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editAgent(Agent _rs) {
            Agent _rd = db.Agents.AsNoTracking().FirstOrDefault(x => x.id == _rs.id);

            _rs.password = _rd.password;


            if (ModelState.IsValid) {
                db.Entry(_rs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("agents");
                }
            return View(_rs);
            }

        public ActionResult passwordchangeagent(string pass, string cpass, int id) {
            if (pass.Equals(cpass) && !string.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(cpass)) {

                Agent _agent = db.Agents.Find(id);
                _agent.password = authservice.HashPassword(pass);
                db.Entry(_agent).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["passerr"] = "Erreur est survenu réessayer";
                }
            return RedirectToAction("editAgent", new { id = id });
            }
        //----------------------------rrc----------------------------

        // créer un RRC
        [HttpGet]
        public ActionResult newRRC() {
            return View();
            }
        [HttpPost]

        public ActionResult newRRC(Responsable_relation_client _rrc, string cpass) {
            if (_rrc.password.Equals(cpass)) {

                if (ModelState.IsValid) {
                    Authentication authservice = new Authentication();
                    _rrc.password = authservice.HashPassword(_rrc.password);
                    db.Responsable_Relation_Clients.Add(_rrc);
                    db.SaveChanges();
                    apiservice.sendmail("Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe ", "Compte créé", _rrc.mail);
                    return RedirectToAction("RRCs");
                    } else {
                    ViewBag.error = "vérifier les données saisi";
                    }
                } else {
                ViewBag.passerr = "vérifier les mots de passe";
                }
            return View(_rrc);
            }

        // afficher la liste des RRC
        public ActionResult RRCs() {

            List<Responsable_relation_client> rs = db.Responsable_Relation_Clients.ToList();

            return View(rs);
            }

        // supprimer un RRC
        public ActionResult deleteRRC(int id) {

            Responsable_relation_client rs = db.Responsable_Relation_Clients.Find(id);
            db.Responsable_Relation_Clients.Remove(rs);
            db.SaveChanges();
            return RedirectToAction("RRCs");

            }
        // methode pour  affichage des details d'un RRC
        public ActionResult detailsRRC(int id) {

            Responsable_relation_client rs = db.Responsable_Relation_Clients.Find(id);

            return View(rs);
            }

        //modifier un RRC
        [HttpGet]
        public ActionResult editRRC(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            Responsable_relation_client _rs = db.Responsable_Relation_Clients.FirstOrDefault(x => x.id == id);
            if (_rs == null) {
                return HttpNotFound();
                }
            return View(_rs);
            }

        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editRRC(Responsable_relation_client _rs) {

            Responsable_relation_client _rrc = db.Responsable_Relation_Clients.AsNoTracking().FirstOrDefault(x => x.id == _rs.id);
            _rs.password = _rrc.password;

            if (ModelState.IsValid) {
                db.Entry(_rs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("RRCs");

                }
            return View(_rs);
            }

        public ActionResult passwordchangerrc(string pass, string cpass, int id) {
            if (pass.Equals(cpass) && !string.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(cpass)) {

                Responsable_relation_client _rrc = db.Responsable_Relation_Clients.Find(id);
                _rrc.password = authservice.HashPassword(pass);
                db.Entry(_rrc).State = EntityState.Modified;
                db.SaveChanges();
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["passerr"] = "Erreur est survenu réessayer";
                }
            return RedirectToAction("editRRC", new { id = id });
            }



        }
    }