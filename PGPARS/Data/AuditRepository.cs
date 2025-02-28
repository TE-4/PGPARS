using Microsoft.EntityFrameworkCore;
using PGPARS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGPARS.Data
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string action, string user, string details, string category)
        {
            var log = new AuditLog
            {
                Action = action,
                Actor = user ?? "System",
                TimeStamp = DateTime.Now,
                Details = details,
                Category = category
            };
            await _context.AuditLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetLogsAsync()
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.TimeStamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetLogsByFiltersAsync(string category, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(a => a.Category == category);
            }

            if (startDate.HasValue)
            {
                query = query.Where(a => a.TimeStamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                // Ensure we include logs from the entire day (if filtering by day only)
                query = query.Where(a => a.TimeStamp <= endDate.Value.AddDays(1).AddTicks(-1));
            }

            return await query.OrderByDescending(a => a.TimeStamp).ToListAsync();
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _context.AuditLogs
                .Select(a => a.Category)
                .Distinct()
                .ToListAsync();
        }






    }
}
