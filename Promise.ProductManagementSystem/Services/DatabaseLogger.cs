
using Promise.ProductManagementSystem.Data;
using Promise.ProductManagementSystem.Models;

namespace Promise.ProductManagementSystem.Services
{
    public class DatabaseLogger : IDatabaseLogger
    {
        private readonly ApplicationDbContext _context;

        public DatabaseLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Log(Exception ex)
        {
            var auditLog = new AuditLog
            {
                Message = ex.Message ?? "Unexpected error occured",
                StackTrace = ex.StackTrace ?? "No stack trace available",
                Timestamp = DateTime.UtcNow,
            };

            _context.AuditLogs.Add(auditLog);
            _context.SaveChanges();
        }
    }
}
