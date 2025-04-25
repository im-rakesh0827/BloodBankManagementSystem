using BloodBankManagementSystem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BloodBankManagementSystem.Core.Interfaces
{
     public interface IBloodRequestRepository{
          Task AddRequestAsync(BloodRequest request);
          Task<List<BloodRequest>> GetAllRequestsAsync();
          Task<BloodRequest?> GetRequestByIdAsync(int id);
          // Task UpdateRequestDetails(int id, BloodRequest request);
          Task UpdateRequestDetailsAsync(BloodRequest request);
          Task<bool> UpdateRequestStatusAsync(int id, string status);

          Task DeleteRequestAsync(int id);
     }
}