using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.Infrastructure.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly AppDbContext _context;

        public DonorRepository(AppDbContext context)
        {
            try
            {
                _context = context;
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<Donor?> GetDonorByEmailAsync(string email)
        {
            try
            {
                return await _context.Donors.FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task<List<Donor>> GetAllDonorsAsync()
        {
            try
            {
                var donors = await _context.Donors.ToListAsync();
                return donors ?? new List<Donor>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database Error: {ex.Message}");
                return new List<Donor>();
            }
        }

        public async Task AddDonorAsync(Donor donor)
        {
           try
           {
             _context.Donors.Add(donor);
             await _context.SaveChangesAsync();
           }
           catch (System.Exception)
           {
            throw;
           }
        }

        public async Task<Donor> GetDonorByIdAsync(int donorId)
        {
            try
            {
                return await _context.Donors.FindAsync(donorId);
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task UpdateDonorAsync(Donor donor)
        {
            try
            {
                _context.Donors.Update(donor);
                await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public async Task DeleteAsync(Donor donor)
        {
          try
          {
            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
          }
          catch (System.Exception)
          {
            
            throw;
          }
        }



    public async Task AddDonorHistoryAsync(DonorHistory history)
    {
        try
        {
            _context.DonorHistories.Add(history);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task<List<DonorHistory>> GetHistoryByDonorIdAsync(int donorId)
    {
        try
        {
            return await _context.DonorHistories
                            .Where(h => h.DonorId == donorId)
                            .OrderByDescending(h => h.ActionDate)
                            .ToListAsync();
        }
        catch (System.Exception)
        {
            throw;
        }
    }       
    }
}
