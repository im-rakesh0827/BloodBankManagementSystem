using System.Threading.Tasks;
namespace BloodBankManagementSystem.API.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }
}
