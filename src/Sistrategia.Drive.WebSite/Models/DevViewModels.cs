using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.Owin;
using Sistrategia.Drive.Business;
//using System.Web;
//using Microsoft.AspNet.Identity;
//using Sistrategia.Drive.Resources;
//using Sistrategia.Drive.Business;

namespace Sistrategia.Drive.WebSite.Models
{
    public class DevIndexViewModel
    {
        private IOwinContext context = null;
        //public DevIndexViewModel(System.Web.HttpContextBase httpContext) {
        //    this.CloudStorageAccounts = new List<CloudStorageAccount>();
        //}

        public DevIndexViewModel(IOwinContext owinContext) { //System.Web.HttpContextBase httpContext) {
            this.context = owinContext;
        }

        protected IOwinContext OwinContext {
            get { return this.context; }
        }

        public string Title { get; set; }

        public string AcceptLanguage {
            get {
                return this.OwinContext.Request.Headers["Accept-Language"];
            }
        }
        //this.ViewBag.AcceptLanguage = HttpContext.Request.Headers["Accept-Language"];

        public System.Globalization.CultureInfo CurrentUICulture {
            get {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
        }

        public System.Globalization.CultureInfo CurrentCulture {
            get {
                return System.Threading.Thread.CurrentThread.CurrentCulture;
            }
        }        
    }

    public class DevUsersViewModel
    {
        private ApplicationDbContext dbContext = null;
        private IList<SecurityUser> users = null;

        public DevUsersViewModel(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }

        protected ApplicationDbContext DBContext {
            get { return this.dbContext; }
        }

        public string Title { get; set; }

        public IList<SecurityUser> Users {
            get {
                if (this.users == null) {
                    this.users = this.DBContext.Users.OrderBy(u => u.UserName).ToList();
                }

                return this.users;
            }
        }
    }
}