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
            /*
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("redirectrole", "Users");
            }
            else
            {
                return RedirectToAction("Signin", "Users");
            }
            */
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
            Authentication authservice = new Authentication();
            Superviseur _ad = new Superviseur();
            _ad.mail = "sami@sami.com";
            _ad.nom = "sami";
            _ad.prenom = "krimi";
            _ad.tel = "25415633";
            _ad.username = "sami";
            _ad.cin = "11654521";
            _ad.password = authservice.HashPassword("sami123");
            _ad.date_aff = DateTime.Now;
            db.Superviseurs.Add(_ad);
            db.SaveChanges();



            return Redirect("index");

        }


    }
}