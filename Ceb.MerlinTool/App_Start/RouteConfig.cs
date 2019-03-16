using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ceb.MerlinTool
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Account", action = "About" },
                 namespaces: new string[] { "Ceb.BigNumberBM.Web.Controllers" }
            );

            routes.MapRoute(
               name: "Default",
               url: "Account/Login",
               defaults: new { controller = "Account", action = "Login" },
               namespaces: new string[] { "Ceb.BigNumberBM.Web.Controllers" }
           );

            routes.MapRoute(
                name: "SBWS",
                url: "SBWSAuth/Login",
                defaults: new { controller = "SBWSAuth", action = "Login" },
                namespaces: new string[] { "CEB.CustomControls.Controllers" }
           );

            routes.MapRoute(
                name: "all",
                 url: "{controller}/{action}",
                 defaults: new { controller = "Account", action = "Login" },
                  namespaces: new string[] { "Ceb.BigNumberBM.Web.Controllers" }
                );
        }
    }
}
