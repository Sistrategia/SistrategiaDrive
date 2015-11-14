using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Sistrategia.Drive.Resources;
using Sistrategia.Drive.Business;
//using Microsoft.Owin.Security;

namespace Sistrategia.Drive.WebSite.Models
{
    public class CloudStorageAccountIndexViewModel
    {
        //public string UserName { get; set; }
        public IList<CloudStorageAccount> CloudStorageAccounts { get; set; }
    }

    public class CloudStorageAccountDetailViewModel
    {
        public CloudStorageAccount CloudStorageAccount { get; set; }
        //public IList<CloudStorageAccount> CloudStorageAccounts { get; set; }
    }

    public class CloudStorageAccountCreateViewModel
    {
        //public CloudStorageAccountCreateViewModel() {            
        //}

        public IList<CloudStorageProvider> CloudStorageProviders { get; set; }
        public string CloudStorageProviderId { get; set; }        

        public string AccountName { get; set; }

        public string AccountKey { get; set; }

        public string Alias { get; set; }

        public string Description { get; set; }
    }



    public class CloudStorageContainerDetailViewModel
    {
        public CloudStorageContainer CloudStorageContainer { get; set; }
    }

    public class CloudStorageContainerCreateViewModel
    {
        public string CloudStorageContainerId { get; set; }
        public IList<CloudStorageContainer> CloudStorageContainers { get; set; }
        public string CloudStorageAccountId { get; set; }

        public string ProviderKey { get; set; }
        
        public string ContainerName { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
    }

    public class CloudStorageItemDetailViewModel
    {
        public CloudStorageItem CloudStorageItem { get; set; }
    }
}