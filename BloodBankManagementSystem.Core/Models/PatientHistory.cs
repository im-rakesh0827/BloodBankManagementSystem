namespace BloodBankManagementSystem.Core.Models{
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
