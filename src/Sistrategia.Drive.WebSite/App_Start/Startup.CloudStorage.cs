using System;
using Owin;
using Microsoft.Owin;
using Sistrategia.Drive.Business; // Owin

namespace Sistrategia.Drive.WebSite
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureCloudStorage(IAppBuilder app) {
            // app.CreatePerOwinContext<CloudStorageMananger>(CloudStorageMananger.Create);
        }
    }
}