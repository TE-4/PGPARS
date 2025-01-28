using PGPARS.Models;

namespace PGPARS.Data
{
    public interface IAuditRepository
    {
        void LogAction(string action, string user, string details);
        IEnumerable<AuditLog> GetLogs();
    }
}
