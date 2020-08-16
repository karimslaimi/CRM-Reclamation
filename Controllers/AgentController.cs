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
            List<Reclamation> _reclams = db.Reclamations.Where(x => x.Traite.agent.id == id && x.etat == Etat.Finis).ToList();
            return View(_reclams);
           
            
            }


        public ActionResult messages() {
            // to contact his respo dep

            int ida = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
            Agent agent = db.Agents.Where(x => x.id == ida).FirstOrDefault();

            Responsable_departement _rd = db.Responsable_Departements.Where(x => x.departement.id == agent.departement.id).FirstOrDefault();

            List<Message> _messages = db.Messages.Where(x => x.sentTo.id == _rd.id && x.sentBy.id == ida).OrderBy(x=>x.date).ToList();
            return View(_messages);


            }

        public ActionResult sendmessage(string content) {
            int ida = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
            Agent agent = db.Agents.Where(x => x.id == ida).FirstOrDefault();
            Responsable_departement _rd = db.Responsable_Departements.Where(x => x.departement.id == agent.departement.id).FirstOrDefault();
            Message _message = new Message();
            _message.content = content;
            _message.date = DateTime.Now;
            _message.sentBy = agent;
            _message.sentTo = _rd;
            db.Messages.Add(_message);
            db.SaveChanges();
            return Redirect("messages");
            


            }



        }
    }