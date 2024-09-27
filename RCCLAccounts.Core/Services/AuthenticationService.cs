using ProvidentFund.Core.Interfaces;
using ProvidentFund.Core.Models;
using ProvidentFund.Data.Interfaces;

namespace ProvidentFund.Core.Services
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
