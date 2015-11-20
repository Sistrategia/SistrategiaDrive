using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistrategia.Drive.Business
{
    public class CloudStorageItem
    {
        public CloudStorageItem() {
            this.PublicKey = Guid.NewGuid();
        }

        [Key] //, Required]
        public int CloudStorageItemId { get; set; }

        [ForeignKey("CloudStorageContainer")]
        public int CloudStorageContainerId { get; set; }
        public virtual CloudStorageContainer CloudStorageContainer { get; set; }

        [Required]
        public Guid PublicKey { get; set; }

        [Required, MaxLength(1024)]
        public string ProviderKey { get; set; }

        //[ForeignKey("Owner")] // [Required]
        //public int OwnerId { get; set; }        
        //public virtual SecurityUser Owner { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "DocumentName")]
        [Required, MaxLength(2048)] // , Display(Name = "Nombre del documento")]        
        public string Name { get; set; }

        // [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Modified { get; set; }

        [Required, MaxLength(255)]
        public string ContentType { get; set; }

        public string ContentMD5 { get; set; }

        [MaxLength(2048)]
        public string OriginalName { get; set; }

        public string Url { get; set; }


        public string GetTempUrl() {
            throw new NotImplementedException();
        }

        public string GetTempDownloadUrl() {
            throw new NotImplementedException();
        }
    }
}


//public string GetTempUrl(string fullPath) {
//            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = this.GetDefaultCloudStorageAccount();
//            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
//            var blob = blobClient.GetBlobReferenceFromServer(new Uri(fullPath));

//            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
//                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
//                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
//            };

//            string resultUrl = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy)).ToString();

//            return resultUrl;
//        }

//        public string GetTempDownloadUrl(string fullPath) {
//             Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = this.GetDefaultCloudStorageAccount();
//            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
//            var blob = blobClient.GetBlobReferenceFromServer(new Uri(fullPath));

//            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
//                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
//                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
//            };

//            string resultUrl = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy,
//                    new SharedAccessBlobHeaders {
//                        ContentDisposition = blob.Metadata.ContainsKey("originalfilename") ? "attachment; filename=" + blob.Metadata["originalfilename"] : "attachment; filename=FileUnknown",
//                        ContentType = blob.Properties.ContentType
//                    }
//                    )).ToString();

//            return resultUrl;
//        }