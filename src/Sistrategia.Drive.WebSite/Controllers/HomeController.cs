using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistrategia.Drive.WebSite.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated) {
                return RedirectToAction("Welcome");
            }

            return View();
        }

        //[OutputCache(Duration = 60, VaryByParam="*")]
        public ActionResult Welcome() {
            Sistrategia.Drive.WebSite.Views.Home.Welcome.Title.ToString();
            return View();
        }

        public ActionResult ChangeLang(string lang, string returnUrl) {
            var langCookie = new HttpCookie("locale", lang) { HttpOnly = true };
            Response.AppendCookie(langCookie);
            //if (string.IsNullOrEmpty(returnUrl))
            //    return RedirectToAction("Index");
            return Redirect(HttpUtility.UrlDecode(returnUrl));
        }

        public ActionResult Cover() {
            return View();
        }
    }
}