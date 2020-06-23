using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;

namespace glimpse.Models.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageGmailOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageGmailOptions Options { get; } // Set only via Secret Manager

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.GmailUser, Options.GmailKey, "", subject, message, email);
        }

        public Task Execute(string apiUser, string apiKey, string fromEmail, string subject, string message, string email)
        {
            var mailMessage = new System.Net.Mail.MailMessage
            {
                From = new System.Net.Mail.MailAddress(apiUser),
                Subject = subject,
                Body = message,
                IsBodyHtml = false
            };
            mailMessage.To.Add(email);
            mailMessage.ReplyToList.Add(fromEmail);

            var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(mailMessage);

            var gmailMessage = new Google.Apis.Gmail.v1.Data.Message
            {
                Raw = Encode(mimeMessage.ToString())
            };

            var service = new Google.Apis.Gmail.v1.GmailService();
            Google.Apis.Gmail.v1.UsersResource.MessagesResource.SendRequest request
                = service.Users.Messages.Send(gmailMessage, apiUser);

            request.Execute();

            return Task.CompletedTask;
        }

        public static string Encode(string text)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);

            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}