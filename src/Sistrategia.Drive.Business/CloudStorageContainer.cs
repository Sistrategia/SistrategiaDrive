using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistrategia.Drive.Business
{
    public class CloudStorageContainer
    {
        public string CloudStorageContainerId { get; set; }

        //[Column(Order = 2)]
        [ForeignKey("CloudStorageAccount")]
        public string CloudStorageAccountId { get; set; }
        public virtual CloudStorageAccount CloudStorageAccount { get; set; }

        [MaxLength(512)]
        public string ContainerName { get; set; }

        //public string ContainerKey { get; set; }

        [MaxLength(256)]
        public string Alias { get; set; }

        public string Description { get; set; }

        //public virtual IList<SecurityUser> SecurityUsers { get; set; }
        public virtual IList<CloudStorageItem> CloudStorageItems { get; set; }
    }
}
