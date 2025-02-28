﻿using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index(string filter, DateTime? startDate, DateTime? endDate)
        {
            Console.WriteLine($"Filter: {filter}, StartDate: {startDate}, EndDate: {endDate}");

            var categories = await _auditRepository.GetCategoriesAsync();
            ViewBag.Categories = categories;

            // Fetch logs based on selected category and date range
            var logs = await _auditRepository.GetLogsByFiltersAsync(filter, startDate, endDate);

            return View(logs);
        }



    }
}
