using System.Collections.Generic;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;

namespace BloodBankManagementSystem.Infrastructure.Repositories
{
    public interface IPatientRepository
    {
        Task<int> RegisterPatientAsync(Patient patient);
        Task<List<Patient>> GetAllPatientsAsync();
        Task<Patient> GetPatientByIdAsync(int id);
        Task<Patient?> FindByIdAsync(int id);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(Patient patient);
    }
}
