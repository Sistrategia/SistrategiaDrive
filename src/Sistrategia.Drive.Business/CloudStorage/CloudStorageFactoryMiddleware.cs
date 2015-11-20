using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

using Sistrategia.Drive.Business;
using Sistrategia.Drive.Business.CloudStorage.Owin;

namespace Sistrategia.Drive.Business.CloudStorage
{
    public class CloudStorageFactoryMiddleware<TResult, TOptions> : OwinMiddleware
        where TResult : IDisposable
        where TOptions : CloudStorageFactoryOptions<TResult>
    {
        public CloudStorageFactoryMiddleware(OwinMiddleware next, TOptions options)
            : base(next) {
                if (options == null) {
                    throw new ArgumentNullException("options");
                }
                if (options.Provider == null) {
                    throw new ArgumentNullException("options.Provider");
                }
                Options = options;
        }

        public TOptions Options { get; private set; }        

        public override async Task Invoke(IOwinContext context) {
            //var instance = CloudStorageMananger.Create(context);
            var instance = Options.Provider.Create(Options, context);
            try {
                context.Set(instance); // Sistrategia.Drive.Business.CloudStorage.Owin;
                if (Next != null) {
                    await Next.Invoke(context);
                }
            }
            finally {
                Options.Provider.Dispose(Options, instance);
                // instance.Dispose();
            }
            //var instance = Options.Provider.Create(Options, context);
            //try {
            //    context.Set(instance);
            //    if (Next != null) {
            //        await Next.Invoke(context);
            //    }
            //}
            //finally {
            //    Options.Provider.Dispose(Options, instance);
            //}
        }

    }
}
