using Microsoft.AspNetCore.Mvc;
using BloodBankManagementSystem.Core.Interfaces;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BloodBankManagementSystem.API.Services; 
using Microsoft.Extensions.Logging; 
namespace BloodBankManagementSystem.API.Controllers{

     [Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(IUserRepository userRepository, JwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            Console.WriteLine("✅ API Login method hit!");

            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials.");
            }

            string token = _jwtTokenService.GenerateToken(user.Id.ToString(), user.Email);
        //    return Ok(new { Token = token });

            // var accessToken = GenerateJwtToken(user);
            // var refreshToken = GenerateRefreshToken();
            var accessToken = token;
            return Ok(new
            {
                token_type = "Bearer",
                access_token = accessToken,
                expires_in = DateTime.Now.AddHours(9),
            });
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
    

    private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
    {
        try
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var enteredHash = System.Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(enteredPassword)));
                return enteredHash == storedPasswordHash;
            }
        }
        catch (System.Exception)
        {
            
            throw;
        }
    }
}
}


