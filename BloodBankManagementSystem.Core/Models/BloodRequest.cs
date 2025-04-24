namespace BloodBankManagementSystem.Core.Models{
     public class BloodRequest
{
    public int Id { get; set; }
    public int? RequesterId { get; set; }  = 101;
    public string? RequesterType { get; set; } ="Patient";
    public string PatientName { get; set; }
    public int? PatientId { get; set; } 
    public int? DonorId { get; set; } 
    public string Gender { get; set; }
    public string BloodGroup { get; set; }
    public int UnitsRequired { get; set; }
    public string Location { get; set; }
    public string ContactNumber { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? FullAddress { get; set; } = string.Empty;
    public string Description { get; set; }
    public DateTime RequestedDate { get; set; } = DateTime.Now;
    public DateTime? ApprovedDate { get; set; } // Nullable if not yet approved
    public string? CreatedBy {get; set;} = "System";
    public string? UpdatedBy {get; set;} = "Admin";
    public DateTime? UpdatedAt {get; set;}
    public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected"
    // public bool? IsActive {get; set;} = true;
}

}
