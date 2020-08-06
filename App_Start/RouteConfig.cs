using System.Web.Mvc;
using System.Web.Routing;

namespace PFE_reclamation
    {
    public class RouteConfig
        {
        public static void RegisterRoutes(RouteCollection routes)
            {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
               name: "Login",
                url: "login",
               defaults: new { controller = "Users", action = "Signin" }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );



             

            }
        }
    }
