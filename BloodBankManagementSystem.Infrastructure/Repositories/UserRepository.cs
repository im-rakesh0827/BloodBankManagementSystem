using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            try
            {
                _context = context;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return users ?? new List<User>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                return new List<User>();
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch(Exception){
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try{
                return await _context.Users.FindAsync(userId);
            }
            catch(Exception){
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try{
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch(Exception){
                throw;
            }
        }

        public async Task DeleteAsync(User user)
        {
          try{
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
          }
          catch(Exception){
            throw;
          }
        }

        public async Task AddUserHistoryAsync(UserHistory history)
        {
            try{
                _context.UserHistories.Add(history);
                await _context.SaveChangesAsync();
            }
            catch(Exception){
                throw;
            }
        }

        public async Task<List<UserHistory>> GetUserHistoryByIdAsync(int userId)
        {
            try{
                return await _context.UserHistories
                                .Where(h => h.UserId == userId)
                                .OrderByDescending(h => h.ActionDate)
                                .ToListAsync();
            }
            catch(Exception){
                throw;
            }
        }
    }
}
