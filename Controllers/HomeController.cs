﻿using PFE_reclamation.DAL;
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
        Authentication authservice = new Authentication();

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



            //Admin _admin = new Admin { cin = "11445566", mail = "admin@gmail.com", nom = "adminos", prenom = "admin", tel = "51447788", username = "admin", password = authservice.HashPassword("karim123") };
            //db.Admins.Add(_admin);

            ////Responsable_departement _ad = new Responsable_departement();
            ////_ad.mail = "karimrd@gmail.com";
            ////_ad.nom = "slaimi";
            ////_ad.prenom = "karim";
            ////_ad.tel = "25415633";
            ////_ad.username = "karimrd";
            ////_ad.cin = "11654555";
            ////_ad.password = authservice.HashPassword("karim123");
            ////_ad.date_aff = DateTime.Today;
            ////_ad.departement = db.Departements.Find(1);
            ////db.Responsable_Departements.Add(_ad);
            //// db.SaveChanges();
            //IList<Client> _clients = new List<Client>();

            //_clients.Add(new Client() { cin = "11335566", mail = "karimc@gmail.com", nom = "client", prenom = "karim", tel = "51667788", username = "karimc", password = authservice.HashPassword("karim123") });
            //_clients.Add(new Client() { cin = "11225566", mail = "monjic@gmail.com", nom = "client", prenom = "monji", tel = "51777788", username = "monjic", password = authservice.HashPassword("karim123") });

            //db.Clients.AddRange(_clients);

            //db.SaveChanges();
            //IList<Departement> _deps = new List<Departement>();
            //db.Departements.Add(new Departement() { label = "Financiére" });
            //db.SaveChanges();
            //db.Departements.Add(new Departement() { label = "Immobilier" });
            //db.SaveChanges();
            //db.Departements.Add(new Departement() { label = "Immobillier" });
         
            //db.SaveChanges();

            //Departement dp = db.Departements.Find(1);

            //Responsable_departement _rd = new Responsable_departement() { cin = "15487963", mail = "karimrd@gmail.com", nom = "rd", prenom = "karim", departementId=dp.id ,tel = "51676788", username = "karimrd", password = authservice.HashPassword("karim123"), date_aff = DateTime.Now };

            //db.Responsable_Departements.Add(_rd);
            //db.SaveChanges();
           


            return Redirect("index");

        }


    }
}