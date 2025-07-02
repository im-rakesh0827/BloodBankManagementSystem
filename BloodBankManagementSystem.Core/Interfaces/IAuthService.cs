using BloodBankManagementSystem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.Core.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task LogoutAsync();
    }
}
