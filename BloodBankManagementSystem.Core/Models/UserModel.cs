 using System;
using System.ComponentModel.DataAnnotations;
namespace BloodBankManagementSystem.Core.Models
{
    
    public class RegisterRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; } = "User"; // Optional: defaults to "User"
    }

    public class TokenResponse
{
    public string Token { get; set; } = string.Empty;
    public string Expiration { get; set; } = string.Empty;
}



     public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


    public class AuthResponseDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }





    public class User
    {
        public int Id { get; set; }

        // public string Username { get; set; }    

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = string.Empty;

        // [Required(ErrorMessage = "Password is required.")]
        // [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string PasswordHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        public string Address { get; set; } = string.Empty;

        // [Required(ErrorMessage = "Role is required.")]
        // [RegularExpression("^(User|Admin)$", ErrorMessage = "Role must be either 'User' or 'Admin'.")]
        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // [Required]
        public string CreatedBy { get; set; } = "System";

        // [Required]
        public string UpdatedBy { get; set; } = "System";

        public bool IsActive { get; set; } = true;
        public bool IsAlive { get; set; } = true;

        // [Required(ErrorMessage = "Country is required.")]
        // [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters.")]
        public string Country { get; set; } = string.Empty;

        // [Required(ErrorMessage = "State is required.")]
        // [StringLength(100, ErrorMessage = "State cannot exceed 100 characters.")]
        public string State { get; set; } = string.Empty;

        // [Required(ErrorMessage = "District is required.")]
        // [StringLength(100, ErrorMessage = "District cannot exceed 100 characters.")]
        public string District { get; set; } = string.Empty;

        // [Required(ErrorMessage = "PinCode is required.")]
        // [StringLength(10, ErrorMessage = "PinCode cannot exceed 10 characters.")]
        public string PinCode { get; set; } = string.Empty;
    }


}
