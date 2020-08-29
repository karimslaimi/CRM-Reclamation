using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PFE_reclamation
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {    
            AntiForgeryConfig.UniqueClaimTypeIdentifier = "id";
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        
            }
        protected void Application_Error() {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = "General";
            routeData.Values["exception"] = exception;
            Response.StatusCode = 500;
            if (httpException != null) {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode) {
                    case 403:
                        routeData.Values["action"] = "Http403";
                        Response.Redirect("/home/Error/Http403");
                            break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        Response.Redirect("/home/Error/Http404");
                        break;
                    default:
                        Response.Redirect("/home/Error/exception");
                        break;

                    }
                }

            }
        }
}
