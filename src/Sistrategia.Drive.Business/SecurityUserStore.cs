using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Sistrategia.Drive.Business
{
    public class SecurityUserStore : UserStore<SecurityUser, SecurityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public SecurityUserStore(ApplicationDbContext context) 
           :base(context) {

        }
    }
}


//public class UserStore<TUser, TRole, TContext> : UserStore<TUser, TRole, TContext, string>
//    where TUser : IdentityUser, new()
//    where TRole : IdentityRole, new()
//    where TContext : DbContext
//{
//    public UserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
//}