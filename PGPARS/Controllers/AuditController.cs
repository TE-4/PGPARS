using Microsoft.AspNetCore.Mvc;
using PGPARS.Data;

namespace PGPARS.Controllers
{
    public class AuditController : Controller
    {
        private readonly IAuditRepository _auditRepository;

        public AuditController(IAuditRepository auditRepo)
        {
            _auditRepository = auditRepo;
        }

        public IActionResult Index(string filter)
        {
            if(filter != null)
            {
                var filteredLogs = _auditRepository.GetLogs().Where(log => log.Category == filter);
                return View(filteredLogs);
            }
            var logs = _auditRepository.GetLogs();
            return View(logs);
        }

      

    }
}
