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
        public Guid? PublicKey { get; set; }

        public string ProviderKey { get; set; }

        //[ForeignKey("Owner")] // [Required]
        //public int OwnerId { get; set; }        
        //public virtual SecurityUser Owner { get; set; }

        public string OwnerId { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public string ContentType { get; set; }
        public string ContentMD5 { get; set; }
        public string OriginalName { get; set; }
        public string Url { get; set; }
    }
}
