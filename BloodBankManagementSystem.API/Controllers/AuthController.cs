using BloodBankManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.API.Services;

using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;


        public AuthController(IUserRepository userRepository, IJwtService jwtService, ILogger<AuthController> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginRequest)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
                if (user == null || !VerifyPassword(loginRequest.Password, user.PasswordHash))
                    return Unauthorized(new { message = "Invalid credentials" });

                var token = _jwtService.GenerateJwtToken(user);
                return Ok(new AuthResponseDto
                {
                    Token = token,
                    UserId = user.Id,
                    UserName = $"{user.FirstName} {user.LastName}",
                    Role = user.Role
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var hashedInput = HashPassword(inputPassword);
            return hashedInput == storedHash;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
