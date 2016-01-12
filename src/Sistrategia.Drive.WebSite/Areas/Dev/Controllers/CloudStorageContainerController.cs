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
    public class CloudStorageContainerController : Sistrategia.Drive.WebSite.Controllers.BaseController
    {
        //// GET: Dev/CloudStorageContainer
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Details(string id) {            
            var cid = Guid.Parse(id);
            var container = DBContext.CloudStorageContainers.SingleOrDefault(c => c.PublicKey == cid);

           // var blobs = CloudStorageMananger.ImportStorageItems(container.CloudStorageAccount.ProviderKey, container.CloudStorageAccount.AccountKey, container.ProviderKey); // .GetContainers(account.AccountName, account.AccountKey);
            //var currentItems = container.CloudStorageItems.ToList();

            //foreach (var blob in blobs) {
            //    //container.CloudStorageAccountId = 
            //    //if (container.CloudStorageItems.SingleOrDefault(i => i.ProviderKey == blob.ProviderKey) == null)
            //    if (currentItems.SingleOrDefault(i => i.ProviderKey == blob.ProviderKey) == null)
            //        container.CloudStorageItems.Add(blob);
            //}
            
            var model = new CloudStorageContainerDetailsViewModel {
                CloudStorageContainer = container,
                //blobs = CloudStorageMananger.ImportStorageItems(container.CloudStorageAccount.ProviderKey, container.CloudStorageAccount.AccountKey, container.ProviderKey); // .GetContainers(account.AccountName, account.AccountKey);

            };
            return View(model);
        }



        public ActionResult Sync(string id) {

            ApplicationDbContext context = new ApplicationDbContext();
            //var container = context.CloudStorageContainers.Find(Guid.Parse(id).ToString("D").ToLower());
            var cid = Guid.Parse(id);
            var container = context.CloudStorageContainers.SingleOrDefault(c => c.PublicKey == cid);

            //var user = this.CurrentSecurityUser;
            //if (user != null) {
            //var account = user.CloudStorageAccounts.SingleOrDefault(a => a.CloudStorageAccountId == id);
            //var model = new CloudStorageContainerDetailViewModel {
            //    CloudStorageContainer = container
            //};
            System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary();
            dict.Add("id", id);

            var blobs = CloudStorageMananger.ImportStorageItems(container.CloudStorageAccount.ProviderKey, container.CloudStorageAccount.AccountKey, container.ProviderKey); // .GetContainers(account.AccountName, account.AccountKey);

            foreach (var blob in blobs) {
                ////if (string.IsNullOrEmpty(blob.OwnerId))
                //if (blob.OwnerId == 0)
                if (container.CloudStorageItems.SingleOrDefault(i => i.ProviderKey == blob.ProviderKey) == null)
                    container.CloudStorageItems.Add(blob);
            }
            context.SaveChanges();

            return RedirectToAction("Details", dict); // , new { Id = id });
        }


    }
}