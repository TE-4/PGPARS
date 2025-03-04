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

        public async Task LogAction(string action, string user, string details, string category)
        {
            await _auditRepository.LogActionAsync(action, user, details, category);
        }

    }
}
