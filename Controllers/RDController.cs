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
                Authentication authservice = new Authentication();
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
            DatabContext db = new DatabContext();
            List<Agent> rs = db.Agents.ToList();

            return View(rs);
        }

        // supprimer un agent
        public ActionResult deleteAgent(int id)
        {
            DatabContext db = new DatabContext();
            Agent rs = db.Agents.Find(id);
            db.Agents.Remove(rs);
            db.SaveChanges();
            return RedirectToAction("agents");

        }
        // methode pour  affichage des details d'un agent
        public ActionResult detailsAgent(int id)
        {
            DatabContext db = new DatabContext();
            Agent rs = db.Agents.Find(id);

            return View(rs);
        }



    }
}