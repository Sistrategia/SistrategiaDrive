using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Sistrategia.Drive.Resources;
using Sistrategia.Drive.Business;
using Microsoft.Owin.Security;
//using Microsoft.Owin.Security;

namespace Sistrategia.Drive.WebSite.Models
{
    public class AccountIndexViewModel
    {
        public AccountIndexViewModel() {
            this.CloudStorageAccounts = new List<CloudStorageAccount>();
        }

        public string UserName { get; set; }
        public string FullName { get; set; }
        
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }

        public IList<CloudStorageAccount> CloudStorageAccounts { get; set; }
    }

    public enum AccountIndexMessageId
    {
        AddPhoneSuccess,
        ChangePasswordSuccess,
        SetTwoFactorSuccess,
        SetPasswordSuccess,
        RemoveLoginSuccess,
        RemovePhoneSuccess,
        Error
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Password")]
        public string Password { get; set; }

        //[Display(Name = "Remember me?")]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Account_RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Email")]
        public string Email { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "FullNameRequired")]        
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FullNameField")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(ResourceType=typeof(LocalizedStrings), Name = "Email")]
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

        //[Display(Name = "Hometown")]
        //public string Hometown { get; set; }
    }

    public class ResetPasswordViewModel
    {
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

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Email")]
        public string Email { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    // Used when register with external login
    public class ExternalLoginConfirmationViewModel
    {
        //[Required]
        //[Display(ResourceType = typeof(LocalizedStrings), Name = "Email")]
        //public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "FullNameRequired")]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FullNameField")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Email")]
        public string Email { get; set; }

    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }



    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

}