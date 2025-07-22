using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
// using System.Data.SqlClient;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace BloodBankManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public UserRepository(AppDbContext context, IConfiguration config)
        {
            try
            {
                _context = context;
                _config = config;
                _connectionString = _config.GetConnectionString("DefaultConnection")!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // public async Task<User?> GetUserByEmailAsync(string email)
        // {
        //     try
        //     {
        //         return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        //     }
        //     catch (Exception)
        //     {
        //         throw;
        //     }
        // }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Users WHERE Email = @Email";
            // return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });

        }

         

        // public async Task<List<User>> GetAllUsersAsync()
        // {
        //     try
        //     {
        //         var users = await _context.Users.ToListAsync();
        //         return users ?? new List<User>();
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Database Error: {ex.Message}");
        //         return new List<User>();
        //     }
        // }

        public async Task<List<User>> GetAllUsersAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT * FROM Users";
            var result = await connection.QueryAsync<User>(sql);
            return result.ToList();
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


    public async Task<bool> UpdateOtpAsync(string email, string otpCode, DateTime otpExpiry)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "UPDATE Users SET OtpCode = @OtpCode, OtpExpiry = @OtpExpiry WHERE Email = @Email";
        var rows = await connection.ExecuteAsync(sql, new { OtpCode = otpCode, OtpExpiry = otpExpiry, Email = email });
        return rows > 0;
    }

    public async Task<bool> VerifyOtpAsync(string email, string otpCode)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "SELECT * FROM Users WHERE Email = @Email AND OtpCode = @OtpCode AND OtpExpiry > GETUTCDATE()";
        var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email, OtpCode = otpCode });
        return user != null;
    }

    public async Task<bool> UpdatePasswordAsync(string email, string newPasswordHash)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = "UPDATE Users SET PasswordHash = @PasswordHash, OtpCode = NULL, OtpExpiry = NULL WHERE Email = @Email";
        var rows = await connection.ExecuteAsync(sql, new { PasswordHash = newPasswordHash, Email = email });
        return rows > 0;
    }






        
    }
}
