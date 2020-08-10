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
            Agent _rrc = db.Agents.FirstOrDefault(x => x.id == id);



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
            Reclamation _reclam = db.Reclamations.Find(idrec);
            _reclam.etat = Etat.Finis;
            _reclam.fin_reclam = DateTime.Now;
            db.Entry(_reclam).State = EntityState.Modified;


            Traite _traite = new Traite();
            int ida = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(i => i.Type == "id").Value);
            _traite.agent = db.Agents.Where(x => x.id == ida).FirstOrDefault();
            _traite.date = DateTime.Now;
            _traite.detaille = detaille;
            _traite.reclamation = _reclam;
            db.Traites.Add(_traite);
            db.SaveChanges();
            return Redirect("reclams");

            }



        }
    }