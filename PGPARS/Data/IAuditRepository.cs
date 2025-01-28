using PGPARS.Models;

namespace PGPARS.Data
{
    public interface IAuditRepository
    {
        void LogAction(string action, string user, string details, string category);
        IEnumerable<AuditLog> GetLogs();
    }
}
