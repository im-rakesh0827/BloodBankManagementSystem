using BloodBankManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.API.Services;

using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BloodBankManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailService _emailService;


        public AuthController(IUserRepository userRepository, IJwtService jwtService, ILogger<AuthController> logger, IEmailService emailService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
            _emailService = emailService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginRequest)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
                if (user == null || !EncryptionHelper.VerifyPassword_SHA256(loginRequest.Password, user.PasswordHash))
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



    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] ForgotPasswordRequest request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var otp = new Random().Next(100000, 999999).ToString();
        var expiry = DateTime.UtcNow.AddMinutes(5);
        string hashedOtp = EncryptionHelper.HashPassword_BCrypt(otp);

        await _userRepository.UpdateOtpAsync(request.Email, hashedOtp, expiry);
        await _emailService.SendEmailAsync(request.Email, "Your OTP Code", $"Your OTP code is: {otp}");
        return Ok("OTP sent successfully");
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        // var isValid = await _userRepository.VerifyOtpAsync(request.Email, request.Otp);
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        bool isValid = EncryptionHelper.VerifyKey_BCrypt(request.Otp, user.OtpCode);

        if (!isValid)
        {
            return BadRequest("Invalid or expired OTP");
        }
        return Ok("OTP verified successfully");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound("User not found");
        }
        var passwordHash = EncryptionHelper.HashPassword_SHA256(request.NewPassword);
        await _userRepository.UpdatePasswordAsync(request.Email, passwordHash);
        return Ok("Password reset successfully");
    }








    [HttpPost("encrypt")]
    public IActionResult Encrypt([FromBody] EncryptRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.PlainText))
            return BadRequest("PlainText cannot be empty.");

        var encrypted = EncryptionHelper.Encrypt(request.PlainText);
        return Ok(new { EncryptedValue = encrypted });
    }

    [HttpPost("decrypt")]
    public IActionResult Decrypt([FromBody] DecryptRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CipherText))
            return BadRequest("CipherText cannot be empty.");

        try
        {
            var decrypted = EncryptionHelper.Decrypt(request.CipherText);
            return Ok(new { DecryptedValue = decrypted });
        }
        catch
        {
            return BadRequest("Invalid cipher text or decryption failed.");
        }
    }

    }
}
