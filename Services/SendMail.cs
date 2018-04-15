using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SendGrid;
using SendGrid.Helpers.Mail;

namespace LanfeustBridge.Services
{
    public class SendMail : IEmailSender
    {
        private ILogger _logger;
        private string _sendGridKey;

        public SendMail(IConfiguration configuration, ILogger<SendMail> logger)
        {
            _sendGridKey = configuration["SendGrid:ApiKey"];
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogInformation("Sending mail to {address} about '{subject}'", email, subject);

            var message = new SendGridMessage();
            message.SetFrom("webmaster@LanfeustBridge.com", "Lanfeust Bridge");
            message.AddTo(email);
            message.SetSubject(subject);
            message.AddContent(MimeType.Html, htmlMessage);

            var sendGridClient = new SendGridClient(_sendGridKey);
            var response = await sendGridClient.SendEmailAsync(message);

            var responseContent = await response.Body.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Accepted)
                _logger.LogInformation("Mail sent successfully", responseContent);
            else
                _logger.LogWarning("Mail not sent, {status} {reason}", response.StatusCode, responseContent);
        }
    }
}
