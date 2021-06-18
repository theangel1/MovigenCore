using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace MovigenCore.Utility
{
    public class EmailSender : IEmailSender
    {
       
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute( subject, message, email);
        }
        public Task Execute( string subject, string message, string email)
        {
            var client = new SendGridClient("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");
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("sisgenchile@dtecore.com", "Sisgen Chile Team"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));
            
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
