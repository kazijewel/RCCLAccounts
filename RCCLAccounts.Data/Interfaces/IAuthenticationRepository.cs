using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvidentFund.Data.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<bool> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(string fullName, string email, string password, string confirmPassword);
    }
}
