using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Promise.ProductManagementSystem.Helpers;

namespace Promise.ProductManagementSystem.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<EmailSender> _logger;
        public EmailSender(EmailConfiguration emailConfig, IWebHostEnvironment environment, ILogger<EmailSender> logger)
        {
            _emailConfig = emailConfig;
            _environment = environment;
            _logger = logger;
        }
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = CreateEmailMessage(new Message(new string[] { email }, subject, htmlMessage));
            await Send(emailMessage);
        }
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Coselat", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private Task Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    if (_environment.IsDevelopment())
                    {
                        // Papercut configuration for development
                        _logger.LogInformation("Using Papercut SMTP for development - connecting to {Server}:{Port}",
                            _emailConfig.SmtpServer, _emailConfig.Port);
                        client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.None);
                    }
                    else
                    {
                        // Production SMTP configuration (Gmail/other providers)
                        _logger.LogInformation("Using production SMTP - connecting to {Server}:{Port}",
                            _emailConfig.SmtpServer, _emailConfig.Port);
                        client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                        client.AuthenticationMechanisms.Remove("XOAUTH2");
                        client.Authenticate(_emailConfig.Username, _emailConfig.Password);
                    }

                    client.Send(mailMessage);
                    _logger.LogInformation("Email sent successfully to {Recipients}",
                        string.Join(", ", mailMessage.To.Select(x => x.ToString())));

                    return Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email to {Recipients}",
                        string.Join(", ", mailMessage.To.Select(x => x.ToString())));
                    throw new InvalidOperationException($"Failed to send email: {ex.Message}", ex);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}