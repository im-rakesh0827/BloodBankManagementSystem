using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BloodBankManagementSystem.API.Services;
using Microsoft.AspNetCore.Authorization;


namespace BloodBankManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, IEmailService emailService, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
                if (existingUser != null)
                    return BadRequest(new { message = "User already exists" });

                var plainPassword = user.PasswordHash; // Save plain password for email (not recommended in real apps)

                user.PasswordHash = HashPassword(plainPassword);
                user.IsActive = true;
                user.CreatedAt = DateTime.Now;

                await _userRepository.AddUserAsync(user);

                await AddHistory(user.Id, "System", "Register", "User registered");

                var emailSent = await SendEmail(user, plainPassword);
                if (emailSent)
                {
                    await AddHistory(user.Id, "System", "Email Sent", "Welcome email sent");
                }
                else
                {
                    return StatusCode(500, new { message = "User registered, but failed to send email" });
                }

                return Ok(new { message = "User registered successfully and email sent" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpGet("{email}")]
        public async Task<IActionResult> GetUserByEmailAsync(string email){
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                return Ok(user);

            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        [HttpGet("allUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                if (users == null || !users.Any())
                {
                    return NotFound(new { message = "No users found" });
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] User updatedUser)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByIdAsync(userId);
                if (existingUser == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.Email = updatedUser.Email;
                existingUser.Phone = updatedUser.Phone;
                existingUser.Role = updatedUser.Role;
                existingUser.Address = updatedUser.Address;
                existingUser.UpdatedAt = DateTime.Now;

                if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
                {
                    existingUser.PasswordHash = HashPassword(updatedUser.PasswordHash);
                }

                await _userRepository.UpdateUserAsync(existingUser);
                await AddHistory(existingUser.Id, "System", "Update", "User details updated");

                return Ok(new { message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                user.IsActive = false;
                await _userRepository.UpdateUserAsync(user);
                await AddHistory(user.Id, "System", "Delete", "User deactivated");

                return Ok(new { message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("history/{userId}")]
        public async Task<ActionResult<IEnumerable<UserHistory>>> GetHistoryByUserId(int userId)
        {
            try
            {
                var history = await _userRepository.GetUserHistoryByIdAsync(userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user history");
                return StatusCode(500, new { message = "Internal server error" });
            }
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
            var hashedInput = HashPassword(inputPassword);
            return hashedInput == storedHash;
        }

        private async Task<bool> SendEmail(User user, string plainPassword)
        {
            try
            {
                var subject = "🎉 Welcome to Vital Drop";
                var body = $@"
                <html>
                <body>
                    <div style='font-family: Arial, sans-serif;'>
                        <h2>Welcome, {user.FirstName}!</h2>
                        <p>Thank you for registering with <strong>Vital Drop</strong>.</p>
                        <p>You can now log in to your account using your email: <strong>{user.Email}</strong>.</p>
                        <p>We recommend changing your password after first login for security reasons.</p>
                        <a href='https://your-bloodbank-system.com/login' style='background-color: #d9534f; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Login Now</a>
                        <p style='font-size: 12px; color: #777;'>If you have questions, reply to this email.</p>
                    </div>
                </body>
                </html>";

                var result = await _emailService.SendEmailAsync(user.Email, subject, body);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {user.Email}");
                return false;
            }
        }

        private async Task AddHistory(int userId, string actionUser, string actionType, string actionNote)
        {
            var history = new UserHistory
            {
                ActionDate = DateTime.Now,
                ActionType = actionType,
                ActionUser = actionUser,
                ActionNote = actionNote,
                UserId = userId
            };
            await _userRepository.AddUserHistoryAsync(history);
        }
    }
}
