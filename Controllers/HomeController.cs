using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Security;
using System;
using System.Collections.Generic;
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

            Admin ad = new Admin();
            ad.cin = "00000000";
            ad.mail = "admin@admin.com";
            ad.nom = "admin";
            ad.prenom = "adminos";
            ad.tel = "700000";
            ad.username = "admin";
            ad.password = "karim123";
            db.Admins.AddOrUpdate(ad);
            db.SaveChanges();

            return Redirect("index");

        }


    }
}