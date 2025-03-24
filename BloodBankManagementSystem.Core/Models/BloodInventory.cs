namespace BloodBankManagementSystem.Core.Models{
    public class BloodInventory
    {
        public int BloodID { get; set; }
        public string BloodType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
