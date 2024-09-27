using System.ComponentModel.DataAnnotations;

namespace ProvidentFund.Core.Models
{
	public class LoginModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email Address")]
		public required string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		//[StringLength(100, MinimumLength = 8)]
		public required string Password { get; set; }

		[Display(Name = "Remember Me")]
		public bool RememberMe { get; set; } = false;

	}
}
