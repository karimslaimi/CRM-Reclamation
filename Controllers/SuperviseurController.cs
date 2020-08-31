using PFE_reclamation.DAL;
using PFE_reclamation.Models;
using PFE_reclamation.Security;
using PFE_reclamation.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PFE_reclamation.Controllers
{
    [CustomAuthorize("SUPERVISEUR")]
    public class SuperviseurController : Controller
    {
        DatabContext db = new DatabContext();
        Authentication authservice = new Authentication();
        ApiService apiservice = new ApiService();
        // GET: Superviseur
        public ActionResult Index()
        {


            List<Reclamation> _reclams = db.Reclamations.ToList();


            ViewBag.traitereclam = _reclams.Where(x => x.etat == Etat.Traite).Count();
            ViewBag.encourreclam = _reclams.Where(x => x.etat == Etat.En_cours).Count();
            ViewBag.nbreclam = _reclams.Count();
            ViewBag.newreclam = _reclams.Where(x=>x.etat==Etat.Nouveau).Count();
            ViewBag.clients = db.Clients.Include(s=>s.Reclamations).Where(x=>x.Reclamations.Count()>0).OrderBy(x => x.Reclamations.Count()).Take(5).ToList();

            ViewBag.lastreclams =db.Reclamations.Where(x=>x.etat==Etat.Nouveau).OrderBy(s=>s.debut_reclam).Take(5);
            return View();
        }



        protected bool verifyFiles(HttpPostedFileBase item) {
            bool flag = true;

            if (item != null) {
                if (item.ContentLength > 0 && item.ContentLength < 5000000) {
                    if (!(Path.GetExtension(item.FileName).ToLower() == ".jpg" ||
                        Path.GetExtension(item.FileName).ToLower() == ".png" ||
                        Path.GetExtension(item.FileName).ToLower() == ".jpeg")) {
                        flag = false;
                        }
                    } else { flag = false; }
                } else { flag = false; }

            return flag;
            }



        [HttpGet]
        public ActionResult profile() {
            //get the userid from claims principal where we stored the user data after login
            string userid = ClaimsPrincipal.Current.Claims.FirstOrDefault(x => x.Type == "id").Value;
            int id = int.Parse(userid);
            Superviseur _superviseur = db.Superviseurs.FirstOrDefault(x => x.id == id);



            if (TempData["error"] != null) {
                ViewBag.passerr = TempData["error"];
                }
            if (TempData["msg"] != null) {
                ViewBag.passmsg = TempData["msg"];
                }
            return View(_superviseur);



            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult profile(Superviseur _superviseur, HttpPostedFileBase postedFile) {

            //we get the object so we can fill the fields left empty
            Superviseur superviseur = db.Superviseurs.AsNoTracking().FirstOrDefault(x => x.id == _superviseur.id);

            _superviseur.password = superviseur.password;
            _superviseur.date_aff = superviseur.date_aff;

            ModelState.Remove("password");

            if (ModelState.IsValid) {
                try {

                    if (postedFile != null && verifyFiles(postedFile)) {
                        string path = Server.MapPath("/Content/images/");
                        if (!Directory.Exists(path)) {
                            Directory.CreateDirectory(path);
                            }

                        if (System.IO.File.Exists(Path.GetFullPath(path + "profile_" + _superviseur.id + Path.GetExtension(postedFile.FileName))))
                            System.IO.File.Delete(path + "profile_" + _superviseur.id + Path.GetExtension(postedFile.FileName));

                        postedFile.SaveAs(path + "profile_" + _superviseur.id + Path.GetExtension(postedFile.FileName));
                        _superviseur.photo = Path.GetFileName("profile_" + _superviseur.id + Path.GetExtension(postedFile.FileName));
                        }



                    db.Entry(_superviseur).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.msg = "Profile modifié avec succés";
                    } catch (Exception e) {
                    ViewBag.error = "Erreur est survenu réessayer";
                    }
                } else {
                ViewBag.error = "Erreur est survenu réessayer";
                }


            return View(_superviseur);



            }


        public ActionResult passwordchange(string pass, string cpass, int id) {

            if (pass.Equals(cpass) && !String.IsNullOrEmpty(pass) && !string.IsNullOrWhiteSpace(pass)) {
                Superviseur _superviseur = db.Superviseurs.Find(id);
                _superviseur.password = authservice.HashPassword(pass);
                TempData["msg"] = "Mot de passe a été modifié";
                } else {
                TempData["error"] = "les mots de passe ne correspondent pas";
                return Redirect("profile");
                }



            return Redirect("profile");
            }



        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Responsable_departement responsable)
        {
   
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
        {  
            List<Responsable_departement> rs = db.Responsable_Departements.ToList();

            return View(rs);
        }

        public ActionResult delete(int id)
        {  
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



        public ActionResult reclams() {

            List<Reclamation> _reclams = db.Reclamations.ToList();
            ViewBag.deps = db.Responsable_Departements.Select(s=>s.departement).ToList();
            return View(_reclams);


            }
        public ActionResult encours_reclams() {

            List<Reclamation> _reclams = db.Reclamations.Where(x=>x.Departement==null && x.etat==Etat.En_cours).ToList();
            ViewBag.deps = db.Responsable_Departements.Select(s => s.departement).ToList();
            return View(_reclams);


            }

        public ActionResult traite_reclams() {
            List<Reclamation> _reclams = db.Reclamations.Include(x => x.Traite.agent).Where(x => x.etat == Etat.Traite).ToList();

            return View(_reclams);
            }



        public ActionResult valider_reclam(int idr,int iddep,string returnurl) {

           
            Reclamation _reclam = db.Reclamations.Find(idr);
            if (_reclam.Departement == null) {
                Departement _dep = db.Departements.Find(iddep);
                Responsable_departement _rd = db.Responsable_Departements.Where(x => x.departementId == iddep).FirstOrDefault();
                _reclam.Departement = _dep;
                _reclam.DepartementId = iddep;
                db.Entry(_reclam).State = EntityState.Modified;
                db.SaveChanges();
                apiservice.sendmail("Une réclamation intitulé " + _reclam.titre + " a été affecté à votre département", "Réclamation validé", _rd.mail);
                }
            return Redirect(returnurl);


            }
        public ActionResult deleteReclam(int? id) {
            if (id == null) {
                return RedirectToAction("reclams");
                } else {
                Reclamation _reclam = db.Reclamations.Find(id);
                if (_reclam.etat == Etat.Nouveau) {
                    db.Reclamations.Remove(db.Reclamations.Find(id));
                    db.SaveChanges();
                    }

                return RedirectToAction("reclams");
                }
            }



        }
}