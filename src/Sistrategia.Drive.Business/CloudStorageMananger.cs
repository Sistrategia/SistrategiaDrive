using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sistrategia.Drive.Business
{
    public class CloudStorageMananger
    {
        #region Private Properties and Methods
        private string AzureDefaultStorageConnectionString {
            get {
                return System.Configuration.ConfigurationManager.AppSettings["AzureDefaultStorageConnectionString"];
            }
        }

        private string AzureAccountName {
            get {
                return System.Configuration.ConfigurationManager.AppSettings["AzureAccountName"];
            }
        }

        private string DefaultCloudBlobContainerName {
            get {
                return System.Configuration.ConfigurationManager.AppSettings["AzureDefaultStorage"];
            }
        }

        private Microsoft.WindowsAzure.Storage.CloudStorageAccount GetDefaultCloudStorageAccount() {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount =
                Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(this.AzureDefaultStorageConnectionString);
                return storageAccount;
        }

        //public ICloudBlob GetBlobReference(string itemName) {
        //    Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = this.GetDefaultCloudStorageAccount();
        //    Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        //}
        #endregion

        public string GetTempUrl(string fullPath) {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = this.GetDefaultCloudStorageAccount();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var blob = blobClient.GetBlobReferenceFromServer(new Uri(fullPath));

            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
            };

            string resultUrl = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy)).ToString();

            return resultUrl;
        }

        public string GetTempDownloadUrl(string fullPath) {
             Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = this.GetDefaultCloudStorageAccount();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var blob = blobClient.GetBlobReferenceFromServer(new Uri(fullPath));

            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
            };

            string resultUrl = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy,
                    new SharedAccessBlobHeaders {
                        ContentDisposition = blob.Metadata.ContainsKey("originalfilename") ? "attachment; filename=" + blob.Metadata["originalfilename"] : "attachment; filename=FileUnknown",
                        ContentType = blob.Properties.ContentType
                    }
                    )).ToString();

            return resultUrl;
        }

        public CloudStorageItem GetStorageItem(string itemName) {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = this.GetDefaultCloudStorageAccount();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            //Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(this.DefaultCloudBlobContainerName);
            var blob = blobClient.GetBlobReferenceFromServer(new Uri(string.Format("https://{0}.blob.core.windows.net/{1}/{2}", this.AzureAccountName, this.DefaultCloudBlobContainerName, itemName)));            
            
            //var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
            //    Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
            //    SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
            //};

            blob.FetchAttributes();

            var uriBuilder = new UriBuilder(blob.Uri);
            uriBuilder.Scheme = "https";
            var fullPath = uriBuilder.ToString();

            //if (Guid.TryParse(blob.Name))
          
            var item = new CloudStorageItem {
                // PublicKey
                ProviderKey = blob.Name,
                //OwnerId = blob.Metadata.ContainsKey("userid") ? int.Parse( blob.Metadata["userid"] ) : 0, // null,
                OwnerId = blob.Metadata.ContainsKey("userid") ? blob.Metadata["userid"] : null,
                Created = blob.Metadata.ContainsKey("created") ? DateTime.Parse(blob.Metadata["created"]) : DateTime.UtcNow,
                Modified = blob.Metadata.ContainsKey("modified") ? DateTime.Parse(blob.Metadata["modified"]) : DateTime.UtcNow,
                Name = blob.Metadata.ContainsKey("name") ? blob.Metadata["name"] : blob.Name, // sourceFileName,
                OriginalName = blob.Metadata.ContainsKey("originalfilename") ? blob.Metadata["originalfilename"] : null,
                Url = fullPath,
                //Url = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy,
                //    new SharedAccessBlobHeaders {
                //        ContentDisposition = blob.Metadata.ContainsKey("originalfilename") ? "attachment; filename=" + blob.Metadata["originalfilename"] : "attachment; filename=FileUnknown",
                //        ContentType = blob.Properties.ContentType
                //    }
                //    )).ToString(),
                ContentType = blob.Properties.ContentType,
                ContentMD5 = blob.Properties.ContentMD5
            };
            
            return item;
        }

        public CloudStorageItem UploadFromStream(Guid userPublicKey, string userName, Guid publicKey, string sourceFileName, string fileContentType, System.IO.Stream fileInputStream, string name, string fileDescription) {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = GetDefaultCloudStorageAccount();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(DefaultCloudBlobContainerName);

            // Guid publicKey = Guid.NewGuid();
            string ext = System.IO.Path.GetExtension(sourceFileName);

            string providerKey = String.Format(
                //"{0}-{1}{2}",
                //DateTime.Now.ToString("yyyy-MM-dd"),
                        //"{0}{1}",
                        "{0}",
                        publicKey.ToString("N")
                        //,System.IO.Path.GetExtension(sourceFileName)
                        );

            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(providerKey);
            blockBlob.Properties.ContentType = fileContentType;
            blockBlob.Metadata.Add("name", name);            
            blockBlob.Metadata.Add("originalfilename", sourceFileName);
            blockBlob.Metadata.Add("userid", userPublicKey.ToString("N")); // userId.ToString());
            blockBlob.Metadata.Add("ownerid", userPublicKey.ToString("N")); // userId.ToString());
            DateTime created = DateTime.UtcNow;
            // https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
            // http://stackoverflow.com/questions/114983/given-a-datetime-object-how-do-i-get-a-iso-8601-date-in-string-format
            blockBlob.Metadata.Add("username", userName);
            blockBlob.Metadata.Add("created", created.ToString("yyyy-MM-ddTHH:mm:ss")); // "yyyy-MM-ddTHH:mm:ssZ"
            blockBlob.Metadata.Add("modified", created.ToString("yyyy-MM-ddTHH:mm:ss")); // "yyyy-MM-ddTHH:mm:ssZ"
            blockBlob.Metadata.Add("fileext", ext);
            
            blockBlob.UploadFromStream(fileInputStream);
            blockBlob.SetMetadata();
            
            //await blockBlob.UploadFromStreamAsync(photoToUpload.InputStream);

            // Convert to be HTTP based URI (default storage path is HTTPS)
            var uriBuilder = new UriBuilder(blockBlob.Uri);
            uriBuilder.Scheme = "https";
            var fullPath = uriBuilder.ToString();
            //blockBlob.Properties.ContentMD5;

            blockBlob.FetchAttributes();

            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
            };

            CloudStorageItem item = new CloudStorageItem {                
                PublicKey = publicKey,
                ProviderKey = providerKey,
                OwnerId = userPublicKey.ToString("N"),
                Created = created,
                Modified = created,
                Name = name,
                OriginalName = sourceFileName,
                Description = fileDescription,
                ContentType = blockBlob.Properties.ContentType,
                ContentMD5 = blockBlob.Properties.ContentMD5,
                Url = fullPath,
                //Url = new Uri(blockBlob.Uri.AbsoluteUri + blockBlob.GetSharedAccessSignature(readPolicy,
                //    new SharedAccessBlobHeaders {
                //        ContentDisposition = blockBlob.Metadata.ContainsKey("originalfilename") ? "attachment; filename=" + blockBlob.Metadata["originalfilename"] : "attachment; filename=FileUnknown",
                //        ContentType = blockBlob.Properties.ContentType
                //    }
                //    )).ToString(),
            };

            return item;
        }

        #region ToRemove
        //public static List<CloudStorageItem> ImportStorageItems(string accountName, string accountKey, string containerName) {

        //    Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount =
        //        Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
        //            string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};BlobEndpoint=https://{0}.blob.core.windows.net/", accountName, accountKey)
        //           );
        //    Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        //    Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(containerName);

        //    var list = container.ListBlobs();            
        //    var itemList = new List<CloudStorageItem>();
        //    // http://stackoverflow.com/questions/31183477/md5-hash-of-blob-uploaded-on-azure-doesnt-match-with-same-file-on-local-machine

        //    var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
        //        Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
        //        SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
        //    };

        //    foreach (var blob in list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>()) {
        //        blob.FetchAttributes();
        //        var item = new CloudStorageItem {
        //            ProviderKey = blob.Name,
        //            OwnerId = blob.Metadata.ContainsKey("userid") ? int.Parse(blob.Metadata["userid"]) : 0, // null,
        //            Created = blob.Metadata.ContainsKey("created") ? DateTime.Parse(blob.Metadata["created"]) : DateTime.UtcNow,
        //            Modified = blob.Metadata.ContainsKey("modified") ? DateTime.Parse(blob.Metadata["modified"]) : DateTime.UtcNow,
        //            Name = blob.Metadata.ContainsKey("name") ? blob.Metadata["name"] : blob.Name, // sourceFileName,
        //            Url = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy)).ToString(),
        //            ContentType = blob.Properties.ContentType,
        //            ContentMD5 = blob.Properties.ContentMD5
        //        };
        //        itemList.Add(item);
        //    }
        //    return itemList;
        //}
        #endregion

        
    }
}
