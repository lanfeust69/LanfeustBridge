using SendGrid;
using SendGrid.Helpers.Mail;

namespace LanfeustBridge.Services;

public class SendMail : IEmailSender
{
    private readonly ILogger _logger;
    private readonly string _sendGridKey;

    public SendMail(IConfiguration configuration, ILogger<SendMail> logger)
    {
        _sendGridKey = configuration["SendGrid:ApiKey"];
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation("Sending mail to {Address} about '{Subject}'", email, subject);

        var message = new SendGridMessage();
        message.SetFrom("webmaster@LanfeustBridge.com", "Lanfeust Bridge");
        message.AddTo(email);
        message.SetSubject(subject);
        message.AddContent(MimeType.Html, htmlMessage);

        var sendGridClient = new SendGridClient(_sendGridKey);
        var response = await sendGridClient.SendEmailAsync(message);

        var responseContent = await response.Body.ReadAsStringAsync();
        if (response.StatusCode == HttpStatusCode.Accepted)
            _logger.LogInformation("Mail sent successfully with content {Content}", responseContent);
        else
            _logger.LogWarning("Mail not sent, {Status} {Reason}", response.StatusCode, responseContent);
    }
}
