namespace BloodBankManagementSystem.Core.Models{
     public class BloodRequest
{
    public int Id { get; set; }
    public int PatinetId{get;set;}
    public string PatientName { get; set; }
    public string BloodGroup { get; set; }
    public int UnitsRequired { get; set; }
    public string Location { get; set; }
    public string ContactNumber { get; set; }
    public DateTime RequestedDate { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
}
}
