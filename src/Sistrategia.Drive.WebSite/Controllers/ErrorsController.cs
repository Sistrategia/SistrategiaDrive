using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistrategia.Drive.WebSite.Controllers
{
    public class ErrorsController : Controller
    {
        //// GET: Errors
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult General(Exception exception) {
            // return Content("General failure", "text/plain");
            return View("Error");
        }

        public ActionResult Http404() {
            //return Content("Not found", "text/plain");
            //Response.StatusCode
            return View("Error");
        }

        public ActionResult Http403() {
            return Content("Forbidden", "text/plain");
        }
    }
}