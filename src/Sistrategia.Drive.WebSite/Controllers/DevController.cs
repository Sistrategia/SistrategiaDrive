using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Threading.Tasks;
//using Microsoft.Owin.Security;
//using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Sistrategia.Drive.Business;
using Sistrategia.Drive.WebSite.Models;

namespace Sistrategia.Drive.WebSite.Controllers
{
    [Authorize(Roles = "Developer")]
    public class DevController : Controller
    {
        // GET: Dev        
        public ActionResult Index() {
            var model = new DevIndexViewModel(HttpContext.GetOwinContext());
            model.Title = "Developer Control Panel";
            return View(model);
        }

        public ActionResult CreateDatabase() {
            //Sistrategia.Drive.Data.DatabaseManager.CreateDatabase();
            return Redirect("~/Dev");
        }

        public ActionResult CreateSchema() {
            //Sistrategia.Drive.Data.DatabaseManager.CreateSchema();
            return Redirect("~/Dev");
        }

        public ActionResult DropSchema() {
            //Sistrategia.Drive.Data.DatabaseManager.DropSchema();
            return Redirect("~/Dev");
        }

        public ActionResult Users() {
            var model = new DevUsersViewModel(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
            model.Title = "Users";
            return View(model);
        }
    }
}