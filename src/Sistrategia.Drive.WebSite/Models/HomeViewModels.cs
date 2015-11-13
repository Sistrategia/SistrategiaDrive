using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Sistrategia.Drive.Resources;
using Sistrategia.Drive.Business;
//using Microsoft.Owin.Security;

namespace Sistrategia.Drive.WebSite.Models
{
    public class HomeIndexViewModel
    {
        public IList<CloudStorageItem> RecentItems { get; set; }
    }
}