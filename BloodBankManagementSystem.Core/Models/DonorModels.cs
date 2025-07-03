using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankManagementSystem.Core.Models
{

    public class Donor
    {
        public int Id { get; set; }
        // Personal Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        
        // Location Information
        public string Country { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string PinCode { get; set; }
        public string Address { get; set; }

        // Medical & Donation Information
        public string BloodGroup { get; set; }
        public decimal Weight { get; set; } // In kg
        public DateTime? LastDonationDate { get; set; }
        public string MedicalHistory { get; set; } = string.Empty;

        // Status & Audit Information
        public bool IsEligible { get; set; }
        public bool IsActive { get; set; }  = true;
        public bool IsAlive { get; set; } = true;
        public string CreatedBy { get; set; } = string.Empty;
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }

    public class DonorHistory
{
     public int Id { get; set; }
     public int DonorId{get; set;}
    public DateTime ActionDate { get; set; }
    public string ActionType { get; set; } = string.Empty;
    public string ActionUser { get; set; } = string.Empty;
    public string ActionNote { get; set; } = string.Empty;
}
    
}