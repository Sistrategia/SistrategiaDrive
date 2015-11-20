using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
//using Sistrategia.AspNet.Identity;
//using Sistrategia.AspNet.Identity.Owin;
using Microsoft.Owin;
using Owin;
using Sistrategia.Drive.Business.CloudStorage;
//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.DataProtection;
//using Microsoft.Owin.Security.OAuth;

//namespace Sistrategia.Drive.Business.Extensions
namespace Sistrategia.Drive.Business
{
    /// <summary>
    ///     Extensions off of IAppBuilder to make it easier to configure the SignInCookies
    /// </summary>
    public static class AppBuilderExtensions
    {
        /// <summary>
        ///     Registers a callback that will be invoked to create an instance of type T that will be stored in the OwinContext
        ///     which can fetched via context.Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="app"></param>
        /// <param name="createCallback"></param>
        /// <returns></returns>
        public static IAppBuilder CreatePerOwinContext<T>(this IAppBuilder app,            
            Func<CloudStorageFactoryOptions<T>, IOwinContext, T> createCallback) where T : class, IDisposable {
            if (app == null) {
                throw new ArgumentNullException("app");
            }
            return app.CreatePerOwinContext(createCallback, (oprtions, instance) => instance.Dispose());
        }

        /// <summary>
        ///     Registers a callback that will be invoked to create an instance of type T that will be stored in the OwinContext
        ///     which can fetched via context.Get
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="app"></param>
        /// <param name="createCallback"></param>
        /// <param name="disposeCallback"></param>
        /// <returns></returns>
        public static IAppBuilder CreatePerOwinContext<T>(this IAppBuilder app,
            Func<CloudStorageFactoryOptions<T>, IOwinContext, T> createCallback,
            Action<CloudStorageFactoryOptions<T>, T> disposeCallback) where T : class, IDisposable {            
            if (app == null) {
                throw new ArgumentNullException("app");
            }
            if (createCallback == null) {
                throw new ArgumentNullException("createCallback");
            }
            if (disposeCallback == null) {
                throw new ArgumentNullException("disposeCallback");
            }

            //app.Use(typeof(IdentityFactoryMiddleware<T, IdentityFactoryOptions<T>>),
            //    new IdentityFactoryOptions<T> {
            //        DataProtectionProvider = app.GetDataProtectionProvider(),
            //        Provider = new IdentityFactoryProvider<T> {
            //            OnCreate = createCallback,
            //            OnDispose = disposeCallback
            //        }
            //    });

            app.Use(typeof(CloudStorageFactoryMiddleware<T, CloudStorageFactoryOptions<T>>),
                new CloudStorageFactoryOptions<T> {
                    //DataProtectionProvider = app.GetDataProtectionProvider(),
                    Provider = new CloudStorageFactoryProvider<T> {
                        OnCreate = createCallback,
                        OnDispose = disposeCallback
                    }
                });

            return app;
        }
    }
}
