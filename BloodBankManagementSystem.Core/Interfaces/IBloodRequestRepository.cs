using BloodBankManagementSystem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace BloodBankManagementSystem.Core.Interfaces
{
     public interface IBloodRequestRepository{
          Task AddRequestAsync(BloodRequest request);
          Task<List<BloodRequest>> GetAllRequestsAsync();
          Task<BloodRequest?> GetRequestByIdAsync(int id);
          Task UpdateRequestDetailsAsync(BloodRequest request);
          Task<bool> UpdateRequestStatusAsync(int id, string status);
          Task DeleteRequestAsync(int id);
          Task AddBloodRequestHistoryAsync(BloodRequestHistory history);
          Task<List<BloodRequestHistory>> GetHistoryByRequestIdAsync(int requestId);
          Task<int> BulkUpdateStatusAsync(List<BloodRequestStatusUpdateModel> updates);
          Task<int> DeleteBloodRequestAsync(List<BloodRequest> request);
          Task<int> SoftDeleteBloodRequestsAsync(List<int> ids);
     }
}