using ProvidentFund.Core.Models;

namespace ProvidentFund.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> LoginAsync(LoginModel model);
        Task<bool> RegisterAsync(RegisterModel model);
    }
}
