namespace BloodBankManagementSystem.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone {get;set;}
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
    }
}
