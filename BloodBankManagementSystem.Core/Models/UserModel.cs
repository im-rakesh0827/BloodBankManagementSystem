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


        public string OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }

    }


    public class UserHistory
{
     public int Id { get; set; }
     public int UserId{get; set;}
    public DateTime ActionDate { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string ActionUser { get; set; } = string.Empty;
    public string ActionNote { get; set; } = string.Empty;
}


// public class EmailRequest
// {
//     // [Required(ErrorMessage = "Email is required")]
//     // [EmailAddress(ErrorMessage = "Invalid email")]
//     public string Email { get; set; }
// }

// public class OtpVerificationRequest
// {
//     public string Email { get; set; }
//     public string Otp { get; set; }
// }

// public class ResetPasswordAfterOtpRequest
// {
//     public string Email { get; set; }
//     public string Otp { get; set; }
//     public string NewPassword { get; set; }
// }

public class ForgotPasswordRequest
{
    public string Email { get; set; }
}

public class VerifyOtpRequest
{
    public string Email { get; set; }
    public string Otp { get; set; }
}

public class ResetPasswordRequest
{
    public string Email { get; set; }
    public string Otp { get; set; }
    public string NewPassword { get; set; }
}

public class EncryptRequest
{
    public string PlainText { get; set; }
}

public class DecryptRequest
{
    public string CipherText { get; set; }
}


public class PincodeApiRoot
{
    public string Message { get; set; }
    public string Status { get; set; }
    public List<PostOffice> PostOffice { get; set; }
}

public class PostOffice
{
    public string Name { get; set; }
    public string District { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string BranchType { get; set; }
    public string DeliveryStatus { get; set; }
    public string Circle { get; set; }
    public string Division { get; set; }
    public string Region { get; set; }
    public string Block { get; set; }
    public string Pincode { get; set; }
}


}
