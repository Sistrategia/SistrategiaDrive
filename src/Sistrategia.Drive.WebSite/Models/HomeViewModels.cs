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
        public IList<DriveItem> RecentItems { get; set; }
    }

    public class HomeDetailViewModel
    {
        private Sistrategia.Drive.Business.CloudStorageMananger manager = null;

        //public HomeDetailViewModel() {
        //}

        public HomeDetailViewModel(Sistrategia.Drive.Business.CloudStorageMananger manager) {
            this.manager = manager;
        }

        private Sistrategia.Drive.Business.CloudStorageMananger Manager {
            get {
                return this.manager; // ?? new Sistrategia.Drive.Business.CloudStorageMananger();
            }
        }

        public Sistrategia.Drive.Business.DriveItem DriveItem { get; set; }
        public bool IsImage { get; set; }

        //public object GetTempUrl() {
        //    return this.Manager.GetTempUrl(this.DriveItem.Url);
        //}

        //public object GetTempDownloadUrl() {
        //    return this.Manager.GetTempDownloadUrl(this.DriveItem.Url);
        //}

        public object GetTempUrl() {
            return this.DriveItem.GetTempUrl();
        }

        public object GetTempDownloadUrl() {
            return this.DriveItem.GetTempDownloadUrl();
        }
    }
}