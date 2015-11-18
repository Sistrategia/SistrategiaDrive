using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistrategia.Drive.Business.CloudStorage.Azure
{
    internal class AzureCloudStorageProvider : ICloudStorageProvider
    {
        private ApplicationDbContext context = null;
        private CloudStorageAccount defaultCloudStorageAccount = null;

        internal AzureCloudStorageProvider(ApplicationDbContext context) {
            if (context == null)
                throw new ArgumentNullException("context");
            this.context = context;
        }

        internal CloudStorageAccount GetDefaultCloudStorageAccount() {
            if (this.defaultCloudStorageAccount == null) {

                //var account = this.context.CloudStorageAccounts.Find(new Guid(ConfigurationManager.AppSettings["AzureDefaultCloudStorageAccountId"]));
                Guid accountPublicKey = new Guid(ConfigurationManager.AppSettings["AzureDefaultCloudStorageAccountId"]);
                var account = this.context.CloudStorageAccounts.Where(p => p.PublicKey == accountPublicKey).SingleOrDefault();

                if (account == null) {
                    account = this.context.CloudStorageAccounts.Add(new CloudStorageAccount {
                        PublicKey = new Guid(ConfigurationManager.AppSettings["AzureDefaultCloudStorageAccountId"]),
                        CloudStorageProviderId = "Azure",
                        ProviderKey = ConfigurationManager.AppSettings["AzureAccountName"],
                        AccountName = ConfigurationManager.AppSettings["AzureAccountName"],
                        AccountKey = ConfigurationManager.AppSettings["AzureAccountKey"],
                        Alias = ConfigurationManager.AppSettings["AzureAccountName"],
                        Description = "SistrategiaDrive Default Account",
                    });
                    this.context.SaveChanges();
                }

                this.defaultCloudStorageAccount = account;
            }

            return this.defaultCloudStorageAccount;
        }

        internal Microsoft.WindowsAzure.Storage.CloudStorageAccount CreateInternalAzureCloudStorageAccount(string accountName, string accountKey) {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = //new Microsoft.WindowsAzure.Storage.CloudStorageAccount()
               Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
                   string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};BlobEndpoint=https://{0}.blob.core.windows.net/", accountName, accountKey)
                  );
            return storageAccount;
        }

        public CloudStorageContainer CreateCloudStorageContainer(string alias, string description) {
            CloudStorageAccount account = this.GetDefaultCloudStorageAccount();
            Guid publicKey = Guid.NewGuid();
            return this.CreateCloudStorageContainer(account, publicKey, alias, publicKey.ToString("N"), description);
        }

        public CloudStorageContainer CreateCloudStorageContainer(Guid publicKey, string alias, string description) {
            CloudStorageAccount account = this.GetDefaultCloudStorageAccount();
            return this.CreateCloudStorageContainer(account, publicKey, alias, publicKey.ToString("N"), description);
        }

        public CloudStorageContainer CreateCloudStorageContainer(Guid publicKey, string alias, string containerName, string description) {
            CloudStorageAccount account = this.GetDefaultCloudStorageAccount();
            return this.CreateCloudStorageContainer(account, publicKey, alias, containerName, description);
        }        

        public CloudStorageContainer CreateCloudStorageContainer(CloudStorageAccount account, string alias, string description) {
            Guid publicKey = Guid.NewGuid();
            return this.CreateCloudStorageContainer(account, publicKey, alias, publicKey.ToString("N"), description);
        }

        public CloudStorageContainer CreateCloudStorageContainer(CloudStorageAccount account, Guid publicKey, string alias, string containerName, string description) {
            if (account == null)
                throw new NullReferenceException("DefaultCloudStorageAccount cannot be null.");

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = CreateInternalAzureCloudStorageAccount(account.AccountName, account.AccountKey);
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            container.Create();

            container.SetPermissions(new Microsoft.WindowsAzure.Storage.Blob.BlobContainerPermissions {
                PublicAccess = Microsoft.WindowsAzure.Storage.Blob.BlobContainerPublicAccessType.Off
            });

            // var containers = blobClient.ListContainers();

            CloudStorageContainer csContainer = this.context.CloudStorageContainers.Add( new CloudStorageContainer {
                CloudStorageAccountId = account.CloudStorageAccountId,
                PublicKey = publicKey,
                ProviderKey = containerName,
                ContainerName = containerName,
                Alias = alias,
                Description = description
            });

            context.SaveChanges();

            return csContainer;
        }

        ////internal CloudStorageContainer CreateCloudStorageContainer(string containerName,  string accountName, string AccountKey)
    }
}
