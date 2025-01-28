using PGPARS.Models;

namespace PGPARS.Data
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void LogAction(string action, string user, string details)
        {
            var log = new AuditLog
            {
                Action = action,
                User = user ?? "System", // If there is no User, default to "System"
                TimeStamp = DateTime.Now,
                Details = details
            };
            _context.AuditLogs.Add(log);
            _context.SaveChanges();
        }

        public IEnumerable<AuditLog> GetLogs()
        {
            // This should hopefully return the logs in descending order of TimeStamp
            return _context.AuditLogs.OrderByDescending(a => a.TimeStamp).ToList();
        }
    }
}
