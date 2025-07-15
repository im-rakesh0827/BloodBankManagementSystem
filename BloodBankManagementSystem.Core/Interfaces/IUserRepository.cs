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
        Task UpdateUserAsync(User user);
        Task DeleteAsync(User user);

        Task AddUserHistoryAsync(UserHistory userHistory);
        Task<List<UserHistory>> GetUserHistoryByIdAsync(int userId);


        Task<bool> UpdateOtpAsync(string email, string otpCode, DateTime otpExpiry);
        Task<bool> VerifyOtpAsync(string email, string otpCode);
        Task<bool> UpdatePasswordAsync(string email, string newPasswordHash);


    }

    
}
