using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.Infrastructure.Data;

namespace BloodBankManagementSystem.Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;
        public PatientRepository(AppDbContext context)
        {
          _context = context;
        }
        public async Task<int> RegisterPatientAsync(Patient patient)
        {
          patient.CreatedAt = DateTime.UtcNow;
          patient.UpdatedAt = DateTime.UtcNow;
          patient.CreatedBy = "Admin";
          patient.UpdatedBy = "Admin";
          _context.Patients.Add(patient);
          await _context.SaveChangesAsync();
          return patient.PatientID;
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
          return await _context.Patients.ToListAsync();
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
          return await _context.Patients.FindAsync(id);
        }

        public async Task<Patient?> FindByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task UpdateAsync(Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Patient patient)
        {
          _context.Patients.Remove(patient);
          await _context.SaveChangesAsync();
        }
    }
}
