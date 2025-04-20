using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.Infrastructure.Repositories
{
     public class BloodRequestRepository : IBloodRequestRepository
{
    private readonly AppDbContext _context;

    public BloodRequestRepository(AppDbContext context)
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

    public async Task AddRequestAsync(BloodRequest request)
    {
        try
        {
            _context.BloodRequests.Add(request);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task<List<BloodRequest>> GetAllRequestsAsync()
    {
        try
        {
            return await _context.BloodRequests.ToListAsync();
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task<BloodRequest?> GetRequestByIdAsync(int id)
    {
        try
        {
            return await _context.BloodRequests.FindAsync(id);
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

    public async Task UpdateRequestStatusAsync(int id, string status)
    {
        try
        {
            var request = await _context.BloodRequests.FindAsync(id);
            if (request != null)
            {
                request.Status = status;
                await _context.SaveChangesAsync();
            }
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }

     public async Task DeleteRequestAsync(int id)
    {
        var req = await _context.BloodRequests.FindAsync(id);
        if (req != null)
        {
            _context.BloodRequests.Remove(req);
            await _context.SaveChangesAsync();
        }
    }
}
}


