using Sistrategia.Drive.WebSite.Areas.Dev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistrategia.Drive.WebSite.Areas.Dev.Controllers
{
    [Authorize(Roles = "Developer")]
    public class HomeController : Controller
    {
        // GET: Dev/Home
        public ActionResult Index()
        {
            var model = new DevIndexViewModel(HttpContext.GetOwinContext());
            model.Title = "Developer Control Panel";
            return View(model);
        }
    }
}