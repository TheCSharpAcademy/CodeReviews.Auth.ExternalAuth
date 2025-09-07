using Promise.ProductManagementSystem.Helpers;

namespace Promise.ProductManagementSystem.Services
{
    public interface IEmailSender : Microsoft.AspNetCore.Identity.UI.Services.IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
