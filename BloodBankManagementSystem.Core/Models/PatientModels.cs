using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BloodBankManagementSystem.Core.Models
{

    public class Patient
    {
        public int PatientID { get; set; }
        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters.")]
        public string LastName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required.")]
        public string Email{get; set;} = string.Empty;
        [Required(ErrorMessage = "Age is required.")]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Weight is required.")]
        [Range(5, 120, ErrorMessage = "Weight must be between 5 and 120.")]
        public decimal Weight { get; set; }
        [Required(ErrorMessage = "Blood Type is required.")]
        public string BloodTypeNeeded { get; set; } = string.Empty;
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone Number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsAlive { get; set; } = true;
        [Required(ErrorMessage = "Country is required.")]
        public string Country { get; set; } = string.Empty;
        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; } = string.Empty;
        [Required(ErrorMessage = "District is required.")]
        public string District { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pincode is required.")]
        public string PinCode { get; set; } = string.Empty;
    }


    public class PatientHistory
{
     public int Id { get; set; }
     public int PatientId{get; set;}
    public DateTime ActionDate { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string ActionUser { get; set; } = string.Empty;
    public string ActionNote { get; set; } = string.Empty;
}
    
}