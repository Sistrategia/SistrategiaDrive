using Sistrategia.Drive.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sistrategia.Drive.Resources;

namespace Sistrategia.Drive.WebSite.Areas.Backstage.Models
{
    public class UserIndexViewModel
    {
        public List<SecurityUser> Users { get; set; }        
    }

    public class UserDeleteViewModel
    {
        public SecurityUser User { get; set; }
    }

    public class UserCreateViewModel
    {
        //[Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "UserNameRequired")]
        //[EmailAddress]
        //[Display(ResourceType = typeof(LocalizedStrings), Name = "UserName")]
        //public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "FullNameRequired")]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FullNameField")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(100,
            MinimumLength = 6,
            ErrorMessageResourceType = typeof(LocalizedStrings),
            ErrorMessageResourceName = "Account_PasswordValidationError"
            //ErrorMessage = "The {0} must be at least {2} characters long."
            )]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Account_ConfirmPassword")]
        [Compare("Password",
            ErrorMessageResourceType = typeof(LocalizedStrings),
            //ErrorMessage = "The password and confirmation password do not match."
            ErrorMessageResourceName = "Account_ConfirmPasswordDoesNotMatchError"
            )]
        public string ConfirmPassword { get; set; }
    }
}