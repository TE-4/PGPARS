using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PGPARS.Data;
using PGPARS.Infrastructure;
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

        public async Task<IActionResult> Index(List<string>? filters, string? searchTerm,
            DateTime? startDate, DateTime? endDate, 
            int page = 1, int pageSize = 20)
        {

            filters ??= new List<string>();

            var categories = await _auditRepository.GetCategoriesAsync();
            ViewBag.Categories = categories;

            // Fetch filtered logs
            var logs = await _auditRepository.GetLogsByFiltersAsync(filters, searchTerm, startDate, endDate);

            int totalItems = logs.Count();
            var pagedLogs = logs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var model = new PaginatedList<AuditLog>(pagedLogs, totalItems, page, pageSize);

            ViewBag.Filters = filters;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");


            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSelectedLogs(List<int> SelectedLogs)
        {
            if (SelectedLogs == null || !SelectedLogs.Any())
            {
                TempData["ErrorMessage"] = "No logs selected";
                return RedirectToAction("Index");
            }

            await _auditRepository.DeleteLogsAsync(SelectedLogs);

            TempData["SuccessMessage"] = $"{SelectedLogs.Count} logs deleted.";
            return RedirectToAction("Index");
        }



    }
}
