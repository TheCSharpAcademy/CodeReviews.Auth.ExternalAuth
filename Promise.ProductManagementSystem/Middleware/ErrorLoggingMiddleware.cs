using System.Security.Authentication;
using Promise.ProductManagementSystem.Data;

namespace Promise.ProductManagementSystem.Middleware
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLoggingMiddleware> _logger;
        public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, ApplicationDbContext db)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An unhandled exception occurred.");

                // Attempt to persist an audit entry but do not let DB failures hide the original error.
                try
                {
                    var auditLog = new Models.AuditLog
                    {
                        Id = Guid.NewGuid(),
                        Timestamp = DateTime.UtcNow,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace,
                        ExceptionType = ex.GetType().ToString()
                    };
                    db.AuditLogs.Add(auditLog);
                    await db.SaveChangesAsync();
                }
                catch (Exception dbEx)
                {
                    _logger.LogError(dbEx, "Failed to persist audit log for exception.");
                }

                // If the response already started, we can't change it; rethrow so upstream can handle properly.
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Response has already started. Rethrowing exception so existing pipeline can handle it.");
                    throw;
                }

                // If this looks like an authentication/remote auth failure (user cancelled, provider denied, etc.),
                // redirect back to the login page with a friendly message instead of returning a raw 500.
                var isAuthRelated =
                    ex is AuthenticationException ||
                    ex.GetType().Name.Contains("RemoteAuthenticationException", StringComparison.OrdinalIgnoreCase) ||
                    (ex.Message?.IndexOf("access_denied", StringComparison.OrdinalIgnoreCase) >= 0);

                if (isAuthRelated)
                {
                    context.Response.Redirect("/Identity/Account/Login");
                    return;
                }

                // Default behavior: return 500 with minimal message.
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
