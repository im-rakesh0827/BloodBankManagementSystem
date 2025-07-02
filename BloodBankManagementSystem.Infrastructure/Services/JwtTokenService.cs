// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using Microsoft.IdentityModel.Tokens;

// namespace BloodBankManagementSystem.Infrastructure.Services
// {
//     public class JwtTokenService
//     {
//         private const string SecretKey = "YourSuperSecretKey123!";
//         private const string Issuer = "yourapi";
//         private const string Audience = "yourapp";

//         public string GenerateToken(string email, string role, int userId)
// {
//     var claims = new[]
//     {
//         new Claim(ClaimTypes.Name, email),
//         new Claim(ClaimTypes.Role, role),
//         new Claim(ClaimTypes.NameIdentifier, userId.ToString())
//     };

//     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey123!"));
//     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//     var token = new JwtSecurityToken(
//         issuer: "yourapi",
//         audience: "yourapp",
//         claims: claims,
//         expires: DateTime.UtcNow.AddHours(1),
//         signingCredentials: creds);

//     return new JwtSecurityTokenHandler().WriteToken(token);
// }

//     }
// }
