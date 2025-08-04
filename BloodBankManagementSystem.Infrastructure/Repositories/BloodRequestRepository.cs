using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace BloodBankManagementSystem.Infrastructure.Repositories
{
     public class BloodRequestRepository : IBloodRequestRepository
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public BloodRequestRepository(AppDbContext context, IConfiguration config)
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

        return false; 
    }
    catch (Exception)
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


    public async Task<int> BulkUpdateStatusAsync(List<BloodRequestStatusUpdateModel> updates)
{
    using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync();

    using var transaction = connection.BeginTransaction();
    try
    {
        var sql = @"UPDATE BloodRequests 
                    SET Status = @NewStatus, 
                        Notes = @Notes, 
                        ApprovedDate = GETDATE() 
                    WHERE Id = @Id";

        int totalAffected = 0;

        foreach (var update in updates)
        {
            var affected = await connection.ExecuteAsync(sql, new
            {
                update.NewStatus,
                update.Notes,
                update.Id
            }, transaction);

            totalAffected += affected;
        }

        transaction.Commit();
        return totalAffected;
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}


public async Task<int> DeleteBloodRequestAsync(List<BloodRequest> requests)
{
    using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync();

    using var transaction = connection.BeginTransaction();
    try
    {
        var sql = @"UPDATE BloodRequests 
                    SET ActiveYN = @ActiveYN,
                    WHERE Id = @Id";

        int totalAffected = 0;

        foreach (var request in requests)
        {
            var affected = await connection.ExecuteAsync(sql, new
            {   request.ActiveYN,
                request.Id
            }, transaction);
            totalAffected += affected;
        }
        transaction.Commit();
        return totalAffected;
    }
    catch
    {
        transaction.Rollback();
        throw;
    }
}


public async Task<int> SoftDeleteBloodRequestsAsync(List<int> ids)
{

    using var connection = new SqlConnection(_connectionString);
    await connection.OpenAsync();
    var query = "UPDATE BloodRequests SET ActiveYN = 0 WHERE Id IN @Ids";
    return await connection.ExecuteAsync(query, new { Ids = ids });
}


}
}


