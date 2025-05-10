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

    public async Task<BloodRequest> GetRequestByIdAsync(int id)
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

    public async Task UpdateRequestDetailsAsync(BloodRequest requestModel){
        _context.BloodRequests.Update(requestModel);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateRequestStatusAsync(int id, string status)
{
    try
    {
        var request = await _context.BloodRequests.FindAsync(id);
        if (request != null)
        {
            if(status=="Delete"){
                // request.IsActive = false;
                request.ApprovedDate = null;
            }
            if(status=="Approved"){
                request.ApprovedDate = DateTime.Now;
            }
            else{
                request.ApprovedDate = null;
            }
            request.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        return false; // Not found
    }
    catch (Exception)
    {
        // Log exception if needed
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

    public async Task AddBloodRequestHistoryAsync(BloodRequestHistory history){

        try
        {
            _context.BloodRequestHistories.Add(history);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<List<BloodRequestHistory>> GetHistoryByRequestIdAsync(int requestId){
        try
        {
           return await _context.BloodRequestHistories.Where(x=>x.RequestId==requestId).OrderByDescending(y=>y.ActionDate).ToListAsync();
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
}


