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

        public IActionResult Index()
        {
            var logs = _auditRepository.GetLogs();
            return View(logs);
        }

      

    }
}
