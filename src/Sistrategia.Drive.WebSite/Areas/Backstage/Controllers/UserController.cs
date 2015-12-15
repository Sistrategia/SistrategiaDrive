using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.Drive.Business;
using Sistrategia.Drive.WebSite.Areas.Backstage.Models;
using System.Net;

namespace Sistrategia.Drive.WebSite.Areas.Backstage.Controllers
{
    [Authorize(Roles = "Backstage")]
    public class UserController : Sistrategia.Drive.WebSite.Controllers.BaseController
    {
        // GET: Backstage/User
        public ActionResult Index()
        {
            var model = new UserIndexViewModel();
            model.Users = this.DBContext.Users.ToList();
            return View(model);
        }

        public ActionResult Create() {
            var model = new UserCreateViewModel();            
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserCreateViewModel model) {
            if (ModelState.IsValid) {
                var user = new SecurityUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);
                DBContext.Users.Add(user);
                DBContext.SaveChanges();
                //user.Roles.Add(DBContext)
                //await UserManager.SendEmailAsync(user.Id,
                //        LocalizedStrings.Account_ConfirmYourAccount,
                //        string.Format(LocalizedStrings.Account_ConfirmYourAccountBody, callbackUrl));
                return RedirectToAction("Index");
            }
            //AddErrors(result);
            return View(model);
        }

        //private void AddErrors(IdentityResult result) {
        //    foreach (var error in result.Errors) {
        //        ModelState.AddModelError("", error);
        //    }
        //}

        public ActionResult Details(string id) {
            Guid userId;
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out userId)) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityUser user = DBContext.Users.SingleOrDefault(u => u.PublicKey == userId);
            if (user == null) {
                //return HttpNotFound();
                throw new HttpException(404, "User not found.");
            }

            return View(user);
        }

        public ActionResult Edit(string id) {
            Guid userId;
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out userId)) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityUser user = DBContext.Users.SingleOrDefault(u => u.PublicKey == userId);
            if (user == null) {
                //return HttpNotFound();
                throw new HttpException(404, "User not found.");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, SecurityUser editedUser) {
            Guid userId;
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out userId)) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityUser user = DBContext.Users.SingleOrDefault(u => u.PublicKey == userId);
            if (user == null) {
                //return HttpNotFound();
                throw new HttpException(404, "User not found.");
            }
            user.FullName = editedUser.FullName;
            DBContext.SaveChanges();
            return RedirectToAction("Index");

            // return View(editedUser);
        }

        public ActionResult Delete(string id, bool? saveChangesError = false) {
            Guid userId;
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out userId) ){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityUser user = DBContext.Users.SingleOrDefault(u => u.PublicKey == userId);
            if (user == null) {
                //return HttpNotFound();
                throw new HttpException(404, "User not found.");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id) {
            Guid userId;
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out userId)) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SecurityUser user = DBContext.Users.SingleOrDefault(u => u.PublicKey == userId);
            if (user == null) {
                //return HttpNotFound();
                throw new HttpException(404, "User not found.");
            }
            DBContext.Users.Remove(user);
            DBContext.SaveChanges();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(SecurityUser user) {            
        //    SecurityUser user2 = DBContext.Users.SingleOrDefault(u => u.PublicKey == user.PublicKey); // userId);
        //    if (user2 == null) {
        //        //return HttpNotFound();
        //        throw new HttpException(404, "User not found.");
        //    }
        //    DBContext.Users.Remove(user2);
        //    DBContext.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}