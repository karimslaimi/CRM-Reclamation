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
    [CustomAuthorize("AGENT")]
    public class AgentController : Controller
    {
        DatabContext db = new DatabContext();
        Authentication authservice = new Authentication();
        ApiService apiservice = new ApiService();

        // GET: Agent
        public ActionResult Index()
        {



            return View();
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
        public ActionResult profile(Agent _agent) {

            //we get the object so we can fill the fields left empty
            Agent agent = db.Agents.AsNoTracking().FirstOrDefault(x => x.id == _agent.id);

            _agent.password = agent.password;

            ModelState.Remove("password");

            if (ModelState.IsValid) {
                try {




                    db.Entry(_agent).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.msg = "Profile modifié avec succés";
                    } catch (Exception e) {
                    ViewBag.error = "Erreur est survenu réessayer";
                    }
                } else {
                ViewBag.error = "Erreur est survenu réessayer";
                }


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

            //add the view later and user the mailling service

            //get the reclam and change its state and the end date
            Reclamation _reclam = db.Reclamations.Find(idrec);
            _reclam.etat = Etat.Finis;
            _reclam.fin_reclam = DateTime.Now;
            db.Entry(_reclam).State = EntityState.Modified;

            //get the traite object that where created by the Respo Dep find by reclam id 

            Traite _traite = db.Traites.Where(x=>x.Reclamation.id==idrec).FirstOrDefault();

            
            //set the date and the details
            _traite.date = DateTime.Now;
            _traite.detaille = detaille;
            

            //update
            db.Entry(_traite).State = EntityState.Modified;
            db.SaveChanges();
            apiservice.sendmail("Votre réclamation "+_traite.Reclamation.titre+" a été traité par l'agent"+_traite.agent.nom+" "+_traite.agent.prenom+"\n"+_traite.detaille, "Réclamation traité", _traite.Reclamation.Client.mail);
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
            List<Reclamation> _reclams = db.Reclamations.Include(x => x.Traite.agent).Where(x => x.etat == Etat.Finis && x.Traite.agent.id==id).ToList();
            return View(_reclams);
           
            
            }


        public ActionResult messages(int? id) {
         int myid = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(c => c.Type == "id").Value);
            Departement dep = db.Agents.Find(myid).departement;
            ViewBag.rd = db.Responsable_Departements.Include(x => x.receivedmessages).Include("receivedmessages.sentBy").Include("receivedmessages.sentTo").Include(s => s.sentmessages).Include("sentmessages.sentTo").Include("sentmessages.sentBy").Where(d=>d.departementId==dep.id).ToList();
         
            if (id != null) {
                 Responsable_departement _rd = db.Responsable_Departements.Find(id);
               if (_rd != null) {
                    ViewBag.msgs = db.Messages.Include(i => i.sentBy).Include(c => c.sentTo).
                    Where(x => (x.sentBy.id == id && x.sentTo.id == myid) || (x.sentTo.id == id && x.sentBy.id == myid)).ToList();
              
                

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