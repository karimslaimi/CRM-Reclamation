using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Security;
using PFE_reclamation.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PFE_reclamation.Controllers {
    [CustomAuthorize("RRC")]
    public class RRCController : Controller {
        //in this controller i don't really know what to do i have to check if the views are ok and test the methods
        //add verify method and reclam check method

        DatabContext db = new DatabContext();
        Authentication authservice = new Authentication();
        ApiService apiservice = new ApiService();



        // GET: RRC
        public ActionResult Index() {
            return View();
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
            Responsable_relation_client _rrc = db.Responsable_Relation_Clients.FirstOrDefault(x => x.id == id);



            if (TempData["error"] != null) {
                ViewBag.passerr = TempData["error"];
                }
            if (TempData["msg"] != null) {
                ViewBag.passmsg = TempData["msg"];
                }
            return View(_rrc);



            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult profile(Responsable_relation_client _rrc, HttpPostedFileBase postedFile) {

            //we get the object so we can fill the fields left empty
            Responsable_relation_client rrc = db.Responsable_Relation_Clients.AsNoTracking().FirstOrDefault(x => x.id == _rrc.id);

            _rrc.password = rrc.password;

            ModelState.Remove("password");

            if (ModelState.IsValid) {
                try {

                    if (postedFile != null && verifyFiles(postedFile)) {
                        string path = Server.MapPath("/Content/images/");
                        if (!Directory.Exists(path)) {
                            Directory.CreateDirectory(path);
                            }

                        if (System.IO.File.Exists(Path.GetFullPath(path + "profile_" + _rrc.id + Path.GetExtension(postedFile.FileName))))
                            System.IO.File.Delete(path + "profile_" + _rrc.id + Path.GetExtension(postedFile.FileName));

                        postedFile.SaveAs(path + "profile_" + _rrc.id + Path.GetExtension(postedFile.FileName));
                        _rrc.photo = Path.GetFileName("profile_" + _rrc.id + Path.GetExtension(postedFile.FileName));
                        }


                    db.Entry(_rrc).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.msg = "Profile modifié avec succés";
                    } catch (Exception e) {
                    ViewBag.error = "Erreur est survenu réessayer";
                    }
                } else {
                ViewBag.error = "Erreur est survenu réessayer";
                }


            return View(_rrc);



            }


        public ActionResult passwordchange(string pass, string cpass, int id) {

            if (pass.Equals(cpass) && !String.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(pass)) {
                Responsable_relation_client _rrc = db.Responsable_Relation_Clients.Find(id);
                _rrc.password = authservice.HashPassword(pass);
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
                    _client.password = authservice.HashPassword(_client.password);
                    db.Clients.Add(_client);
                    db.SaveChanges();
                    apiservice.sendmail("Votre compte a été créé dans le crm vous pouvez vous connectez\nContactez l'administrateur pour le mot de passe ", "Compte créé", _client.mail);
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




        public ActionResult clientpasswordchange(string pass, string cpass, int id) {

            if (pass.Equals(cpass) && !String.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(pass)) {
                Client _client = db.Clients.Find(id);
                _client.password = authservice.HashPassword(pass);
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["error"] = "les mots de passe ne correspondent pas";
                return Redirect("profile");
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
                }
            return Redirect("clients");
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
            return View(contrats);


            }

        public ActionResult clientReclam(int id) {
            if (id == 0) {
                return Redirect("clients");
                }

            List<Reclamation> reclamations = db.Reclamations.Where(x => x.Client.id == id).ToList();
            return View(reclamations);

            }



        public ActionResult reclams() {
            IEnumerable<Reclamation> _reclamas = db.Reclamations.ToList();
            return View(_reclamas);


            }

        public ActionResult treatedreclams() {
            IList<Reclamation> _reclams = db.Reclamations.Include(x => x.Traite.agent).Where(x => x.etat == Etat.Finis).ToList();
            return View(_reclams);


            }

        public ActionResult verify(int? id) {
            if (id != null) {
                // i have to implement mail to tell client about the verification

                Reclamation _reclam = db.Reclamations.Find(id);
                if (_reclam.etat == Etat.Nouveau) {
                    _reclam.etat = Etat.En_cours;
                    db.Entry(_reclam).State = EntityState.Modified;
                    db.SaveChanges();
                    apiservice.sendmail("Votre réclamation " + _reclam.titre + " a été approuvé et elle est en cours de traitement", "réclamation vérifié", _reclam.Client.mail);
                    }



                }
            return Redirect("reclams");
            }


        public ActionResult deleteReclam(int? id) {
            if (id == null) {
                return RedirectToAction("reclams");
                } else {
                Reclamation _reclam = db.Reclamations.Find(id);
                if (_reclam.etat == Etat.Nouveau) {
                    db.Reclamations.Remove(db.Reclamations.Find(id));
                    db.SaveChanges();
                    }

                return RedirectToAction("reclams");
                }
            }


        public ActionResult messages(int? id) {
            int myid = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(c => c.Type == "id").Value);

            ViewBag.clients = db.Clients.Include(x => x.receivedmessages).Include("receivedmessages.sentBy").Include("receivedmessages.sentTo").Include(s => s.sentmessages).Include("sentmessages.sentTo").Include("sentmessages.sentBy").ToList();
            if (id != null) {
                Client _client = db.Clients.Find(id);
                if (_client != null) {
                    ViewBag.msgs = db.Messages.Include(i => i.sentBy).Include(c => c.sentTo).
                                       Where(x => (x.sentBy.id == id && x.sentTo.id == myid) || (x.sentTo.id == id && x.sentBy.id == myid)).ToList();

                    ViewBag.clname = _client.nom + " " + _client.prenom;
                    ViewBag.clid = id;
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