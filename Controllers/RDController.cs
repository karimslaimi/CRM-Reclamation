using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PFE_reclamation.Controllers
{
    public class RDController : Controller
    {
        // GET: RD
        public ActionResult Index()
        {
            return View();
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
            DatabContext db = new DatabContext();
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