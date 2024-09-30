using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace RCCLAccounts.Core.Models
{
    public class UserProfileModel
    {
        [Display(Name = "Full Name")]
        public  string? FullName { get; set; }

        //[Required(ErrorMessage = "User Name can't be blank")]
        [Display(Name = "User Name")]
        public string? UserName { get; set; }

        [Display(Name = "User Image")]
        public string? UserImage { get; set; }

        [Display(Name = "Upload User Image:")]
        public IFormFile? ImageUp { get; set; }

        [Display(Name = "Phone Number")]
        public  string? PhoneNumber { get; set; }

        [Display(Name = "Email")] 
        public  string? Email { get; set; }

       
    }
}
