using System.ComponentModel.DataAnnotations;

namespace RCCLAccounts.Core.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="Full Name can't be blank")]
        [Display(Name = "Full Name")]
        public required string FullName { get; set; }

        //[Required(ErrorMessage = "User Name can't be blank")]
        [Display(Name = "User Name")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Phone Number can't be blank")]
        [Display(Name = "Phone Number")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Email should be a proper email address format")]
        [Display(Name = "Email")] 
        public required string Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password",ErrorMessage ="Password and Confirm Password should be same")]
        public required string ConfirmPassword { get; set; }
    }
}
