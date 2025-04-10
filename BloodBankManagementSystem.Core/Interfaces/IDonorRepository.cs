using BloodBankManagementSystem.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.Core.Interfaces
{
    public interface IDonorRepository
    {
        Task<List<Donor>> GetAllDonorsAsync();
        Task<Donor?> GetDonorByIdAsync(int donorId);
        Task<Donor> GetDonorByEmailAsync(string email);
        Task AddDonorAsync(Donor donor);
        Task UpdateDonorAsync(Donor donor);
        Task DeleteAsync(Donor donor);


        Task AddDonorHistoryAsync(DonorHistory history);
        // Task<IEnumerable<DonorHistory>> GetHistoryByDonorIdAsync(int donorId);
        Task<List<DonorHistory>> GetHistoryByDonorIdAsync(int donorId);

    }
}
