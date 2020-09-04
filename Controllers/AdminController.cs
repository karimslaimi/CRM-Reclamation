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
using System.Web;

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

            //getting all reclams from db , filter it and send it to the index for the charts
            List<Reclamation> _reclams = db.Reclamations.ToList();


            ViewBag.traitereclam = _reclams.Where(x => x.etat == Etat.Traite).Count();
            ViewBag.encourreclam = _reclams.Where(x => x.etat == Etat.En_cours).Count();
            ViewBag.nbreclam = _reclams.Count();
            ViewBag.clnb = db.Clients.Count();
            ViewBag.clients = db.Clients.Include(s => s.Reclamations).Where(x => x.Reclamations.Count() > 0).OrderBy(x => x.Reclamations.Count()).Take(5).ToList();

            ViewBag.lastreclams = db.Reclamations.Where(x => x.etat == Etat.Nouveau).OrderBy(s => s.debut_reclam).Take(5);

          


            return View();
            }
        public PartialViewResult chart()
        {
            
            return PartialView();
        }
        public JsonResult json()
        {
            //réclamations par département
            List<FirstChart> chart1 = db.Reclamations.GroupBy(x => x.Departement).Select(x => new FirstChart { name = x.Key.label, y = x.Count() }).ToList();
           foreach(FirstChart fs in chart1)
            {
                if (fs.name == null)
                    fs.name = "Non Attribué";
            }
           //réclamations par mois
            List<FirstChart> chart2 = db.Reclamations.GroupBy(x=>x.debut_reclam.Month.ToString()).Select(x => new FirstChart { name = x.Key.ToString(), y = x.Count() }).ToList();
            chart2.OrderBy(x=>x.name).ToList().ForEach(x=>x.name=new DateTime(2000, Int32.Parse(x.name), 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("fr")));
            
            //liste à envoyer
            List<List<FirstChart>> final = new List<List<FirstChart>>();
            final.Add(chart1);
            final.Add(chart2);
            List<FirstChart> chart3= db.Reclamations.GroupBy(x => x.etat).Select(s => new FirstChart { name = s.Key.ToString(), y = s.Count() }).ToList();
            final.Add(chart3);
            DateTime testDate = DateTime.Now.AddYears(-1);
            DateTime testDate1 = DateTime.Now.AddYears(1);
            DateTimeFormatInfo mn = CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat;
            List<Contrat> cn = db.Contrats.Where(x => x.deb_contrat > testDate && x.deb_contrat < testDate1).ToList();
            List<FirstChart> chart4 = cn.OrderBy(x=>x.deb_contrat.Month).GroupBy(x => x.deb_contrat.Month).Select(s => new FirstChart { name = mn.GetMonthName(s.Key).ToString() , y = s.Count() }).ToList();
              
            final.Add(chart4);
            return Json(final, JsonRequestBehavior.AllowGet);
        }
        protected bool verifyFiles(HttpPostedFileBase item) {
            bool flag = true;

            if (item != null) {
                //check if the size and the extension are ok
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
        public ActionResult profile(Admin _admin, HttpPostedFileBase postedFile) {

            //we get the object so we can fill the fields left empty
            Admin ad = db.Admins.AsNoTracking().FirstOrDefault(x => x.id == _admin.id);

            _admin.password = ad.password;

            //we have done this one to ignore the password validation
            ModelState.Remove("password");

            if (ModelState.IsValid) {
                try {
                    //check if the file is allowed 
                    if (postedFile != null && verifyFiles(postedFile)) {
                        string path = Server.MapPath("/Content/images/");
                        if (!Directory.Exists(path)) {
                            Directory.CreateDirectory(path);
                            }

                        //if the image exist then remove it
                        if (System.IO.File.Exists(Path.GetFullPath(path + "profile_" + _admin.id + Path.GetExtension(postedFile.FileName))))
                            System.IO.File.Delete(path + "profile_" + _admin.id + Path.GetExtension(postedFile.FileName));

                        //save the new file
                        postedFile.SaveAs(path + "profile_" + _admin.id + Path.GetExtension(postedFile.FileName));
                        _admin.photo = Path.GetFileName("profile_" + _admin.id + Path.GetExtension(postedFile.FileName));
                        }




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
                //encrypt the password in the authservice 
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
                        _client.enabled = true;
                        db.Clients.Add(_client);
                        db.SaveChanges();

                        //send mail and sms to the client
                        apiservice.sendmail(_client.mail,"Compte créé","Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe") ;
                        apiservice.sendSMS("Votre compte a été créé contacter l'administrateur pour le mot de passe", _client.tel);
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

            ModelState.Remove("password");

            if (ModelState.IsValid) {
                db.Entry(_client).State = EntityState.Modified;
                db.SaveChanges();
              ViewBag.msg="Modifié avec succés";
                } else {
                ViewBag.error = "verifier les donnés saisi";
                }
            return View(_client);
            }



        public ActionResult passwordchangeclient(string pass, string cpass, int id) {
            if (pass.Equals(cpass) && !string.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(cpass) && pass.Length>=8) {

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
            Client _client = db.Clients.Include(x=>x.Contrats).Include(x=>x.Reclamations).Include(s=>s.receivedmessages).Include(q=>q.sentmessages).FirstOrDefault(x=>x.id==id);
            if (_client == null) {
                return HttpNotFound();
                } else {
                //if we delete the client we have to delete his reclams so we should disable his account he won't be able to connect
                _client.enabled = false;
                db.Entry(_client).State = EntityState.Modified;
                db.SaveChanges();
                }
            return Redirect("clients");
            }



        public ActionResult enableclient(int id) {
            if (id != 0) {
                //enable the client account in case he s back
                Client _client = db.Clients.Find(id);
                if (_client != null) {
                    _client.enabled = true;
                    db.Entry(_client).State = EntityState.Modified;
                    db.SaveChanges();

                    }
                }
            return RedirectToAction("Editc", new { id = id });

            }

        public ActionResult newContrat(string titre, string descr , string datecontrat, string clid) {

            int idc = int.Parse(clid);


            Contrat contrat = new Contrat();
            contrat.Client = db.Clients.Find(idc);
            string d1 = datecontrat.Substring(0,datecontrat.IndexOf("-"));
            string d2 = datecontrat.Substring(datecontrat.IndexOf("-")+1);

       
            contrat.deb_contrat =  DateTime.Parse(d1);
            contrat.fin_contrat = DateTime.Parse(d2);
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
                //verify the reclam and send mail to the creator of the reclam
                Reclamation _reclam = db.Reclamations.Find(id);
                if (_reclam.etat == Etat.Nouveau) {
                    _reclam.etat = Etat.En_cours;
                    db.Entry(_reclam).State = EntityState.Modified;
                    db.SaveChanges();
                    apiservice.sendmail( _reclam.Client.mail, "réclamation vérifié","Votre réclamation "+_reclam.titre+" a été approuvé et en cours de traitement");
                    }



                }
            return RedirectToAction("reclams");
            }

        public ActionResult reclams() {
            //get all the reclam even with orderby the datatable is ordering the data by the id 
            List<Reclamation> _reclamas = db.Reclamations.OrderBy(x => x.etat).ToList();
            return View(_reclamas);
            }

        public ActionResult traite_reclams() {
            //find treated reclams
            IList<Reclamation> _reclams = db.Reclamations.Include(x => x.Traite.agent).Where(x => x.etat == Etat.Traite).ToList();
            return View(_reclams);
            }
        public ActionResult encours_reclams() {
            //verified reclams but still not treated
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
                //the select is the id of the departement
                int id = Int32.Parse(select);
                if (id != 0)
                    responsable.departementId = id;

                if (ModelState.IsValid) {
                    Authentication authservice = new Authentication();
                    //hash the password in the authservice
                    responsable.password = authservice.HashPassword(responsable.password);
                    responsable.enabled = true;
                    db.Responsable_Departements.Add(responsable);
                    try {
                        db.SaveChanges();
                        apiservice.sendmail(responsable.mail,  "Compte créé","Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe");
                        return RedirectToAction("responsables");
                        } catch (Exception ee) { ViewBag.error = "Ce département à déja un responsable"; }

                    } else {
                    ViewBag.error = "vérifier les données saisi";
                    }
                } else {
                ViewBag.passerr = "vérifier les mots de passe";
                }
            //send back the dep list in the viewbag
            //if he s here that means the validation failed and he have to fix the given informations
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
            if (TempData["passerr"] != null)
            {
                ViewBag.passerr = TempData["error"];
            }
            if (TempData["msg"] != null)
            {
                ViewBag.passmsg = TempData["msg"];
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

            //include his messages so it can be deleted along with the user
            Responsable_departement rs = db.Responsable_Departements.Include(x=>x.sentmessages).Include(x=>x.receivedmessages).SingleOrDefault(x=>x.id==id);

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
                    superviseur.enabled = true;
                    db.Superviseurs.Add(superviseur);
                  

                    db.SaveChanges(); 
                    apiservice.sendmail(superviseur.mail, "Compte créé","Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe " );

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
                //the select is the id of the departement the agent belong to
                int id = Int32.Parse(select);
                if (id != 0)
                    agent.departement = db.Departements.Find(id);

                if (ModelState.IsValid) {
                    Authentication authservice = new Authentication();
                    agent.password = authservice.HashPassword(agent.password);
                    agent.enabled = true;
                    db.Agents.Add(agent);
                    db.SaveChanges();
                    apiservice.sendmail(agent.mail,"Compte créé","Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe "  );
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
            //won't delete it because he may have entries in the traite table that way it will raise an exception and somehow we managed to bypass the exception 
            //the reclams he treated will be deleted
            Agent rs = db.Agents.Find(id);
            rs.enabled = false;
            db.Entry(rs).State = EntityState.Modified;
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
            if (TempData["passerr"] != null)
            {
                ViewBag.passerr = TempData["error"];
            }
            if (TempData["msg"] != null)
            {
                ViewBag.passmsg = TempData["msg"];
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
                    _rrc.enabled = true;
                    db.SaveChanges();
                    apiservice.sendmail(_rrc.mail, "Compte créé","Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe " );
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

            Responsable_relation_client rs = db.Responsable_Relation_Clients.Include(x=>x.sentmessages).Include(x=>x.receivedmessages).SingleOrDefault(x=>x.id==id);
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
            if (TempData["passerr"] != null)
            {
                ViewBag.passerr = TempData["error"];
            }
            if (TempData["msg"] != null)
            {
                ViewBag.passmsg = TempData["msg"];
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