using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistrategia.Drive.Business
{
    //public enum CloudStorageAccountType
    //{
    //    Azure,
    //    Amazon,
    //    Google,
    //    Rackspace
    //}

    public class CloudStorageAccount
    {
        public string CloudStorageAccountId { get; set; }

        //[Column(Order = 2)]
        [ForeignKey("CloudStorageProvider")]
        public string CloudStorageProviderId { get; set; }

        public virtual CloudStorageProvider CloudStorageProvider { get; set; }

        [MaxLength(512)]
        public string AccountName { get; set; }
        
        public string AccountKey { get; set; }

        [MaxLength(256)]
        public string Alias { get; set; }

        public string Description { get; set; }

        //public virtual IList<SecurityUser> SecurityUsers { get; set; }

        public virtual IList<CloudStorageContainer> CloudStorageContainers { get; set; }

        
    }
}
