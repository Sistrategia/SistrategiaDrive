using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Sistrategia.Drive.WebSite.Models;

namespace Sistrategia.Drive.WebSite
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store) {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(/*context.Get<ApplicationDbContext>()*/));
            return manager;
        }
    }


    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user) {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context) {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }



    public class UserStore<TUser> : IUserStore<TUser>
        where TUser : ApplicationUser
    {


        public Task CreateAsync(TUser user) {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TUser user) {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByIdAsync(string userId) {
            throw new NotImplementedException();
        }

        public Task<TUser> FindByNameAsync(string userName) {

            //Task<string> getStringTask = Task.FromResult(user); // client.GetStringAsync("http://msdn.microsoft.com");

            ApplicationUser user = new ApplicationUser {
                Id = "someId",
                UserName = "SomeName"
            };
            //var result = await getStringTask.Result; // Task.Delay(1000);

            //return (Task<TUser>) Task<ApplicationUser>.FromResult(user);
            return Task.FromResult(user) as Task<TUser>;
            //throw new NotImplementedException();
        }

        public Task UpdateAsync(TUser user) {
            throw new NotImplementedException();
        }

        public void Dispose() {
            //throw new NotImplementedException();
        }
    }
}