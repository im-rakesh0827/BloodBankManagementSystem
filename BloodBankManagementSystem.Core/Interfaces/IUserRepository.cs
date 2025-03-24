using BloodBankManagementSystem.Core.Models;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.Core.Interfaces
{
    public interface IUserRepository
    {
       
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user); // New method
        Task DeleteAsync(User user);

    }
}
