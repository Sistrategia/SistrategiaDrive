using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNet.Identity;
using Sistrategia.Drive.Resources;
using Sistrategia.Drive.Business;

namespace Sistrategia.Drive.WebSite.Models
{
    public class HomeIndexViewModel
    {
        public IList<DriveItem> RecentItems { get; set; }
    }

    public class AddDocumentViewModel : IValidatableObject
    {
        public AddDocumentViewModel() {

        }

        [Required]
        public Guid DocumentId { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required, MaxLength(2048), Display(Name = "Nombre del documento")]
        //[Display(ResourceType = typeof(LocalizedStrings), Name = "DocumentName")]
        public string DocumentName { get; set; }

        [Required, Display(Name = "Descripción")] // MaxLength(256)
        public string Description { get; set; }

        [Required, DataType(DataType.Upload), Display(Name = "Documento")]
        public HttpPostedFileBase File { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (this.DocumentId == null)
                yield return new ValidationResult("Seleccione el contacto origen del documento");
            //else if (this.GroupId == null && (this.ContactType.Equals("group") && this.GroupName == null))
            //    yield return new ValidationResult("El campo Nombre del grupo es obligatorio");

        }
    }

    public class HomeDetailViewModel
    {
        private Sistrategia.Drive.Business.CloudStorageMananger manager = null;

        public HomeDetailViewModel() {
        }

        public HomeDetailViewModel(Sistrategia.Drive.Business.CloudStorageMananger manager) {
            this.manager = manager;
        }

        private Sistrategia.Drive.Business.CloudStorageMananger Manager {
            get {
                return this.manager ?? new Sistrategia.Drive.Business.CloudStorageMananger();
            }
        }

        public Sistrategia.Drive.Business.DriveItem DriveItem { get; set; }
        public bool IsImage { get; set; }

        public object GetTempUrl() {
            return this.Manager.GetTempUrl(this.DriveItem.Url);
        }

        public object GetTempDownloadUrl() {
            return this.Manager.GetTempDownloadUrl(this.DriveItem.Url);
        }
    }
}