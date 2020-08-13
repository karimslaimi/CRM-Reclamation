using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Security;
using PFE_reclamation.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PFE_reclamation.Controllers
{
    public class HomeController : Controller
    {
        DatabContext db = new DatabContext();

       
        public ActionResult Index()
        {
            
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("redirectrole", "Users");
            }
            else
            {
                return RedirectToAction("Signin", "Users");
            }
           
            return View();
             

           

             
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

            
        public ActionResult Unauthorized()
        {
            return View();

        }



        public ActionResult persist()
        {
            ///this action to persist data without haveing to type in form
            //Authentication authservice = new Authentication();
            //Responsable_departement _ad = new Responsable_departement();
            //_ad.id = 5;
            //_ad.mail = "ghassen@gmail.com";
            //_ad.nom = "sami";
            //_ad.prenom = "sami";
            //_ad.tel = "25415633";
            //_ad.username = "ghassen";
            //_ad.cin = "11654555";
            //_ad.password = authservice.HashPassword("ghassen123");
            //_ad.date_aff = DateTime.Today;
            //_ad.departement = db.Departements.Find(1);
            //db.Responsable_Departements.Add(_ad);
            //db.SaveChanges();



            //Departement _dep = new Departement();
            //_dep.label = "Financiére";
            //db.Departements.Add(_dep); 
            //Departement _dep1 = new Departement();
            //_dep.label = "Voiture";
            //db.Departements.Add(_dep1); 
            //Departement _dep2 = new Departement();
            //_dep.label = "Immobillier";
            //List<Departement> lsdep = new List<Departement>();
            //lsdep.Add(_dep);
            //lsdep.Add(_dep1);
            //lsdep.Add(_dep2);

            //db.Departements.AddRange(lsdep);

            // db.SaveChanges();




            return Redirect("index");

        }


    }
}