using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PGPARS.Data;
using PGPARS.Models;
using System.Threading.Tasks;

namespace PGPARS.Controllers
{
    public class AuditController : Controller
    {
        private readonly IAuditRepository _auditRepository;

        public AuditController(IAuditRepository auditRepo)
        {
            _auditRepository = auditRepo;
        }

        public async Task<IActionResult> Index(string filter)
        {
            // Debugging 
            Console.WriteLine($"Filter: {filter}");

            var categories = await _auditRepository.GetCategoriesAsync();
            ViewBag.Categories = categories;

            // Fetch logs, filtered if a category is selected
            var logs = string.IsNullOrEmpty(filter)
                ? await _auditRepository.GetLogsAsync()
                : await _auditRepository.GetLogsByCategoryAsync(filter);



            return View(logs);
        }

      

    }
}
