using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistrategia.Drive.WebSite.Controllers
{
    [Authorize( Roles = "Developer")]
    public class DevController : Controller
    {
        // GET: Dev        
        public ActionResult Index()
        {
            this.ViewBag.AcceptLanguage = HttpContext.Request.Headers["Accept-Language"];
            return View();
        }

        public ActionResult ChangeLang(string lang, string returnUrl) {
            var langCookie = new HttpCookie("locale", lang) { HttpOnly = true };
            Response.AppendCookie(langCookie);
            return Redirect(HttpUtility.UrlDecode(returnUrl));
        }
    }
}