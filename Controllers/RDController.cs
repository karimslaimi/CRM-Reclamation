
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

namespace PFE_reclamation.Controllers
{
    [CustomAuthorize("RD")]
    public class RDController : Controller
    {
        DatabContext db = new DatabContext();
        Authentication authservice = new Authentication();
        // GET: RD
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult profile() {
            //get the userid from claims principal where we stored the user data after login
            string userid = ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value;
            int id = int.Parse(userid);
            Responsable_departement _rd = db.Responsable_Departements.FirstOrDefault(x => x.id == id);



            if (TempData["error"] != null) {
                ViewBag.passerr = TempData["error"];
                }
            if (TempData["msg"] != null) {
                ViewBag.passmsg = TempData["msg"];
                }
            return View(_rd);



            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult profile(Responsable_departement _rd) {

            //we get the object so we can fill the fields left empty
            Responsable_departement rd = db.Responsable_Departements.AsNoTracking().FirstOrDefault(x => x.id == _rd.id);

            _rd.password = rd.password;



            if (ModelState.IsValid) {
                try {




                    db.Entry(_rd).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.msg = "Profile modifié avec succés";
                    } catch (Exception e) {
                    ViewBag.error = "Erreur est survenu réessayer";
                    }
                } else {
                ViewBag.error = "Erreur est survenu réessayer";
                }


            return View(_rd);



            }


        public ActionResult passwordchange(string pass, string cpass, int id) {

            if (pass.Equals(cpass) && !String.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(pass)) {
                Responsable_departement _rd = db.Responsable_Departements.Find(id);
                _rd.password = authservice.HashPassword(pass);
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["error"] = "les mots de passe ne correspondent pas";
                return Redirect("profile");
                }



            return Redirect("profile");
            }



        // créer un agent
        [HttpGet]
        public ActionResult newAgent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult newAgent(Agent agent)
        {
        
            if (ModelState.IsValid)
            {
                int idresp = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
                Responsable_departement _rd = db.Responsable_Departements.Find(idresp);
                agent.departement = db.Departements.Where(x => x.id == _rd.departementId).FirstOrDefault();
                agent.password = authservice.HashPassword(agent.password);
                db.Agents.Add(agent);
                db.SaveChanges();
                return RedirectToAction("agents");
            }
            return View(agent);
        }

        // afficher la liste des agents
        public ActionResult agents()
        {
            string userid = ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value;
            int id = int.Parse(userid); 
         Responsable_departement _rs  = db.Responsable_Departements.Find(id);
            List<Agent> rs = db.Agents.Where(x => x.departementId == _rs.departementId).ToList();

            return View(rs);
        }

        // supprimer un agent
        public ActionResult deleteAgent(int id)
        {
   
            Agent rs = db.Agents.Find(id);
            db.Agents.Remove(rs);
            db.SaveChanges();
            return RedirectToAction("agents");

        }
        // methode pour  affichage des details d'un agent
        public ActionResult detailsAgent(int id)
        {
          
            Agent rs = db.Agents.Find(id);

            return View(rs);
        }

        [HttpGet]
        public ActionResult editAgent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Agent _rs = db.Agents.FirstOrDefault(x => x.id == id);
            if (_rs == null)
            {
                return HttpNotFound();
            }
            return View(_rs);
        }

        // POST: Users/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editAgent(Agent _rs)
        {
            Agent _rd = db.Agents.AsNoTracking().FirstOrDefault(x => x.id == _rs.id);

            _rs.password = _rd.password;


            if (ModelState.IsValid)
            {
                db.Entry(_rs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("agents");
            }
            return View(_rs);
        }


        //get the reclams that were sent to his departement
        public ActionResult reclams() {

            int id = int.Parse(ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value);
            Responsable_departement _rd = db.Responsable_Departements.Include(x=>x.departement).Where(x=>x.id==id).FirstOrDefault();

            List<Reclamation> _reclams = db.Reclamations.Where(x => x.Departement.id == _rd.departement.id).ToList();

            List<Agent> _agents = db.Agents.Where(x => x.departement.id == _rd.departement.id).ToList();
            ViewBag.agents = _agents;
            return View(_reclams);


            }

        public ActionResult affecte_reclam(int idrec,int idage) {
            Traite _traite = new Traite();
            Reclamation _reclam = db.Reclamations.Find(idrec);
            Agent _agent = db.Agents.Find(idage);
            _traite.agent = _agent;
            _traite.Reclamation = _reclam;
            _traite.detaille = "";
            _traite.date = DateTime.Now;
            db.Traites.Add(_traite);
            db.SaveChanges();
            return Redirect("reclams");


            }

     


    }
}