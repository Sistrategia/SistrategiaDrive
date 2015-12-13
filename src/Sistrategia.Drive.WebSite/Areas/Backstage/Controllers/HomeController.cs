using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistrategia.Drive.WebSite.Areas.Backstage.Controllers
{
    [Authorize(Roles="Backstage")]
    public class HomeController : Controller
    {
        // GET: Backstage/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}