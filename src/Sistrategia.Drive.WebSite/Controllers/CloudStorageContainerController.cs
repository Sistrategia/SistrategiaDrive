﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.Drive.Business;
using Sistrategia.Drive.WebSite.Models;

using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace Sistrategia.Drive.WebSite.Controllers
{
    public class CloudStorageContainerController : BaseController
    {
        public CloudStorageContainerController() {
        }

        public CloudStorageContainerController(SecurityUserManager userManager, SecuritySignInManager signInManager)
            : base(userManager, signInManager) {
        }

        // GET: CloudStorageContainer
        public ActionResult Index() {
            ApplicationDbContext context = new ApplicationDbContext();

            var user = context.Users.Find(User.Identity.GetUserId());
            var account = user.CloudStorageAccounts.FirstOrDefault();
            var container = account.CloudStorageContainers.FirstOrDefault();

            // CloudStorageMananger.GetBlobs();

            CloudStorageMananger storage = new CloudStorageMananger();
            var itemList = storage.GetCloudStorageItems();

            CloudStorageContainerListModel model = new CloudStorageContainerListModel {
                Container = container,
                CloudStorageItems = itemList // blobNames
            };

            return View(model);
        }

        public ActionResult Detail(string id) {

            ApplicationDbContext context = new ApplicationDbContext();
            var container = context.CloudStorageContainers.Find(id);
            //var user = this.CurrentSecurityUser;
            //if (user != null) {
            //var account = user.CloudStorageAccounts.SingleOrDefault(a => a.CloudStorageAccountId == id);
            var model = new CloudStorageContainerDetailViewModel {
                CloudStorageContainer = container
            };
            return View(model);
            //}
            //return View();

            return View();
        }

        public ActionResult Create(string id) {

            ApplicationDbContext context = new ApplicationDbContext();
            //var providers = context.CloudStorageProviders.ToList();
            var account = context.CloudStorageAccounts.Find(id);
            var containers = CloudStorageMananger.GetContainers(account.AccountName, account.AccountKey);

            var model = new CloudStorageContainerCreateViewModel {
                CloudStorageAccountId = id,
                CloudStorageContainers = containers,
                //CloudStorageContainersId = null,
                //AccountName = null,
                //AccountKey
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CloudStorageContainerCreateViewModel model, string returnUrl) {

            //ApplicationDbContext context = new ApplicationDbContext();
            //var providers = context.CloudStorageProviders.ToList();

            //var model = new CloudStorageAccountCreateViewModel {
            //    CloudStorageProviders = providers,
            //    //CloudStorageProviderId = null,
            //    //AccountName = null,
            //    //AccountKey
            //};
            CloudStorageContainer container = new CloudStorageContainer {
                // CloudStorageAccountId = model.AccountName,
                // CloudStorageProviderId = model.CloudStorageProviderId,
                CloudStorageContainerId = model.CloudStorageContainerId,
                CloudStorageAccountId = model.CloudStorageAccountId,
                ContainerName = model.ContainerName,
                Alias = string.IsNullOrEmpty(model.Alias) ? model.Alias : model.ContainerName,
                Description = model.Description // ,
            };

            var user = this.CurrentSecurityUser;
            var accounts = user.CloudStorageAccounts.ToList(); //  .Select(m => m.CloudStorageAccountId == model.CloudStorageAccountId).FirstOrDefault();
            var account = accounts.Where(m => m.CloudStorageAccountId == model.CloudStorageAccountId).FirstOrDefault();
            // user.CloudStorageAccounts.Add(container);
            account.CloudStorageContainers.Add(container);
            await UserManager.UpdateAsync(user);


            return RedirectToLocal(returnUrl);

            //return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Sync(string id) {

            ApplicationDbContext context = new ApplicationDbContext();
            var container = context.CloudStorageContainers.Find(id);
            //var user = this.CurrentSecurityUser;
            //if (user != null) {
            //var account = user.CloudStorageAccounts.SingleOrDefault(a => a.CloudStorageAccountId == id);
            //var model = new CloudStorageContainerDetailViewModel {
            //    CloudStorageContainer = container
            //};
            System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary();
            dict.Add("id", id);

            var blobs = CloudStorageMananger.GetCloudStorageItems(container.CloudStorageAccount.AccountName, container.CloudStorageAccount.AccountKey, id); // .GetContainers(account.AccountName, account.AccountKey);

            foreach (var blob in blobs) {
                if (string.IsNullOrEmpty(blob.OwnerId))
                    blob.OwnerId = User.Identity.GetUserId();
                container.CloudStorageItems.Add(blob);
            }
            context.SaveChanges();

            return RedirectToAction("Detail", dict); // , new { Id = id });
        }
    }
}