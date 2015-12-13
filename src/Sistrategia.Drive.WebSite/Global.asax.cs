using System;
using System.Web;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Security;
//using System.Web.SessionState;
//using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sistrategia.Drive.WebSite
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e) {
            AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Session_Start(object sender, EventArgs e) {

        //}

        //protected void Application_BeginRequest(object sender, EventArgs e) {

        //}

        //protected void Application_AuthenticateRequest(object sender, EventArgs e) {

        //}

        protected void Application_Error(object sender, EventArgs e) {
            if (Context.IsCustomErrorEnabled)
                ShowCustomErrorPage(Server.GetLastError());
        }

        private void ShowCustomErrorPage(Exception exception) {
            //var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();            
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
                        break;
                    case 404:
                        routeData.Values["action"] = "Http404";
                        break;
                }
            }
            Server.ClearError();

            IController errorsController = new Sistrategia.Drive.WebSite.Controllers.ErrorsController();
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            errorsController.Execute(rc);
        }

        //protected void Session_End(object sender, EventArgs e) {

        //}

        //protected void Application_End(object sender, EventArgs e) {

        //}
    }
}