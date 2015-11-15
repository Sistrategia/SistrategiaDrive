using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Sistrategia.Drive.Resources;
using System.Web;
//using Microsoft.Owin.Security;

namespace Sistrategia.Drive.WebSite.Models
{
    public class DocumentListModel
    {
        //public List<DocumentListItem> DocumentList { get; set; }
        public List<Sistrategia.Drive.Business.CloudStorageItem> DocumentList { get; set; }
    }

    public class CloudStorageContainerListModel
    {
        public Sistrategia.Drive.Business.CloudStorageContainer Container { get; set; }
        public List<Sistrategia.Drive.Business.CloudStorageItem> CloudStorageItems { get; set; }
    }

    //public class DocumentListItem
    //{
    //    public string Name { get; set; }
    //    public string ContentMD5 { get; set; }
    //    public string Url { get; set; }
    //}

    public class AddDocumentViewModel : IValidatableObject
    {
        public AddDocumentViewModel() {
            //this.ContactTypes = new SelectList(new Dictionary<string, string> { 
            //    { "person", "Persona" } ,
            //    { "organization", "Organización" } ,
            //    { "group", "Grupo" } 
            //}, "Key", "Value");
        }

        [Required]
        public Guid DocumentId { get; set; }

        //[Required, MaxLength(128)]
        //public string OwnerId { get; set; }
        [Required]
        public int OwnerId { get; set; }        

        [Required, MaxLength(2048), Display(Name = "Nombre del documento")]
        //[Display(ResourceType = typeof(LocalizedStrings), Name = "DocumentName")]
        public string DocumentName { get; set; }

        [Required, Display(Name = "Descripción")] // MaxLength(256)
        public string Description { get; set; }

        [Required, DataType(DataType.Upload), Display(Name = "Documento")]
        public HttpPostedFileBase File { get; set; }

        //public Guid? ContactId { get; set; }

        //public IEnumerable<System.Web.Mvc.SelectListItem> ContactTypes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            if (this.DocumentId == null)
                yield return new ValidationResult("Seleccione el contacto origen del documento");            
            //else if (this.GroupId == null && (this.ContactType.Equals("group") && this.GroupName == null))
            //    yield return new ValidationResult("El campo Nombre del grupo es obligatorio");
            
        }
    }
}