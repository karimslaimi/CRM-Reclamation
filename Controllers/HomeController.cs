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
            //Responsable_relation_client _ad = new Responsable_relation_client();
            //_ad.mail = "karimrrc@gmail.com";
            //_ad.nom = "slaimi";
            //_ad.prenom = "karim";
            //_ad.tel = "25415633";
            //_ad.username = "karimrrc";
            //_ad.cin = "11654521";
            //_ad.password = authservice.HashPassword("karim123");

            //db.Responsable_Relation_Clients.Add(_ad);
            //db.SaveChanges();



            Departement _dep = new Departement();
            _dep.label = "Financiére";
            db.Departements.Add(_dep); 
            Departement _dep1 = new Departement();
            _dep.label = "Voiture";
            db.Departements.Add(_dep1); 
            Departement _dep2 = new Departement();
            _dep.label = "Immobillier";
            db.Departements.Add(_dep2);

             db.SaveChanges();




            return Redirect("index");

        }


    }
}