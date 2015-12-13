using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sistrategia.Drive.WebSite.Utils
{
    public static class RouteCollectionExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "This is a URL template with special characters, not just a regular valid URL.")]
        public static Route MapRouteToLocalizeRedirect(this RouteCollection routes, string name, string url, object defaults, object constraints) {
            var redirectRoute = new Route(url, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new LocalizationRedirectRouteHandler());
            routes.Add(name, redirectRoute);
            return redirectRoute;
        }

        public static Route MapRouteToLocalizeRedirect(this RouteCollection routes, string name, string url, object defaults) {
            var redirectRoute = new Route(url, new RouteValueDictionary(defaults), new LocalizationRedirectRouteHandler());
            routes.Add(name, redirectRoute);

            return redirectRoute;
        }

        public static Route MapLocalizeRoute(this RouteCollection routes, string name, string url, object defaults) {
            return routes.MapLocalizeRoute(name, url, defaults, new { });
        }

        public static Route MapLocalizeRoute(this RouteCollection routes, string name, string url, object defaults, object constraints) {
            var route = new Route(
                url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(constraints),
                new LocalizedRouteHandler());

            routes.Add(name, route);

            return route;
        }

        public static Route MapLocalizeRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces) {
            var route = new Route(
                url,
                new RouteValueDictionary(defaults),
                new RouteValueDictionary(constraints),
                new LocalizedRouteHandler());

            if (route.DataTokens == null)
                route.DataTokens = new RouteValueDictionary();
            route.DataTokens.Add("Namespaces", namespaces);

            routes.Add(name, route);

            return route;
        }
    }

    // https://github.com/mono/aspnetwebstack/blob/master/src/System.Web.Mvc/AreaRegistrationContext.cs

    public static class AreaRegistrationContextExtensions
    {
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
        //    Justification = "This is a URL template with special characters, not just a regular valid URL.")]
        //public static Route MapRouteToLocalizeRedirect(this AreaRegistrationContext context, string name, string url, object defaults) {
        //    var redirectRoute = new Route(url, new RouteValueDictionary(defaults), new LocalizationRedirectRouteHandler());
        //    //routes.Add(name, redirectRoute);
        //    context.Routes.Add(name, redirectRoute);

        //    return redirectRoute;
        //}

        public static Route MapLocalizeRoute(this AreaRegistrationContext context, string name, string url, object defaults) {
            return context.MapLocalizeRoute(name, url, defaults, new { });
        }

        public static Route MapLocalizeRoute(this AreaRegistrationContext context, string name, string url, object defaults, object constraints) {
            var route = new Route(
               url,
               new RouteValueDictionary(defaults),
               new RouteValueDictionary(constraints),
               new LocalizedRouteHandler());

            //route.DataTokens.Add("Namespaces", "Sistrategia.Drive.WebSite.Areas.Backstage.Controllers");
            if (route.DataTokens== null)
                route.DataTokens = new RouteValueDictionary();
            route.DataTokens.Add("Namespaces", context.Namespaces);
            route.DataTokens.Add("area", context.AreaName);
            route.DataTokens.Add("UseNamespaceFallback", true);
            // route.DataTokens.Add("Namespaces", "Sistrategia.Drive.WebSite.*");

            context.Routes.Add(name, route);

            return route;
        }
    }

    //public class AreaLocalizationRedirectRouteHandler : IRouteHandler
    //{
    //    public IHttpHandler GetHttpHandler(RequestContext requestContext) {
    //        var routeValues = requestContext.RouteData.Values;
    //        string cultureString = string.Empty;
    //        var cookieLocale = requestContext.HttpContext.Request.Cookies["locale"];
    //        if (cookieLocale != null) {
    //            if (string.IsNullOrEmpty(cookieLocale.Value))
    //                //routeValues["culture"] = CultureInfo.CurrentUICulture.Name;
    //                cultureString = CultureInfo.CurrentUICulture.Name;
    //            else
    //                //routeValues["culture"] = cookieLocale.Value;
    //                cultureString = cookieLocale.Value;
    //            var helper = new UrlHelper(requestContext); // "~/" +
    //            return new CustomRedirectHandler(@"/" + cultureString + helper.RouteUrl(routeValues));
    //        }
    //        var uiCulture = CultureInfo.CurrentUICulture;
    //        cultureString = uiCulture.Name;
    //        var helper2 = new UrlHelper(requestContext); // "~/" +
    //        return new CustomRedirectHandler(cultureString + helper2.RouteUrl(routeValues));
    //        ////if (uiCulture.Name.StartsWith("es-"))
    //        ////    routeValues["culture"] = "es-MX"; // intercept default es-ES for "es".
    //        ////else
    //        //routeValues["culture"] = uiCulture.Name;
    //        //return new CustomRedirectHandler(new UrlHelper(requestContext).RouteUrl(routeValues));
    //    }
    //}

}