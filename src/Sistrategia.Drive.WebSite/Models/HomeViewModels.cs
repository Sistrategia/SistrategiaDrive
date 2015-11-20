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
        private CloudStorageMananger manager = null;

        //public HomeDetailViewModel() {
        //}

        public HomeDetailViewModel(CloudStorageMananger manager) {
            this.manager = manager;
        }

        private CloudStorageMananger Manager {
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

        public string GetTempUrl() {
            return this.DriveItem.GetTempUrl(Manager);
        }

        public string GetTempDownloadUrl() {
            return this.DriveItem.GetTempDownloadUrl(Manager);
        }
    }
}