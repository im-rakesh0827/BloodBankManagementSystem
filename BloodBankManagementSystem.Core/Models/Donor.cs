namespace BloodBankManagementSystem.Core.Models{
    public class Donor
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string PinCode { get; set; } = string.Empty;
    public string BloodGroup { get; set; } = string.Empty; // A+, O-, B+, etc.
    public DateTime? LastDonationDate { get; set; } // Nullable, as a new donor may not have donated yet
    public bool IsEligibleToDonate { get; set; } = true; // System will update based on donation history
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; } = "System";
    public string UpdatedBy { get; set; } = "System";
    public bool IsActive { get; set; } = true; // If donor wants to deactivate their profile
}

}
