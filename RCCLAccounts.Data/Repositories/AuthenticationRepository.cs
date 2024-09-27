using ProvidentFund.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ProvidentFund.Data.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AuthenticationRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public Task<bool> LoginAsync(string email, string password)
        {
            return _userManager.FindByEmailAsync(email).ContinueWith((user) =>
            {
                if (user.Result == null)
                {
                    return false;
                }
                return _userManager.CheckPasswordAsync(user.Result, password).Result;
            });
        }

        public Task<bool> RegisterAsync(string fullName, string email, string password, string confirmPassword)
        {
            return _userManager.CreateAsync(new IdentityUser(email), password).ContinueWith((user) =>
            {
                return user.Result.Succeeded;
            });
        }
    }
}
