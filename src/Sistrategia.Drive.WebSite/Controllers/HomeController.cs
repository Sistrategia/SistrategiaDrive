using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.Drive.Business;
using Sistrategia.Drive.WebSite.Models;

using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;


namespace Sistrategia.Drive.WebSite.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : BaseController
    {
        #region Constructor
        public HomeController() {
        }

        public HomeController(SecurityUserManager userManager, 
            SecuritySignInManager signInManager, 
            ApplicationDbContext applicationDBContext)
        : base (userManager, signInManager, applicationDBContext) {            
        }
        #endregion

        #region Public Welcome Site

        [AllowAnonymous]
        [OutputCache(Duration = 60, VaryByHeader="*", VaryByParam = "*")]
        public ActionResult Welcome() {
            Sistrategia.Drive.WebSite.Views.Home.Welcome.Title.ToString();
            return View();
        }

        [AllowAnonymous]
        public ActionResult TermsOfUse() {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Privacy() {
            return View();
        }        

        [AllowAnonymous]
        public ActionResult Cover() {
            return View();
        }

        #endregion

        [AllowAnonymous]
        public ActionResult Index() {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Welcome");
            var user = UserManager.FindById(this.GetUserId());
            var items = user.DriveItems.OrderByDescending(i => i.Modified);
            HomeIndexViewModel model = new HomeIndexViewModel {
                RecentItems = items.ToList()
            };
            return View(model);
        }

        [Authorize]
        public ActionResult AddDocument() {
            if (!string.IsNullOrEmpty(User.Identity.GetUserId())) {
                var user = UserManager.FindById(this.GetUserId());
                // var user = await UserManager.FindByIdAsync(userId);
                return View(new AddDocumentViewModel() {
                    OwnerId = user.Id
                });
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddDocument(AddDocumentViewModel model) {
            if (ModelState.IsValid) {                
                if (model.File == null || model.File.ContentLength == 0) {
                    return View();
                }
                try {                    
                    var user = UserManager.FindById(this.GetUserId());
                    Guid publicKey = Guid.NewGuid();
                    CloudStorageMananger storage = new CloudStorageMananger(this.DBContext);                    
                    var cloudStorageItem = storage.UploadFromStream(
                        user.DefaultContainer.CloudStorageAccount.AccountName, 
                        user.DefaultContainer.CloudStorageAccount.AccountKey, 
                        user.DefaultContainer.ContainerName,
                        this.GetUserId(), User.Identity.Name, 
                        model.File.FileName, model.File.ContentType, 
                        model.File.InputStream, model.DocumentName, model.Description);
                    var item = new DriveItem(cloudStorageItem);
                    item.OwnerId = this.GetUserId();
                    item.CloudStorageItem.CloudStorageContainerId = (int)user.DefaultContainerId;                    
                    this.DBContext.DriveItems.Add(item);
                    this.DBContext.SaveChanges();
                }
                catch (Exception ex) {
                    //log.Error(ex, "Error upload photo blob to storage");
                    ex.ToString();
                }                
            }
            return RedirectToAction("Index", "Home");            
        }

        [Authorize]
        public ActionResult Detail(string id) {
            var user = UserManager.FindById(this.GetUserId());
            var cid = Guid.Parse(id);
            var item = user.DriveItems.SingleOrDefault(c => c.PublicKey == cid);
            var manager = new CloudStorageMananger(this.DBContext);
            if (item.ContentType.StartsWith("image/")) {
            }
            var model = new HomeDetailViewModel(manager) {
                DriveItem = item,             
                IsImage = item.ContentType.StartsWith("image/")
            };
            return View(model);
        }
    }
}