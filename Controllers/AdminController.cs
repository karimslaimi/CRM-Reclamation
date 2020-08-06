using PFE_reclamation.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PFE_reclamation.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [CustomAuthorize("ADMIN")]
        public ActionResult Index()
        {
           

           

            return View();
        }
    }
}