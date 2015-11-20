using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistrategia.Drive.Business
{
    public class DriveItem
    {
        public DriveItem() {
            this.PublicKey = Guid.NewGuid();
        }

        public DriveItem(CloudStorageItem cloudStorageItem) {
            //this.PublicKey = cloudStorageItem.PublicKey ?? Guid.NewGuid();
            this.PublicKey = Guid.NewGuid();
            //this.ProviderKey = cloudStorageItem.ProviderKey;
            this.Name = cloudStorageItem.Name;
            this.Description = cloudStorageItem.Description;
            this.Created = cloudStorageItem.Created;
            this.Modified = cloudStorageItem.Modified;
            this.ContentType = cloudStorageItem.ContentType;
            this.ContentMD5 = cloudStorageItem.ContentMD5;
            this.OriginalName = cloudStorageItem.OriginalName;
            this.Url = cloudStorageItem.Url;
            this.CloudStorageItem = cloudStorageItem;
        }

        [Key]
        public int DriveItemId { get; set; }

        [Required]
        public Guid PublicKey { get; set; }

        //[Required, MaxLength(1024)]
        //public string ProviderKey { get; set; }

        [ForeignKey("Owner")] // [Required]
        public int OwnerId { get; set; }
        public virtual SecurityUser Owner { get; set; }

        [ForeignKey("CloudStorageItem")] // [Required]
        public int CloudStorageItemId { get; set; }
        public virtual CloudStorageItem CloudStorageItem { get; set; }

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
            //throw new NotImplementedException();
            return this.CloudStorageItem.GetTempUrl();
        }

        public string GetTempDownloadUrl() {
            return this.CloudStorageItem.GetTempDownloadUrl();
        }

    }
}
