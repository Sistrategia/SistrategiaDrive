using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using Sistrategia.Drive.Business;
using Sistrategia.Drive.WebSite.Models;
using Sistrategia.Drive.Resources;

namespace Sistrategia.Drive.WebSite.Controllers
{
    public class BaseController : Controller
    {
        private SecuritySignInManager signInManager;
        private SecurityUserManager userManager;

        public BaseController() {

        }

        public BaseController(SecurityUserManager userManager, SecuritySignInManager signInManager) {
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

        public SecurityUser CurrentSecurityUser {
            get {
                var userId = int.Parse( User.Identity.GetUserId() );
                return UserManager.FindById(userId);
            }
        }
    }
}