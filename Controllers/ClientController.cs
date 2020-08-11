using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PFE_reclamation.Controllers
{
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
            return View();
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
        public ActionResult profile(Client _client) {

            //we get the object so we can fill the fields left empty
            Client rrc = db.Clients.AsNoTracking().FirstOrDefault(x => x.id == _client.id);

            _client.password = rrc.password;



            if (ModelState.IsValid) {
                try {




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
            if (ModelState.IsValid) {
                int id = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
                Client cl = db.Clients.Find(id);
                _reclam.Client = cl;
                _reclam.debut_reclam = DateTime.Now;
               _reclam.etat = Etat.Nouveau;
                db.Reclamations.Add(_reclam);
                db.SaveChanges();
                }
            return Redirect("myreclams");


            }
 


        public ActionResult deletereclam(int id) {
            
            Reclamation _reclam=db.Reclamations.Find(id);
            if (_reclam != null) {

            db.Reclamations.Remove(_reclam);
                db.SaveChanges();
                }
            return Redirect("myreclams");
            }

        public ActionResult EditR(int recid,string titre, string descr, int Type) {
            // i will finish it later

            Reclamation oldreclam = db.Reclamations.Find(recid);
            if (!oldreclam.titre.Equals(titre) && !string.IsNullOrEmpty(titre) && !string.IsNullOrWhiteSpace(titre)) {
                oldreclam.titre = titre;
                }
            if (!oldreclam.description.Equals(descr) && !string.IsNullOrEmpty(descr) && !string.IsNullOrWhiteSpace(descr)) {
                oldreclam.description = descr;
                }
            if (oldreclam.type!=(Types)Type) {
                oldreclam.type = (Types)Type;
                }
            db.Entry(oldreclam).State = EntityState.Modified;
            db.SaveChanges();
             




            return Redirect("myreclams");

            }


    }
}