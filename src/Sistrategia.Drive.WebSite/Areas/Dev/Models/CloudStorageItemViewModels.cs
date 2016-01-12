using Sistrategia.Drive.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistrategia.Drive.WebSite.Areas.Dev.Models
{
    public class CloudStorageItemDetailsViewModel
    {
        public CloudStorageItem CloudStorageItem { get; set; }
        public string Url { get; set; }
        public bool IsImage { get; set; }

    }
}