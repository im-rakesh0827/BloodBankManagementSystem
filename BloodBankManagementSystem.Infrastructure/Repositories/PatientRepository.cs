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
          try
          {
            _context = context;
          }
          catch (System.Exception)
          {
            throw;
          }
        }
        public async Task<int> RegisterPatientAsync(Patient patient)
        {
          try
          {
            patient.CreatedAt = DateTime.Now;
            patient.CreatedBy = "Admin";
            patient.UpdatedBy = "Admin";
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient.PatientID;
          }
          catch (System.Exception)
          {
            
            throw;
          }
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
          try
          {
            return await _context.Patients.ToListAsync();
          }
          catch (System.Exception)
          {
            throw;
          }
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
          try
          {
            return await _context.Patients.FindAsync(id);
          }
          catch (System.Exception)
          {
            throw;
          }
        }

        public async Task<Patient?> GetPatientByEmailAsync(string email)
        {
          try
          {
            return await _context.Patients.FirstOrDefaultAsync(u => u.Email == email);
          }
          catch (System.Exception)
          {
            throw;
          }
        }

        public async Task UpdateAsync(Patient patient)
        {
            try
            {
              _context.Patients.Update(patient);
              await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
              throw;
            }
        }

        public async Task DeleteAsync(Patient patient)
        {
          try
          {
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
          }
          catch (System.Exception)
          {
            throw;
          }
        }


        public async Task AddPatientHistoryAsync(PatientHistory history)
        {
            try
            {
              _context.PatientHistories.Add(history);
            await _context.SaveChangesAsync();
            }
            catch (System.Exception)
            {
              throw;
            }
        }

        public async Task<List<PatientHistory>> GetHistoryPatientIdAsync(int patientId)
        {
            try
            {
              return await _context.PatientHistories
                                .Where(h => h.PatientId == patientId)
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
