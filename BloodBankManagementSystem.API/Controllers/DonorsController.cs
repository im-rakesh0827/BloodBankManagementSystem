using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using BloodBankManagementSystem.API.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorsController : ControllerBase
    {
        private readonly IDonorRepository _donorRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<DonorsController> _logger;

        public DonorsController(IDonorRepository donorRepository, IEmailService emailService, ILogger<DonorsController> logger)
        {
            _donorRepository = donorRepository;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterDonor([FromBody] Donor donor)
        {
            try
            {
               if (await _donorRepository.GetDonorByEmailAsync(donor.Email) != null)
                    return BadRequest("Donor already exists");

               await _donorRepository.AddDonorAsync(donor);
               bool emailSent = await SendEmail(donor);
               if (!emailSent)
               {
                    return StatusCode(500, new { message = "Donor registered, but email failed to send" });
               }
               return Ok(new { message = "Donor registered successfully, email sent" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering donor: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        // Get all donors
        [HttpGet("allDonors")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllDonors()
        {
            try
            {
                var donors = await _donorRepository.GetAllDonorsAsync();
                if (donors == null || !donors.Any())
                {
                    return NotFound(new { message = "No donors found." });
                }

                return Ok(donors);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching donors: {ex.Message}");
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }

        // Get donor by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDonorById(int id)
        {
            var donor = await _donorRepository.GetDonorByIdAsync(id);
            if (donor == null)
                return NotFound(new { message = "Donor not found" });

            return Ok(donor);
        }

        // Update donor
        [HttpPut("update/{donorId}")]
        public async Task<IActionResult> UpdateDonor(int donorId, [FromBody] Donor updatedDonor)
        {
            var existingDonor = await _donorRepository.GetDonorByIdAsync(donorId);
            if (existingDonor == null)
            {
                return NotFound(new { message = "Donor not found." });
            }

            // Update fields
            existingDonor.FirstName = updatedDonor.FirstName;
            existingDonor.LastName = updatedDonor.LastName;
            existingDonor.Email = updatedDonor.Email;
            existingDonor.Phone = updatedDonor.Phone;
            existingDonor.BloodGroup = updatedDonor.BloodGroup;
            existingDonor.Gender = updatedDonor.Gender;
            existingDonor.Age = updatedDonor.Age;
            existingDonor.Weight = updatedDonor.Weight;
            existingDonor.Address = updatedDonor.Address;
            existingDonor.Country = updatedDonor.Country;
            existingDonor.State = updatedDonor.State;
            existingDonor.District = updatedDonor.District;
            existingDonor.PinCode = updatedDonor.PinCode;
            existingDonor.LastDonationDate = updatedDonor.LastDonationDate;
            existingDonor.IsEligible = updatedDonor.IsEligible;
            existingDonor.MedicalHistory = updatedDonor.MedicalHistory;
            existingDonor.IsActive = updatedDonor.IsActive;

            await _donorRepository.UpdateDonorAsync(existingDonor);
            return Ok(new { message = "Donor updated successfully" });
        }

        // Delete donor
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            var donor = await _donorRepository.GetDonorByIdAsync(id);
            if (donor == null)
            {
                return NotFound(new { message = "Donor not found." });
            }

            await _donorRepository.DeleteAsync(donor);
            return Ok(new { message = "Donor deleted successfully." });
        }

        // Send welcome email to donor
        private async Task<bool> SendEmail(Donor donor)
        {
            try
            {
                string subject = "🩸 Thank You for Registering as a Blood Donor!";
                string body = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            padding: 20px;
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
                        <h2>Welcome, {donor.FirstName}! 🩸</h2>
                        <p>Thank you for registering as a <strong>Blood Donor</strong>. Your contribution can save lives!</p>
                        <p>Here are your details:</p>
                        <ul>
                            <li><strong>Name:</strong> {donor.FirstName} {donor.LastName}</li>
                            <li><strong>Email:</strong> {donor.Email}</li>
                            <li><strong>Blood Group:</strong> {donor.BloodGroup}</li>
                            <li><strong>Last Donation Date:</strong> {donor.LastDonationDate?.ToString("yyyy-MM-dd") ?? "N/A"}</li>
                        </ul>
                        <p>Please keep your profile updated and help us when there is a need for blood.</p>
                        <a href='https://your-bloodbank-system.com/login' class='cta-button'>Update Profile</a>
                        <p class='footer'>If you have any questions, feel free to reply to this email. <br> BloodBank Management System Team</p>
                    </div>
                </body>
                </html>";

                bool emailSent = await _emailService.SendEmailAsync(donor.Email, subject, body);
                if (!emailSent)
                {
                    _logger.LogError($"Failed to send email to {donor.Email}");
                    return false;
                }

                _logger.LogInformation($"Email sent successfully to {donor.Email}");
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
