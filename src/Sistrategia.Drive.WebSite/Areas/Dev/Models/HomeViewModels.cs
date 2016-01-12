using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.Owin;
//using Sistrategia.Drive.Business;
//using Sistrategia.Drive.Resources;

namespace Sistrategia.Drive.WebSite.Areas.Dev.Models
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
}