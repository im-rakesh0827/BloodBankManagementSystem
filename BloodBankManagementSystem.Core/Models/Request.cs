namespace BloodBankManagementSystem.Core.Models{
    public class Request
    {
        public int RequestID { get; set; }
        public int PatientID { get; set; }
        public int BloodID { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
        
        // Navigation Properties (Optional)
        public Patient? Patient { get; set; }
        public BloodInventory? BloodInventory { get; set; }
    }
}

