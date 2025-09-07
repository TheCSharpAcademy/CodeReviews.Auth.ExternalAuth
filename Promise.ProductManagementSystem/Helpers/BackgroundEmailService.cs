using Microsoft.AspNetCore.Identity;
using Promise.ProductManagementSystem.Services;

namespace Promise.ProductManagementSystem.Helpers
{
    public class BackgroundEmailService : BackgroundService
    {
        private readonly ILogger<BackgroundEmailService> _logger;
        private readonly EmailQueue _queue;
        private readonly IServiceProvider _serviceProvider;

        public BackgroundEmailService(ILogger<BackgroundEmailService> logger, EmailQueue queue, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _queue = queue;
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Email sending service started");
            while(!stoppingToken.IsCancellationRequested)
            {
                if(_queue.TryDequeueEmail(out var email))
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();
                        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                        await emailSender.SendEmailAsync(email.To, email.Subject, email.Body);
                        _logger.LogInformation("Email sent to {to}", email.To);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error sending email to {to}", email.To);
                    }
                }
                else
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            _logger.LogInformation("Email sending service stopping");

        }
    }
}
