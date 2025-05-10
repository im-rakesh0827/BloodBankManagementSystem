namespace BloodBankManagementSystem.Core.Models{

     public class BloodRequestHistory
     {
          public int Id { get; set; }
          public int RequestId{get; set;}
          public DateTime ActionDate { get; set; }
          public string ActionType { get; set; } = string.Empty;
          public string ActionUser { get; set; } = string.Empty;
          public string ActionNote { get; set; } = string.Empty;
     }
}

