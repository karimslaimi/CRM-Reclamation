using Microsoft.Ajax.Utilities;
using Microsoft.Owin;
using PFE_reclamation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PFE_reclamation.Security
{
    

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class CustomAuthorizeAttribute : AuthorizeAttribute
        {


        private string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] Roles)
        {
            this.allowedroles = (string[])Roles.Clone();
        }


        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {




            var ctx = HttpContext.Current.GetOwinContext();

            IEnumerable<Claim> claims = ClaimsPrincipal.Current.Claims;

            bool authorize = false;


            if (HttpContext.Current.User.IsInRole(allowedroles[0]))
            {
                authorize = true;
            }

          
            
        /*    User _user = su.Get(x => x.mail == userid);


            if (_user != null && Roles.Contains(_user.type))
            {
                authorize = true;
            }
        */




            return authorize;
        }




        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    filterContext.Result = new ViewResult()
            //    {
            //        ViewName = "~/Home/Unauthorized"
            //    };
            //}

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new
                 RouteValueDictionary(new { controller = "Users", action = "login" }));
            }
            // filterContext.Result = new HttpUnauthorizedResult();

            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                 RouteValueDictionary(new { controller = "Home", action = "Unauthorized" }));
            }
        }

    }

    }
 