using RCCLAccounts.Core.Models;

namespace RCCLAccounts.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync(LoginModel model);
        Task<bool> RegisterAsync(RegisterModel model);
    }
}
