using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sistrategia.Drive.WebSite.Utils;

namespace Sistrategia.Drive.WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{culture}/{controller}/{action}/{id}",
            //    defaults: new { culture="en-US", controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapLocalizeRoute(
                name: "Default",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { culture = "[a-zA-Z]{2}-[a-zA-Z]{2}" },
                namespaces: new[] { "Sistrategia.Drive.WebSite.Controllers" });      
      

            // Las rutas de redirect tienen que ir hasta abajo, por eso no se pueden pasar a sus secciones de areas aunque existan            

            routes.MapRouteToLocalizeRedirect("RedirectToLocalizeArea",
                      url: "Backstage/{controller}/{action}/{id}",
                      defaults: new { area = "Backstage", controller = "Home", action = "Index", id = UrlParameter.Optional });

            // Primero las que van a convertir las áraes agregar cada una probablemente se pueda resolver con un constraint Backstage|bla
            //routes.MapRouteToLocalizeRedirect("RedirectToLocalizeArea",
            //          url: "{area}/{controller}/{action}/{id}",
            //          defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //          constraints: new { area = "Backstage|Admin" });

            routes.MapRouteToLocalizeRedirect("RedirectToLocalize",
                      url: "{controller}/{action}/{id}",
                      defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            

            
        }
    }
}