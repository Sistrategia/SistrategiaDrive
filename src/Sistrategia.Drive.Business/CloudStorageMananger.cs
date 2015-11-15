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
        public static void GetBlobs() {
            
        }

        public List<CloudStorageItem> GetCloudStorageItems() {

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount =                
                Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
                       System.Configuration.ConfigurationManager.AppSettings["AzureDefaultStorageConnectionString"]
                   );
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(System.Configuration.ConfigurationManager.AppSettings["AzureDefaultStorage"]);
            // Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(System.Configuration.ConfigurationManager.AppSettings["AzureUserProfilesStorage"]);
            var list = container.ListBlobs();
            //var itemList = new List<DocumentListItem>();
            var itemList = new List<CloudStorageItem>();

            // http://stackoverflow.com/questions/31183477/md5-hash-of-blob-uploaded-on-azure-doesnt-match-with-same-file-on-local-machine

            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
            };

            foreach (var blob in list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>()) {
                blob.FetchAttributes();
                var item = new CloudStorageItem {
                    // CloudStorageItemId = blob.Name.Substring(0, blob.Name.IndexOf('.')), // documentId.ToString("N"),
                    OwnerId = blob.Metadata["userid"],
                    Created = DateTime.Parse(blob.Metadata["created"]),
                    Modified = DateTime.Parse(blob.Metadata["modified"]),
                    Name = blob.Metadata["name"], // sourceFileName,
                    //Description = blob.Metadata["description"], // fileDescription,
                    Url = new Uri( blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy)).ToString(),
                    ContentMD5 = blob.Properties.ContentMD5
                };

                //var item = new DocumentListItem() {
                //    Name = blob.Name,
                //    ContentMD5 = blob.Properties.ContentMD5,
                //    //blob.BlobType.ToString();
                //    //blob.IsSnapshot
                //    //blob.Metadata
                //    //blob.Properties.
                //    //blob.Properties.ContentMD5;
                //    //blob.Uri
                //    Url = new UriBuilder(blob.Uri).ToString()
                //};
                itemList.Add(item);
                //BlobProperties bp = blob.Name . as BlobProperties;
                //if (bp != null) {
                //    BlobProperties p = _Container.GetBlobProperties(bp.Name);
                //    var name = p.Name; // get the name
                //}
            }

            //List<string> blobNames = list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>().Select(b => b.Name).ToList();

            return itemList;
            
            //foreach (var blobName in blobNames) {

            //}

            //Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount =
            //    // new Microsoft.WindowsAzure.Storage.CloudStorageAccount(
            //            Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
            //            System.Configuration.ConfigurationManager.AppSettings["AzureDefaultStorageConnectionString"]
            //        );

            //Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            //Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(System.Configuration.ConfigurationManager.AppSettings["AzureDefaultStorage"]);
            //// Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(System.Configuration.ConfigurationManager.AppSettings["AzureUserProfilesStorage"]);
            //var list = container.ListBlobs();
            //var itemList = new List<DocumentListItem>();

            //// http://stackoverflow.com/questions/31183477/md5-hash-of-blob-uploaded-on-azure-doesnt-match-with-same-file-on-local-machine

            //foreach (var blob in list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>()) {
            //    var item = new DocumentListItem() {
            //        Name = blob.Name,
            //        ContentMD5 = blob.Properties.ContentMD5,
            //        //blob.BlobType.ToString();
            //        //blob.IsSnapshot
            //        //blob.Metadata
            //        //blob.Properties.
            //        //blob.Properties.ContentMD5;
            //        //blob.Uri
            //        Url = new UriBuilder(blob.Uri).ToString()
            //    };
            //    itemList.Add(item);
            //    //BlobProperties bp = blob.Name . as BlobProperties;
            //    //if (bp != null) {
            //    //    BlobProperties p = _Container.GetBlobProperties(bp.Name);
            //    //    var name = p.Name; // get the name
            //    //}
            //}

            ////List<string> blobNames = list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>().Select(b => b.Name).ToList();

            //DocumentListModel model = new DocumentListModel {
            //    DocumentList = itemList // blobNames
            //};
            ////foreach (var blobName in blobNames) {

            ////}
        }

        public CloudStorageItem UploadFromStream(string userId, string userName, string sourceFileName, string fileContentType, System.IO.Stream fileInputStream, string name, string fileDescription) {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount =
                // new Microsoft.WindowsAzure.Storage.CloudStorageAccount(
                        Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
                        System.Configuration.ConfigurationManager.AppSettings["AzureDefaultStorageConnectionString"]
                    );

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(System.Configuration.ConfigurationManager.AppSettings["AzureDefaultStorage"]);
            //Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(System.Configuration.ConfigurationManager.AppSettings["AzureDefaultStorage"]);
            //CloudBlobContainer container = blobClient.GetContainerReference("wwwroot");

            Guid documentId = Guid.NewGuid();

            string ext = System.IO.Path.GetExtension(sourceFileName);

            string fileName = String.Format(
                //"{0}-{1}{2}",
                //DateTime.Now.ToString("yyyy-MM-dd"),
                        "{0}{1}",
                        documentId.ToString("N"),
                        System.IO.Path.GetExtension(sourceFileName));

            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = fileContentType;
            blockBlob.Metadata.Add("name", name);            
            blockBlob.Metadata.Add("originalfilename", sourceFileName);
            blockBlob.Metadata.Add("userid", userId);            

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

            CloudStorageItem item = new CloudStorageItem {
                //CloudStorageItemId = documentId.ToString("N"),
                ProviderKey = fileName,
                OwnerId = userId,
                Created = created,
                Modified = created,
                Name = sourceFileName,
                Description = fileDescription,
                ContentMD5 = blockBlob.Properties.ContentMD5
            };

            return item;
        }

        public static List<CloudStorageContainer> ImportContainers(string accountName, string accountKey) {

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = //new Microsoft.WindowsAzure.Storage.CloudStorageAccount()
                Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
                    string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};BlobEndpoint=https://{0}.blob.core.windows.net/", accountName, accountKey)                       
                   );
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var containers = blobClient.ListContainers();

            //Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(System.Configuration.ConfigurationManager.AppSettings["AzureDefaultStorage"]);
            // Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(System.Configuration.ConfigurationManager.AppSettings["AzureUserProfilesStorage"]);

            List<CloudStorageContainer> list = new List<CloudStorageContainer>();

            foreach (var azureContainer in containers.OrderBy(c => c.Name)) {
                CloudStorageContainer container = new CloudStorageContainer {
                    //CloudStorageContainerId = azureContainer.Name,
                    //CloudStorageContainerId = Guid.NewGuid().ToString("D").ToLower(),
                    ProviderKey = azureContainer.Name,
                    ContainerName = azureContainer.Name,
                    Alias = azureContainer.Name
                };
                list.Add(container);
            }

            return list;
            
        }




        public static List<CloudStorageItem> ImportStorageItems(string accountName, string accountKey, string containerName) {

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount =
                Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
                    string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};BlobEndpoint=https://{0}.blob.core.windows.net/", accountName, accountKey)
                   );
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            var list = container.ListBlobs();
            //var itemList = new List<DocumentListItem>();
            var itemList = new List<CloudStorageItem>();

            // http://stackoverflow.com/questions/31183477/md5-hash-of-blob-uploaded-on-azure-doesnt-match-with-same-file-on-local-machine

            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
            };

            foreach (var blob in list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>()) {
                blob.FetchAttributes();
                var item = new CloudStorageItem {
                    //CloudStorageItemId = blob.Name.IndexOf('.') > 0 ? blob.Name.Substring(0, blob.Name.IndexOf('.')) : blob.Name, // documentId.ToString("N"),
                    //CloudStorageItemId = blob.Metadata.ContainsKey("cloudstorageitemid") ? blob.Metadata["cloudstorageitemid"] : Guid.NewGuid().ToString("D").ToLower(),
                    ProviderKey = blob.Name,
                    OwnerId = blob.Metadata.ContainsKey("userid") ? blob.Metadata["userid"] : null,
                    Created = blob.Metadata.ContainsKey("created") ? DateTime.Parse(blob.Metadata["created"]) : DateTime.UtcNow,
                    Modified = blob.Metadata.ContainsKey("modified") ? DateTime.Parse(blob.Metadata["modified"]) : DateTime.UtcNow,
                    Name = blob.Metadata.ContainsKey("modified") ? blob.Metadata["name"] : blob.Name, // sourceFileName,
                    //Description = blob.Metadata["description"], // fileDescription,
                    Url = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy)).ToString(),
                    ContentType = blob.Properties.ContentType,
                    ContentMD5 = blob.Properties.ContentMD5
                };

                //var item = new DocumentListItem() {
                //    Name = blob.Name,
                //    ContentMD5 = blob.Properties.ContentMD5,
                //    //blob.BlobType.ToString();
                //    //blob.IsSnapshot
                //    //blob.Metadata
                //    //blob.Properties.
                //    //blob.Properties.ContentMD5;
                //    //blob.Uri
                //    Url = new UriBuilder(blob.Uri).ToString()
                //};
                itemList.Add(item);
                //BlobProperties bp = blob.Name . as BlobProperties;
                //if (bp != null) {
                //    BlobProperties p = _Container.GetBlobProperties(bp.Name);
                //    var name = p.Name; // get the name
                //}
            }

            //List<string> blobNames = list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>().Select(b => b.Name).ToList();

            return itemList;

            // throw new NotImplementedException();
        }

        //public static List<CloudStorageItem> GetCloudStorageItems(string accountName, string accountKey, string containerName) {

        //    Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = 
        //        Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
        //            string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};BlobEndpoint=https://{0}.blob.core.windows.net/", accountName, accountKey)
        //           );
        //    Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
        //    Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            
        //    var list = container.ListBlobs();
        //    //var itemList = new List<DocumentListItem>();
        //    var itemList = new List<CloudStorageItem>();

        //    // http://stackoverflow.com/questions/31183477/md5-hash-of-blob-uploaded-on-azure-doesnt-match-with-same-file-on-local-machine

        //    var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
        //        Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
        //        SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
        //    };

        //    foreach (var blob in list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>()) {
        //        blob.FetchAttributes();
        //        var item = new CloudStorageItem {
        //            CloudStorageItemId = blob.Name.IndexOf('.') > 0 ? blob.Name.Substring(0, blob.Name.IndexOf('.')) : blob.Name, // documentId.ToString("N"),
        //            OwnerId = blob.Metadata.ContainsKey("userid") ? blob.Metadata["userid"] : null,
        //            Created = blob.Metadata.ContainsKey("created") ? DateTime.Parse(blob.Metadata["created"]) : DateTime.UtcNow,
        //            Modified = blob.Metadata.ContainsKey("modified") ? DateTime.Parse(blob.Metadata["modified"]) : DateTime.UtcNow,
        //            Name = blob.Metadata.ContainsKey("modified") ? blob.Metadata["name"] : blob.Name, // sourceFileName,
        //            //Description = blob.Metadata["description"], // fileDescription,
        //            Url = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy)).ToString(),
        //            ContentType = blob.Properties.ContentType,
        //            ContentMD5 = blob.Properties.ContentMD5
        //        };

        //        //var item = new DocumentListItem() {
        //        //    Name = blob.Name,
        //        //    ContentMD5 = blob.Properties.ContentMD5,
        //        //    //blob.BlobType.ToString();
        //        //    //blob.IsSnapshot
        //        //    //blob.Metadata
        //        //    //blob.Properties.
        //        //    //blob.Properties.ContentMD5;
        //        //    //blob.Uri
        //        //    Url = new UriBuilder(blob.Uri).ToString()
        //        //};
        //        itemList.Add(item);
        //        //BlobProperties bp = blob.Name . as BlobProperties;
        //        //if (bp != null) {
        //        //    BlobProperties p = _Container.GetBlobProperties(bp.Name);
        //        //    var name = p.Name; // get the name
        //        //}
        //    }

        //    //List<string> blobNames = list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>().Select(b => b.Name).ToList();

        //    return itemList;
            
        //   // throw new NotImplementedException();
        //}
    }

    
}
