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
    public class HomeController : Controller
    {
        private SecuritySignInManager signInManager;
        private SecurityUserManager userManager;

        public HomeController() {

        }

        public HomeController(SecurityUserManager userManager, SecuritySignInManager signInManager) {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public SecuritySignInManager SignInManager {
            get { return signInManager ?? HttpContext.GetOwinContext().Get<SecuritySignInManager>(); }
            private set { signInManager = value; }
        }

        public SecurityUserManager UserManager {
            get { return userManager ?? HttpContext.GetOwinContext().GetUserManager<SecurityUserManager>(); }
            private set { userManager = value; }
        }


        // GET: Home
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated) {
                return RedirectToAction("Welcome");
            }

            // CloudStorageMananger.GetBlobs();

            CloudStorageMananger storage = new CloudStorageMananger();
            var itemList = storage.GetCloudStorageItems();

            DocumentListModel model = new DocumentListModel {
                DocumentList = itemList // blobNames
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
                var user = UserManager.FindById(userId);
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
                    CloudStorageMananger storage = new CloudStorageMananger();
                    storage.UploadFromStream(User.Identity.GetUserId(), User.Identity.Name, model.File.FileName, model.File.ContentType, model.File.InputStream, model.DocumentName, model.Description);
                    //string fileName = UploadFile.UploadFileToPrivateContainer(model.File);
                    //if (!string.IsNullOrEmpty(userId)) {
                    //    var user = UserManager.FindById(userId);
                    //    blockBlob.Metadata.Add("username", user.UserName);
                    //}
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

    }
}