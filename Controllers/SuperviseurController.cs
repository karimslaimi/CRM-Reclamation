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
    public class SuperviseurController : Controller
    {
        // GET: Superviseur
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Responsable_departement responsable)
        {
            DatabContext db = new DatabContext();
            if (ModelState.IsValid)
            {
                Authentication authservice = new Authentication();
                responsable.password = authservice.HashPassword(responsable.password);
                db.Responsable_Departements.Add(responsable);
                db.SaveChanges();
                return RedirectToAction("responsables");
            }
            return View(responsable);
        }

        public ActionResult responsables()
        { DatabContext db = new DatabContext();
            List<Responsable_departement> rs = db.Responsable_Departements.ToList();

            return View(rs);
        }

        public ActionResult delete(int id)
        { DatabContext db = new DatabContext();
            Responsable_departement rs = db.Responsable_Departements.Find(id);
            db.Responsable_Departements.Remove(rs);
            db.SaveChanges();
            return RedirectToAction("responsables");

        }
        public ActionResult Details(int id)
        {
            DatabContext db = new DatabContext();
            Responsable_departement rs = db.Responsable_Departements.Find(id);

            return View(rs); 
        }
    }
}