using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Infrastructure.Repositories;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BloodBankManagementSystem.API.Services; 
using Microsoft.Extensions.Logging; // Make sure to include this

namespace BloodBankManagementSystem.API.Controllers{
     [Route("api/[controller]")]
    [ApiController]
     public class PatientsController : ControllerBase{
        private readonly IPatientRepository _patientRepository;
        private readonly ILogger<PatientsController> _logger; 
        private readonly IEmailService _emailService;
        public PatientsController(IPatientRepository patientRepository, IEmailService emailService, ILogger<PatientsController> logger)
        {
            _patientRepository = patientRepository;
            _emailService = emailService;
            _logger = logger;
        }
        // Register a new patient
        [HttpPost("register")]
        public async Task<IActionResult> RegisterPatient([FromBody] Patient patient)
        {
            if (patient == null)
            {
                return BadRequest("Invalid patient data.");
            }
            if (await _patientRepository.GetPatientByEmailAsync(patient.Email) != null)
                    return BadRequest("Patient already exists");
            var patientId = await _patientRepository.RegisterPatientAsync(patient);
            bool emailSent = await SendEmail(patient);
            if (!emailSent)
            {
                return StatusCode(500, new { message = "Donor registered, but email failed to send" });
            }
            return Ok(new { message = "Patient registered successfully, email sent" });
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

            var existingPatient = await _patientRepository.GetPatientByIdAsync(id);
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
            existingPatient.Country = updatedPatient.Country;
            existingPatient.State = updatedPatient.State;
            existingPatient.District = updatedPatient.District;
            existingPatient.PinCode = updatedPatient.PinCode;
            existingPatient.CreatedBy = updatedPatient.CreatedBy;
            existingPatient.CreatedAt = updatedPatient.CreatedAt;
            existingPatient.UpdatedBy = updatedPatient.UpdatedBy;
            existingPatient.UpdatedAt = updatedPatient.UpdatedAt;
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
            var patient = await _patientRepository.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound(new { message = "Patient not found." });
            }
            await _patientRepository.DeleteAsync(patient);
            return Ok(new { message = "Patient deleted successfully." });
        }

        private async Task<bool> SendEmail(Patient patient)
        {
            try
            {
                string subject = "🩸 Welcome to BloodBank Management System!";
                string body = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f9f9f9;
                            padding: 20px;
                            text-align: center;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: auto;
                            background: #ffffff;
                            padding: 20px;
                            border-radius: 10px;
                            box-shadow: 0px 0px 10px rgba(0,0,0,0.1);
                        }}
                        h2 {{
                            color: #d9534f;
                        }}
                        p {{
                            color: #333;
                            font-size: 16px;
                        }}
                        .cta-button {{
                            display: inline-block;
                            background-color: #d9534f;
                            color: #ffffff;
                            padding: 12px 24px;
                            text-decoration: none;
                            font-weight: bold;
                            border-radius: 5px;
                            margin-top: 20px;
                        }}
                        .info-box {{
                            background-color: #f8d7da;
                            color: #721c24;
                            padding: 15px;
                            border-radius: 5px;
                            margin-top: 15px;
                        }}
                        .footer {{
                            margin-top: 20px;
                            text-align: center;
                            font-size: 12px;
                            color: #777;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h2>Welcome, {patient.FirstName}! 🏥</h2>
                        <p>Thank you for registering with <strong>BloodBank Management System</strong>.</p>
                        
                        <div class='info-box'>
                            <p>Your account has been successfully created! Here are your details:</p>
                            <ul style='text-align: left; list-style: none; padding: 0;'>
                                <li><strong>Name:</strong> {patient.FirstName} {patient.LastName}</li>
                                <li><strong>Email:</strong> {patient.Email}</li>
                                <li><strong>Blood Group:</strong> {patient.BloodTypeNeeded}</li>
                            </ul>
                        </div>

                        <p>You can now log in to access your dashboard and update your information.</p>
                        <a href='https://your-bloodbank-system.com/login' class='cta-button'>Login to Your Account</a>
                        
                        <p class='footer'>If you have any questions, feel free to reply to this email.<br> BloodBank Management System Team</p>
                    </div>
                </body>
                </html>";

                bool emailSent = await _emailService.SendEmailAsync(patient.Email, subject, body);
                if (!emailSent)
                {
                    _logger.LogError($"Failed to send email to {patient.Email}");
                    return false;
                }
                _logger.LogInformation($"Email sent successfully to {patient.Email}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while sending email: {ex.Message}");
                return false;
            }
    }
    }    
}