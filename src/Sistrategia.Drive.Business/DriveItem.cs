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
            this.PublicKey = cloudStorageItem.PublicKey ?? Guid.NewGuid();
            this.ProviderKey = cloudStorageItem.ProviderKey;
            this.Name = cloudStorageItem.Name;
            this.Description = cloudStorageItem.Description;
            this.Created = cloudStorageItem.Created;
            this.Modified = cloudStorageItem.Modified;
            this.ContentType = cloudStorageItem.ContentType;
            this.ContentMD5 = cloudStorageItem.ContentMD5;
            this.OriginalName = cloudStorageItem.OriginalName;
            this.Url = cloudStorageItem.Url;
        }
        [Key]
        public int DriveItemId { get; set; }
        [Required]
        public Guid PublicKey { get; set; }

        [ForeignKey("Owner")] // [Required]
        public int OwnerId { get; set; }
        public virtual SecurityUser Owner { get; set; }

        //[Display(ResourceType = typeof(LocalizedStrings), Name = "DocumentName")]
        [Required, MaxLength(2048)] // , Display(Name = "Nombre del documento")]        
        public string Name { get; set; }

        // [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Modified { get; set; }




        [Required, MaxLength(1024)]
        public string ProviderKey { get; set; }

        [Required, MaxLength(255)]
        public string ContentType { get; set; }

        [MaxLength(128)]
        public string ContentMD5 { get; set; }

        [MaxLength(2048)]
        public string OriginalName { get; set; }

        [MaxLength(2048)]
        public string Url { get; set; }
    }
}
