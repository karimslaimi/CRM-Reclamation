using PFE_reclamation.DAL;
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
    [CustomAuthorize("CLIENT")]
    public class ClientController : Controller {

        DatabContext db = new DatabContext();
        Authentication authservice = new Authentication();

        //in this controller i have to integrate all the views and test them 
        //i have to add some methods too like 
        //i think i will add a chat module don't know but i will think about it
        //edit reclam and delete reclam 
        //i have to check whether he is the owner of the reclam to edit it or delete it




        // GET: Client
        public ActionResult Index() {

            //get the current client id 
            int userid = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);

            //now we will retrieve only his reclams we will look by his id and then filter and send in viewbag
            List<Reclamation> _reclams = db.Reclamations.Where(x => x.Client.id == userid).ToList();
            ViewBag.nbreclam = _reclams.Count();
            ViewBag.encourreclam = _reclams.Where(x => x.etat == Etat.En_cours).Count();
            ViewBag.traitereclam = _reclams.Where(x => x.etat == Etat.Traite).Count();
            ViewBag.contrat = db.Contrats.Where(x => x.Client.id == userid).Count();
            ViewBag.reclams = _reclams.OrderBy(x=>x.debut_reclam).Take(5).ToList();





            return View();
            }

        protected bool verifyFiles(HttpPostedFileBase item)
        {
            bool flag = true;

            if (item != null)
            {
                if (item.ContentLength > 0 && item.ContentLength < 5000000)
                {
                    if (!(Path.GetExtension(item.FileName).ToLower() == ".jpg" ||
                        Path.GetExtension(item.FileName).ToLower() == ".png" ||
                        Path.GetExtension(item.FileName).ToLower() == ".jpeg"))
                    {
                        flag = false;
                    }
                }
                else { flag = false; }
            }


            else { flag = false; }

            return flag;
        }

        [HttpGet]
        public ActionResult profile() {
            //get the userid from claims principal where we stored the user data after login
            string userid = ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value;
            int id = int.Parse(userid);
            Client _client = db.Clients.FirstOrDefault(x => x.id == id);



            if (TempData["error"] != null) {
                ViewBag.passerr = TempData["error"];
                }
            if (TempData["msg"] != null) {
                ViewBag.passmsg = TempData["msg"];
                }
            return View(_client);



            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult profile(Client _client, HttpPostedFileBase postedFile) {

            //we get the object so we can fill the fields left empty
            Client rrc = db.Clients.AsNoTracking().FirstOrDefault(x => x.id == _client.id);

            _client.password = rrc.password;
            ModelState.Remove("password");


            if (ModelState.IsValid) {
                try {
                    //check the file if it s valid delete the old picture if existing and save the changes
                    if (postedFile != null && verifyFiles(postedFile))
                    {
                        string path = Server.MapPath("/Content/images/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        if (System.IO.File.Exists(Path.GetFullPath(path + "profile_" + _client.id + Path.GetExtension(postedFile.FileName))))
                            System.IO.File.Delete(path + "profile_" + _client.id + Path.GetExtension(postedFile.FileName));

                        postedFile.SaveAs(path + "profile_"+_client.id+ Path.GetExtension(postedFile.FileName));
                        _client.photo = Path.GetFileName("profile_" + _client.id + Path.GetExtension(postedFile.FileName));
                    }

                    db.Entry(_client).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.msg = "Profile modifié avec succés";
                    } catch (Exception e) {
                    ViewBag.error = "Erreur est survenu réessayer";
                    }
                } else {
                ViewBag.error = "Erreur est survenu réessayer";
                }


            return View(_client);



            }


        public ActionResult passwordchange(string pass, string cpass, int id) {

            if (pass.Equals(cpass) && !String.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(pass)) {
                Client _client = db.Clients.Find(id);
                _client.password = authservice.HashPassword(pass);
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["error"] = "les mots de passe ne correspondent pas";
                return Redirect("profile");
                }



            return Redirect("profile");
            }









        public ActionResult myreclams() {
            int id = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
            List<Reclamation> _reclams = db.Reclamations.Where(x => x.Client.id == id).ToList();
            return View(_reclams);

            }

        [HttpGet]
        public ActionResult addreclam() {
            Reclamation _reclam = new Reclamation();

            return View(_reclam);
            }
        [HttpPost]
        public ActionResult addreclam(Reclamation _reclam) {
            ModelState.Remove("Client");
            if (ModelState.IsValid) {
                //as we have a relation between the client and the reclam we have to get the client who created the reclam and put him in the reclam
                
                int id = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
                Client cl = db.Clients.Find(id);
                _reclam.Client = cl;
                _reclam.debut_reclam = DateTime.Now;
                _reclam.etat = Etat.Nouveau;
                db.Reclamations.Add(_reclam);
                db.SaveChanges();
                } else {
                return View(_reclam);
                }
            return Redirect("myreclams");


            }



        public ActionResult deletereclam(int id) {

            Reclamation _reclam = db.Reclamations.Include(x=>x.Traite).FirstOrDefault(x=>x.id==id);
            //check if the id isn't null and chech is he is the owner of the reclam
            if (_reclam != null && _reclam.Client.id == int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value)) {


                db.Reclamations.Remove(_reclam);
                db.SaveChanges();
                }
            return Redirect("myreclams");
            }

        public ActionResult EditR(int recid, string titre, string descr, int Type) {


            Reclamation oldreclam = db.Reclamations.Find(recid);

            //check if his the owner of the claim
            if (oldreclam.Client.id == int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value)) {


                if (!oldreclam.titre.Equals(titre) && !string.IsNullOrEmpty(titre) && !string.IsNullOrWhiteSpace(titre)) {
                    oldreclam.titre = titre;
                    }
                if (!oldreclam.description.Equals(descr) && !string.IsNullOrEmpty(descr) && !string.IsNullOrWhiteSpace(descr)) {
                    oldreclam.description = descr;
                    }
                if (oldreclam.type != (Types)Type) {
                    oldreclam.type = (Types)Type;
                    }
                db.Entry(oldreclam).State = EntityState.Modified;
                db.SaveChanges();

                }



            return Redirect("myreclams");

            }

        public ActionResult reclamation_traite() {
            int idc = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
            List<Reclamation> _reclams = db.Reclamations.Include(x => x.Traite.agent).Where(x => x.etat == Etat.Traite && x.Client.id == idc).ToList();
            return View(_reclams);

            }


        public ActionResult myContracts() {

            int id = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
            List<Contrat> _contrat = db.Contrats.Where(x => x.Client.id == id).ToList();
            return View(_contrat);

            }



        public ActionResult messages(int? id) {

            //the client can communicate with the rrc

            int myid = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(c => c.Type == "id").Value);
            ViewBag.rrc = db.Responsable_Relation_Clients.Include(x => x.receivedmessages).Include("receivedmessages.sentBy").Include("receivedmessages.sentTo")
                .Include(s => s.sentmessages).Include("sentmessages.sentTo").Include("sentmessages.sentBy").ToList();
            if (id != null) {
                Responsable_relation_client _rrc = db.Responsable_Relation_Clients.Find(id);
                if (_rrc != null) {
                    ViewBag.msgs = db.Messages.Include(i => i.sentBy).Include(c => c.sentTo).
                    Where(x => (x.sentBy.id == id && x.sentTo.id == myid) || (x.sentTo.id == id && x.sentBy.id == myid)).ToList();

                    ViewBag.rrcname = _rrc.nom + " " + _rrc.prenom;
                    ViewBag.rrcid = id;
                    ViewBag.rrcimg = _rrc.photo;
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