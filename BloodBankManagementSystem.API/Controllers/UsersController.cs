using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BloodBankManagementSystem.API.Services; 
using Microsoft.Extensions.Logging; // Make sure to include this

namespace BloodBankManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger; // Inject Logger
        private readonly IEmailService _emailService;
        public UsersController(IUserRepository userRepository, IEmailService emailService, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try{
                if (await _userRepository.GetUserByEmailAsync(user.Email) != null)
                return BadRequest("User already exists");
                string UserPassword = user.PasswordHash;
                user.PasswordHash = HashPassword(user.PasswordHash);
                await _userRepository.AddUserAsync(user);
                user.PasswordHash = UserPassword;
                bool emailSent = await SendEmail(user);
                if (!emailSent)
                {
                    return StatusCode(500, new { message = "User registered, but email failed to send" });
                }
                return Ok(new { message = "User registered successfully, email sent" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registering user: {ex.Message}");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("allUsers")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();

                if (users == null || !users.Any())  // Fix null check
                {
                    return NotFound(new { message = "No users found." });
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
            if (user == null || !VerifyPassword(loginRequest.PasswordHash, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            return Ok(new { message = "Login successful", role = user.Role });
        }


        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] User updatedUser)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found." });
            }
            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.Email = updatedUser.Email;
            existingUser.Phone = updatedUser.Phone;
            existingUser.Role = updatedUser.Role;
            existingUser.Address = updatedUser.Address;
            if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
            {
                existingUser.PasswordHash = HashPassword(updatedUser.PasswordHash);
            }
            await _userRepository.UpdateUserAsync(existingUser);
            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }
            // await _userRepository.DeleteAsync(user);
            user.IsActive = false;
            await _userRepository.UpdateUserAsync(user);
            return Ok(new { message = "User deleted successfully." });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            return HashPassword(inputPassword) == storedHash;
        }

    private async Task<bool> SendEmail(User user)
    {
        try
        {
            string subject = "🎉 Welcome to BloodBank Management System!";
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
                    <h2>Welcome, {user.FirstName}! 🎉</h2>
                    <p>Thank you for registering with <strong>BloodBank Management System</strong>. 🩸</p>
                    <p>Here are your details:</p>
                    <ul>
                        <li><strong>Name:</strong> {user.FirstName} {user.LastName}</li>
                        <li><strong>Email:</strong> {user.Email}</li>
                        <li><strong>Password:</strong> {user.PasswordHash}</li>
                    </ul>
                    <p>You can now log in and explore the system and reset your password</p>
                    <a href='https://your-bloodbank-system.com/login' class='cta-button'>Login to Your Account</a>
                    <p class='footer'>If you have any questions, feel free to reply to this email. <br> BloodBank Management System Team</p>
                </div>
            </body>
            </html>";
            bool emailSent = await _emailService.SendEmailAsync(user.Email, subject, body);
            if (!emailSent)
            {
                _logger.LogError($"Failed to send email to {user.Email}");
                return false;
            }
            _logger.LogInformation($"Email sent successfully to {user.Email}");
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
