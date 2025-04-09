using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MimeKit.Text;

namespace ProductManagement.hasona23.Services.Email;

public class EmailSender : IEmailSender
{
    private EmailConfig _emailConfig;
    private ILogger<EmailSender> _logger;
    public EmailSender(EmailConfig emailConfig, ILogger<EmailSender> logger)
    {
        _logger = logger;
        try
        {
            _emailConfig = emailConfig;
            _logger.LogInformation($"Email Sender Initialised Successfully");
        }
        catch (Exception ex)
        {
            logger.LogCritical($"Error Initing Email Sender {ex}");
        }
    }
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation($"Sending an email to {email}");
        var message = new MimeMessage
        {
            Subject = subject,
            Body = new TextPart(TextFormat.Html) { Text = htmlMessage },
        };
        message.From.Add(new MailboxAddress("Sender Name", "sender@email.com"));
        message.To.Add(new MailboxAddress("Receiver Name", "receiver@email.com"));

        try
        {
            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                await smtp.SendAsync(message);
                smtp.Disconnect(true);
                _logger.LogInformation($"Email sent successfully to {email}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send email to {email}: {ex.Message}");
        }
    }

}