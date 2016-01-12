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
    public class CloudStorageAccountController : Sistrategia.Drive.WebSite.Controllers.BaseController
    {
        // GET: Dev/CloudStorageAccount
        public ActionResult Index() {
            var model = new CloudStorageAccountIndexViewModel {
                CloudStorageAccounts = DBContext.CloudStorageAccounts.OrderBy(p => p.Alias).ToList()
            };
            return View(model);
        }

        public ActionResult Details(string id) {
            Guid publicKey = Guid.Parse(id);
            var account = DBContext.CloudStorageAccounts.SingleOrDefault(a => a.PublicKey == publicKey);
            var model = new CloudStorageAccountDetailViewModel {
                CloudStorageAccount = account
            };
            return View(model);
        }

        public ActionResult Create() {
            var model = new CloudStorageAccountCreateViewModel {
                CloudStorageProviders = DBContext.CloudStorageProviders.ToList()
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
                // https://msdn.microsoft.com/en-us/library/97af8hh4(v=vs.110).aspx
                //CloudStorageAccountId = Guid.NewGuid().ToString("D").ToLower(), // model.AccountName,
                CloudStorageProviderId = model.CloudStorageProviderId,
                ProviderKey = model.AccountName,
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
            //ApplicationDbContext context = new ApplicationDbContext();
            //var container = context.CloudStorageContainers.Find(id);
            //var account = context.CloudStorageAccounts.Find(id);
            var cid = Guid.Parse(id);
            var account = DBContext.CloudStorageAccounts.SingleOrDefault(c => c.PublicKey == cid);

            System.Web.Routing.RouteValueDictionary dict = new System.Web.Routing.RouteValueDictionary();
            dict.Add("id", id);

            var containers = CloudStorageMananger.ImportContainers(account.AccountName, account.AccountKey); // .GetContainers(account.AccountName, account.AccountKey);

            foreach (var container in containers) {
                //container.CloudStorageAccountId = 
                if (account.CloudStorageContainers.SingleOrDefault(c => c.ProviderKey == container.ProviderKey) == null)
                    account.CloudStorageContainers.Add(container);
            }
            DBContext.SaveChanges();

            return RedirectToAction("Details", dict); // , new { Id = id });
        }



        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            //return RedirectToAction("Index", "CloudStorageAccount");
            return RedirectToAction("Index");
        }
    }
}