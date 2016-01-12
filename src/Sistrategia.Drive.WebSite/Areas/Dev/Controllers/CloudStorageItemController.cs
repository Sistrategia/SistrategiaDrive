using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.Drive.Business;
using Sistrategia.Drive.WebSite.Areas.Dev.Models;
using System.Threading.Tasks;

namespace Sistrategia.Drive.WebSite.Areas.Dev.Controllers
{
    [Authorize(Roles = "Developer")]
    public class CloudStorageItemController : Sistrategia.Drive.WebSite.Controllers.BaseController
    {
        //// GET: Dev/CloudStorageItem
        //public ActionResult Index()
        //{
        //    return View();
        //}


        public ActionResult Details(string id) {

            ApplicationDbContext context = new ApplicationDbContext();
            // var item = context.CloudStorageItems.Find(id);

            var cid = Guid.Parse(id);
            var item = context.CloudStorageItems.SingleOrDefault(c => c.PublicKey == cid);
            var blob = CloudStorageMananger.GetStorageItem(
                item.CloudStorageContainer.CloudStorageAccount.AccountName,
                item.CloudStorageContainer.CloudStorageAccount.AccountKey,
                item.CloudStorageContainer.ContainerName,
                item.ProviderKey
            );

            if (item.ContentType.StartsWith("image/")) {

            }
           
            var model = new CloudStorageItemDetailsViewModel {
                CloudStorageItem = item,
                Url = blob.Url,
                IsImage = item.ContentType.StartsWith("image/")
            };

            return View(model);
            
        }

    }
}