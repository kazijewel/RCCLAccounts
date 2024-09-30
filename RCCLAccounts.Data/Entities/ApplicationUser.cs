using Microsoft.AspNetCore.Identity;

namespace RCCLAccounts.Data.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string? UserImage { get; set; }
        public DateTime? LastLoginDateTime { get; set; }
    }
}
