using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.Drive.Business;
using Sistrategia.Drive.WebSite.Models;
using System.Threading.Tasks;

namespace Sistrategia.Drive.WebSite.Controllers
{
    public class CloudStorageAccountController : BaseController
    {
        // GET: CloudStorageAccount
        public ActionResult Index()
        {
            var user = this.CurrentSecurityUser;
            if (user != null) {
                var model = new CloudStorageAccountIndexViewModel {
                    CloudStorageAccounts = user.CloudStorageAccounts.OrderBy(p => p.Alias).ToList()
                };

                return View(model);
            }

            return View();
            // CloudStorageAccounts = user.CloudStorageAccounts.OrderBy(p=>p.Alias).ToList()
        }

        public ActionResult Detail(string id) {
            var user = this.CurrentSecurityUser;
            if (user != null) {
                var account = user.CloudStorageAccounts.SingleOrDefault(a => a.CloudStorageAccountId == id);
                var model = new CloudStorageAccountDetailViewModel {
                    CloudStorageAccount = account
                };
                return View(model);
            }
            return View();
        }

        public ActionResult Create() {

            ApplicationDbContext context = new ApplicationDbContext();
            var providers = context.CloudStorageProviders.ToList();

            var model = new CloudStorageAccountCreateViewModel {
                CloudStorageProviders = providers,
                //CloudStorageProviderId = null,
                //AccountName = null,
                //AccountKey
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CloudStorageAccountCreateViewModel model, string returnUrl) {

            //ApplicationDbContext context = new ApplicationDbContext();
            //var providers = context.CloudStorageProviders.ToList();

            //var model = new CloudStorageAccountCreateViewModel {
            //    CloudStorageProviders = providers,
            //    //CloudStorageProviderId = null,
            //    //AccountName = null,
            //    //AccountKey
            //};
            CloudStorageAccount account = new CloudStorageAccount {
                CloudStorageAccountId = model.AccountName,
                CloudStorageProviderId = model.CloudStorageProviderId,
                AccountName = model.AccountName,
                AccountKey = model.AccountKey,
                Alias = string.IsNullOrEmpty(model.Alias) ? model.Alias : model.AccountName,
                Description = model.Description
            };

            var user = this.CurrentSecurityUser;
            user.CloudStorageAccounts.Add(account);
            await UserManager.UpdateAsync(user);
                       

            return RedirectToLocal(returnUrl);

            //return View(model);
        }

        public ActionResult Sync(string id) {
            ApplicationDbContext context = new ApplicationDbContext();
            //var container = context.CloudStorageContainers.Find(id);
            var account = context.CloudStorageAccounts.Find(id);
            
            System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary();
            dict.Add("id", id);

            var containers = CloudStorageMananger.GetContainers(account.AccountName, account.AccountKey); // .GetContainers(account.AccountName, account.AccountKey);

            foreach (var container in containers) {
                //container.CloudStorageAccountId = 
                account.CloudStorageContainers.Add(container);
            }
            context.SaveChanges();

            return RedirectToAction("Detail", dict); // , new { Id = id });
        }

        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}