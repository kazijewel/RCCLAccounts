using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCCLAccounts.Core.Models
{
    public class  PasswordChangeModel
    {

        [Required(ErrorMessage = "Please enter your old password.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please enter your new password.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{6,}$", ErrorMessage = "Invalid password format.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password.")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }
}
