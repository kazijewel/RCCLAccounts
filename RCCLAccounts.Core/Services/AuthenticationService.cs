using RCCLAccounts.Core.Interfaces;
using RCCLAccounts.Core.Models;
using RCCLAccounts.Data.Interfaces;

namespace RCCLAccounts.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationService(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }
        public async Task<bool> LoginAsync(LoginModel model)
        {
            return await _authenticationRepository.LoginAsync(model.Email, model.Password);
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            return await _authenticationRepository.RegisterAsync(
                model.FullName,
                model.Email,
                model.Password,
                model.ConfirmPassword);
        }
    }
}
