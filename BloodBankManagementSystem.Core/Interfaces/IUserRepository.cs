using BloodBankManagementSystem.Core.Models;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
    }
}
