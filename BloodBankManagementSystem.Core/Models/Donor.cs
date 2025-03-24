namespace BloodBankManagementSystem.Core.Models{
    public class Donor
    {
        public int DonorID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string BloodType { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime? LastDonationDate { get; set; }
    }
}
