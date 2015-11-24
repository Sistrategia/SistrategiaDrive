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
    [RequireHttps]
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController() {
        }

        public HomeController(SecurityUserManager userManager, SecuritySignInManager signInManager, ApplicationDbContext applicationDBContext)
        : base (userManager, signInManager, applicationDBContext) {            
        }

        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated) {
                return RedirectToAction("Welcome");
            }
            
            var userId = this.GetUserId();
            var user = UserManager.FindById(userId);

            var items = user.DriveItems.OrderByDescending(i => i.Modified);
            //var items = user.CloudStorageItems.OrderByDescending(i => i.Modified);

            HomeIndexViewModel model = new HomeIndexViewModel {                
                RecentItems = items.ToList()
            };

            return View(model);
        }

        //[OutputCache(Duration = 60, VaryByParam="*")]
        [AllowAnonymous]
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
        public ActionResult ChangeLang(string lang, string returnUrl) {
            var langCookie = new HttpCookie("locale", lang) { HttpOnly = true };
            Response.AppendCookie(langCookie);
            //if (string.IsNullOrEmpty(returnUrl))
            //    return RedirectToAction("Index");
            return Redirect(HttpUtility.UrlDecode(returnUrl));
        }

        [AllowAnonymous]
        public ActionResult Cover() {
            return View();
        }



        //[Authorize(Roles = "Administrator,Developer")]
        [Authorize]
        public ActionResult AddDocument(/*Guid ownerId*/) {

            var userId = User.Identity.GetUserId();

            if (!string.IsNullOrEmpty(userId)) {
                var user = UserManager.FindById(this.GetUserId());
                // var user = await UserManager.FindByIdAsync(userId);
                return View(new AddDocumentViewModel() {
                    //OwnerId = Guid.Parse( user.Id )
                    OwnerId = user.Id
                });
            }

            //using (var db = new ApplicationDbContext()) {                
                //return View(new AddDocumentViewModel() { 
                //    ExpedientId = id, PhaseId = (Guid)phaseId, SubPhaseId = (Guid)subPhaseId 
                //});
            //}
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrador,Developer,Abogado")]
        [Authorize]
        public ActionResult AddDocument(AddDocumentViewModel model) {

            if (ModelState.IsValid) {

                string fullPath = null;

                if (model.File == null || model.File.ContentLength == 0) {
                    return View();
                }

                try {
                    var userId = this.GetUserId();
                    var user = UserManager.FindById(userId);

                    Guid publicKey = Guid.NewGuid();

                    CloudStorageMananger storage = new CloudStorageMananger(this.DBContext);
                    //var cloudStorageItem = storage.UploadFromStream(user.PublicKey, User.Identity.Name, publicKey, model.File.FileName, model.File.ContentType, model.File.InputStream, model.DocumentName, model.Description);
                    var cloudStorageItem = storage.UploadFromStream(user.DefaultContainer.CloudStorageAccount.AccountName, user.DefaultContainer.CloudStorageAccount.AccountKey, user.DefaultContainer.ContainerName,
                            this.GetUserId(), User.Identity.Name, model.File.FileName, model.File.ContentType, model.File.InputStream, model.DocumentName, model.Description);
                    //string fileName = UploadFile.UploadFileToPrivateContainer(model.File);
                    //if (!string.IsNullOrEmpty(userId)) {
                    //    var user = UserManager.FindById(userId);
                    //    blockBlob.Metadata.Add("username", user.UserName);
                    //}

                    var item = new DriveItem(cloudStorageItem);

                    //ApplicationDbContext context = new ApplicationDbContext();
                    //ApplicationDbContext context = DBContext;
                    item.OwnerId = this.GetUserId();
                    item.CloudStorageItem.CloudStorageContainerId = (int)user.DefaultContainerId;
                    //context.CloudStorageItems.Add(item);
                    this.DBContext.DriveItems.Add(item);
                    this.DBContext.SaveChanges();

                    //user.CloudStorageItems.Add(item);                    
                    //UserManager.Update(user);

                    //user.DefaultContainer.CloudStorageItems.Add(item);
                }
                catch (Exception ex) {
                    //log.Error(ex, "Error upload photo blob to storage");
                    ex.ToString();
                }

                //        using (var db = new CSDGContext()) {

                //            Document document = new Document() {
                //                DocumentId = Guid.NewGuid(),
                //                DocumentName = model.DocumentName,
                //                Description = model.Description,
                //                CDNDocumentName = fileName,
                //                //CDNDocumentUrl = rackspace["FileUrlRackspace"],
                //                ExpedientId = model.ExpedientId,
                //                PhaseId = model.SubPhaseId,
                //                Status = "A"
                //            };

                //            if (model.ContactId == null) {
                //                Contact contact = new Contact();
                //                contact.ContactId = Guid.NewGuid();
                //                contact.ContactType = model.ContactType;
                //                if (model.ContactType.Equals("person")) {
                //                    contact.PersonFirstName = model.PersonFirstName;
                //                    contact.PersonLastName1 = model.PersonLastName1;
                //                    contact.PersonLastName2 = model.PersonLastName2;
                //                }
                //                else if (model.ContactType.Equals("organization")) {
                //                    contact.OrganizationName = model.OrganizationName;
                //                }
                //                else if (model.ContactType.Equals("group")) {
                //                    contact.GroupName = model.GroupName;
                //                }
                //                contact.Status = "A";
                //                db.Contacts.Add(contact);
                //                document.ContactId = contact.ContactId;
                //            }
                //            else {
                //                document.ContactId = (Guid)model.ContactId;
                //            }

                //            db.Documents.Add(document);

                //            Phase phase = db.Phases.Find(model.SubPhaseId);

                //            if (phase.SendNotification) {
                //                Expedient expedient = db.Expedients.Find(model.ExpedientId);

                //                string server = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                //                string virtualDirectory = System.Web.HttpContext.Current.Request.ApplicationPath;
                //                string link = server + virtualDirectory + "/Customers/Documents/" + expedient.ExpedientId;

                //                string message = "El usuario " + User.Identity.Name + " ha cargado el documento " + "<a href='#' data-id='" + document.DocumentId + "' id='getDocument'>" + document.DocumentName + "</a>" + " del contrato " + "<a href='" + link + "'>" + expedient.ContractNumber + "</a>" + " del cliente " + expedient.Customer.DisplayName;

                //                DataTable dt = DatabaseManager.ExecuteDataTable(System.Data.CommandType.Text, "SELECT u.* FROM AspNetUsers AS u INNER JOIN AspNetUserRoles as ur ON ur.UserId = u.Id INNER JOIN AspNetRoles as r ON r.Id = ur.RoleId WHERE r.Name = 'Administrador'");


                //                ApplicationUser userSystem = UserManager.Users.Where(u => u.UserName.Equals("Sistema")).Single();
                //                foreach (DataRow user in dt.Rows) {
                //                    Notification notification = new Notification();
                //                    notification.NotificationId = Guid.NewGuid();
                //                    notification.NotificationType = "System";
                //                    notification.EntitySourceType = "Document";
                //                    notification.EntitySourceId = document.DocumentId;
                //                    notification.From = Guid.Parse(userSystem.Id);
                //                    notification.To = Guid.Parse(user["Id"].ToString());
                //                    notification.Subject = "Se agregó el documento " + "<a href='#' data-id='" + document.DocumentId + "' id='getDocument'>" + document.DocumentName + "</a>" + " a la etapa " + phase.PhaseName + " del contrato " + "<a href='" + link + "'>" + expedient.ContractNumber + "</a>" + " del cliente " + expedient.Customer.DisplayName;
                //                    notification.Body = message;
                //                    notification.Seen = false;
                //                    notification.Status = "E";
                //                    db.Notifications.Add(notification);
                //                }

                //            }
                //            db.SaveChanges();
                //            return RedirectToAction("Documents", new { id = model.ExpedientId, phaseId = model.PhaseId, subPhaseId = model.SubPhaseId });
                //        }
                //    }
                //    catch (Exception ex) {
                //        ModelState.AddModelError("", ex.Message);
                //    }
                //}
                //return View(model);               
            }

            return RedirectToAction("Index", "Home");
            //return View(model);
        }

        [Authorize]
        public ActionResult Detail(string id) {

            //ApplicationDbContext context = new ApplicationDbContext();
            // var item = context.CloudStorageItems.Find(id);

            var user = UserManager.FindById(this.GetUserId());

            var cid = Guid.Parse(id);
            //var item = context.DriveItems.SingleOrDefault(c => c.PublicKey == cid);
            var item = user.DriveItems.SingleOrDefault(c => c.PublicKey == cid);
            var manager = new CloudStorageMananger(this.DBContext);
            //var blob = manager.GetStorageItem(item.ProviderKey);

            if (item.ContentType.StartsWith("image/")) {

            }

            //var user = this.CurrentSecurityUser;
            //if (user != null) {
            //var account = user.CloudStorageAccounts.SingleOrDefault(a => a.CloudStorageAccountId == id);
            var model = new HomeDetailViewModel(manager) {
                DriveItem = item,
                //Url = blob.Url,
                IsImage = item.ContentType.StartsWith("image/")
            };

            return View(model);
        }

    }
}