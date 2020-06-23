using System.Threading.Tasks;

namespace glimpse.Models.Services
{
    public interface IEmailSender
    {
        Task Execute(string apiUser, string apiKey, string fromEmail, string subject, string message, string email);
        Task SendEmailAsync(string email, string subject, string message);
    }
}