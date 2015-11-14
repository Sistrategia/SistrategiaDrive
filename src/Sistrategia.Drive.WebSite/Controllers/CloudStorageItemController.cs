using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.Drive.Business;
using Sistrategia.Drive.WebSite.Models;

namespace Sistrategia.Drive.WebSite.Controllers
{
    public class CloudStorageItemController : BaseController
    {
        // GET: CloudStorageItem
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(string id) {

            ApplicationDbContext context = new ApplicationDbContext();
            var item = context.CloudStorageItems.Find(id);
            //var user = this.CurrentSecurityUser;
            //if (user != null) {
            //var account = user.CloudStorageAccounts.SingleOrDefault(a => a.CloudStorageAccountId == id);
            var model = new CloudStorageItemDetailViewModel {
                CloudStorageItem = item
            };
            
            return View(model);
            //}
            //return View();

            return View();
        }
    }
}