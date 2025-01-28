using PGPARS.Data;

namespace PGPARS.Services
{
    public class AuditLogService
    {
        private readonly IAuditRepository _auditRepository;
        public AuditLogService(IAuditRepository auditRepo)
        {
            _auditRepository = auditRepo;
        }

        public void LogAction(string action, string user, string details, string category)
        {
            _auditRepository.LogAction(action, user, details, category);
        }

    }
}
