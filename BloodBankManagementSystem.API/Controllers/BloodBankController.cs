using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Infrastructure.Repositories;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace BloodBankManagementSystem.API.Controllers{
     [Route("api/[controller]")]
    [ApiController]
     public class BloodBankController : ControllerBase{
        private readonly IPatientRepository _patientRepository;
        public BloodBankController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        // Register a new patient
        [HttpPost("register")]
        public async Task<IActionResult> RegisterPatient([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest("Invalid patient data.");
            }
            var patientId = await _patientRepository.RegisterPatientAsync(patient);
            return Ok(new { message = "Patient registered successfully", patientId });
        }
        // Get all patients
        [HttpGet("allPatients")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientRepository.GetAllPatientsAsync();
            return Ok(patients);
        }
        // Get patient by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
                return NotFound("Patient not found");
            return Ok(patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            if (updatedPatient == null || id != updatedPatient.PatientID)
            {
                return BadRequest("Invalid patient data or ID mismatch.");
            }

            var existingPatient = await _patientRepository.FindByIdAsync(id);
            if (existingPatient == null)
            {
                return NotFound("Patient not found.");
            }

            // Update patient details
            existingPatient.FirstName = updatedPatient.FirstName;
            existingPatient.LastName = updatedPatient.LastName;
            existingPatient.Age = updatedPatient.Age;
            existingPatient.BloodTypeNeeded = updatedPatient.BloodTypeNeeded;
            existingPatient.PhoneNumber = updatedPatient.PhoneNumber;
            existingPatient.Address = updatedPatient.Address;

            try
            {
                await _patientRepository.UpdateAsync(existingPatient);
                return Ok(new { message = "Patient details updated successfully." });
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the patient details.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _patientRepository.FindByIdAsync(id);
            
            if (patient == null)
            {
                return NotFound(new { message = "Patient not found." });
            }

            await _patientRepository.DeleteAsync(patient);

            return Ok(new { message = "Patient deleted successfully." });
        }

    }    
}